using LabFoto.Data;
using LabFoto.Models;
using LabFoto.Models.Tables;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LabFoto.Onedrive
{
    public class OnedriveAPI
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _client;
        private readonly string _redirectUrl = "";
        private readonly AppSettings _AppSettings;

        public OnedriveAPI(ApplicationDbContext context, IHttpClientFactory clientFactory, IOptions<AppSettings> settings)
        {
            _context = context;
            _clientFactory = clientFactory;
            _AppSettings = settings.Value;

            // Este cliente vai ser utilizado para envio e recepção pedidos Http
            _client = _clientFactory.CreateClient();

            _redirectUrl = _AppSettings.SiteUrl + "ContasOnedrive/InitAccount";
        } 

        #region Thumbnails
        public async Task<bool> RefreshPhotoUrlsAsync(List<Fotografia> photos)
        {
            try
            {
                foreach (var photo in photos)
                {
                    if (photo != null)
                    {
                        #region Refrescar token
                        // Verificar se o token está válido
                        await RefreshTokenAsync(photo.ContaOnedrive);
                        #endregion

                        #region Preparar pedido HTTP
                        string driveId = photo.ContaOnedrive.DriveId;
                        string url = "https://graph.microsoft.com/v1.0/me" +
                            "/drives/" + driveId +
                            "/items/" + photo.ItemId +
                            "?$expand=thumbnails";
                        var request = new HttpRequestMessage(HttpMethod.Get, url);
                        request.Headers.Add("Authorization", "Bearer " + photo.ContaOnedrive.AccessToken);
                        #endregion

                        // Fazer o pedido e obter resposta
                        var response = await _client.SendAsync(request);

                        #region Tratar resposta
                        // Caso retorne OK 2xx
                        if (response.IsSuccessStatusCode)
                        {
                            // Converter a resposta para um objeto json
                            JObject content = JObject.Parse(await response.Content.ReadAsStringAsync());

                            photo.DownloadUrl = (string)content["@microsoft.graph.downloadUrl"];

                            JObject thumbnails = (JObject)content["thumbnails"][0];
                            photo.Thumbnail_Large = (string)thumbnails["large"]["url"];
                            photo.Thumbnail_Medium = (string)thumbnails["medium"]["url"];
                            photo.Thumbnail_Small = (string)thumbnails["small"]["url"];

                            _context.Update(photo);
                        }
                        #endregion
                    }
                }
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        } 
        #endregion

        #region Token
        // Verifica se o token de uma conta já expirou
        private bool IsTokenValid(ContaOnedrive conta)
        {
            return ((DateTime.Now - conta.TokenDate).TotalSeconds < 3400);
        }

        private async Task<bool> RefreshTokenAsync(ContaOnedrive conta)
        {
            if (!IsTokenValid(conta))
            {
                try
                {
                    #region Preparar pedido HTTP
                    string url = "https://login.microsoftonline.com/21e90dfc-54f1-4b21-8f3b-7fb9798ed2e0/oauth2/v2.0/token";
                    var request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Content = new StringContent(
                        "client_id=2da7484c-9eea-49a3-b337-f59a97f79e47" +
                        "&scope=offline_access+files.read+files.read.all" +
                        "&refresh_token=" + conta.RefreshToken +
                        "&redirect_uri=" + _redirectUrl +
                        "&grant_type=refresh_token" +
                        "&client_secret=3*4Mm%3DHY8M4%40%2FgcZ3GdV*BO7l0%5DvKeu0",
                        Encoding.UTF8, "application/x-www-form-urlencoded"
                    );
                    #endregion

                    // Fazer o pedido e obter resposta
                    var response = await _client.SendAsync(request);

                    #region Tratar resposta
                    if (response.IsSuccessStatusCode)
                    {
                        JObject content = JObject.Parse(await response.Content.ReadAsStringAsync());

                        // Atualizar a data do token
                        conta.TokenDate = DateTime.Now;
                        conta.AccessToken = (string)content["access_token"];
                        conta.RefreshToken = (string)content["refresh_token"];

                        _context.Update(conta);
                        await _context.SaveChangesAsync();

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    #endregion
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<JObject> GetInitialTokenAsync(string code)
        {
            try
            {
                #region Preparar pedido HTTP
                // Inicializar o pedido com o codigo recebido quando foram pedidas as permissões ao utilizador
                string url = "https://login.microsoftonline.com/21e90dfc-54f1-4b21-8f3b-7fb9798ed2e0/oauth2/v2.0/token";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Content = new StringContent(
                    "client_id=2da7484c-9eea-49a3-b337-f59a97f79e47" +
                    "&scope=offline_access+files.read+files.read.all" +
                    "&code=" + code +
                    "&redirect_uri=" + _redirectUrl +
                    "&grant_type=authorization_code" +
                    "&client_secret=3*4Mm%3DHY8M4%40%2FgcZ3GdV*BO7l0%5DvKeu0",
                    Encoding.UTF8, "application/x-www-form-urlencoded"
                );
                #endregion

                // Fazer o pedido e obter resposta
                var response = await _client.SendAsync(request);

                #region Tratar resposta
                // Caso retorne OK 200
                if (response.IsSuccessStatusCode)
                {
                    // Converter a resposta para um objeto json
                    return JObject.Parse(await response.Content.ReadAsStringAsync());
                }
                #endregion
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }
        #endregion

        #region DriveInfo
        public async Task<JObject> GetDriveInfoAsync(string token)
        {
            try
            {
                #region Preparar pedido HTTP
                // Inicializar o pedido com o token de autenticação
                var request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me/drives/");
                request.Headers.Add("Authorization", "Bearer " + token);
                #endregion

                // Fazer o pedido
                var response = await _client.SendAsync(request);

                #region Interpretar resposta
                // Caso retorne OK 2xx
                if (response.IsSuccessStatusCode)
                {
                    // Converter a resposta para um objeto json
                    return JObject.Parse(await response.Content.ReadAsStringAsync());
                }
                #endregion
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        } 
        #endregion

        #region Permissions
        /// <summary>
        /// Retorna o url onde será necessário dar permissões à aplicação.
        /// </summary>
        /// <param name="returnUrl">Url para onde será redirecionado o utilizar após ceder permissões.</param>
        /// <param name="state">Um valor que será retornado da API da Microsoft. Este será utilizado para identificar
        /// o ID da conta quando o cliente retorna à aplicaçã0</param>
        /// <returns></returns>
        public string GetPermissionsUrl(int state = 0)
        {
            string permissionsUrl =
                "https://login.microsoftonline.com/21e90dfc-54f1-4b21-8f3b-7fb9798ed2e0/oauth2/v2.0/authorize?" +
                "client_id=2da7484c-9eea-49a3-b337-f59a97f79e47" +
                "&response_type=code" +
                "&redirect_uri=" + _redirectUrl + "&response_mode=query" +
                "&scope=offline_access%20files.read%20files.read.all%20files.readwrite%20files.readwrite.all" +
                "&state=" + state;

            return permissionsUrl;
        } 
        #endregion

        #region Upload

        /// <summary>
        /// Upload do ficheiro para a Onedrive.
        /// </summary>
        /// <param name="filePath">Caminho do ficheiro no disco.</param>
        /// <param name="fileName">Nome do ficheiro.</param>
        /// <returns>Booleano que indica o sucesso do método.</returns>
        public async Task<UploadedPhotoModel> UploadFileAsync(string filePath, string fileName)
        {
            WebResponse result = null;
            ContaOnedrive conta = null;

            using (FileStream stream = File.OpenRead(filePath))
            {
                long position = 0;
                long totalLength = stream.Length;
                int uploadFragmentSizeInMB = _AppSettings.UploadFragmentSizeInMB;
                int length = uploadFragmentSizeInMB * 1024 * 1024;

                #region Encontrar conta e refrescar token
                // Encontrar a conta onedrive a ser utilizada para o upload
                conta = GetAccountToUpload(totalLength);
                // Se a conta retornar null, poderá ser porque já não existem contas com espaço
                if (conta == null)
                {
                    // Verificar se as contas têm espaço
                    // Enviar e-mail ao administrador
                    return new UploadedPhotoModel
                    {
                        Success = false,
                        ErrorDescription = "Não foi possível adquirir uma conta com espaço suficiente."
                    };
                }
                await RefreshTokenAsync(conta);
                #endregion

                #region Criar sessão de upload
                string uploadUrl = await GetUploadSessionAsync(conta, fileName);
                if (uploadUrl == "Error")
                {
                    // Não foi possível fazer a sessão de upload
                    return new UploadedPhotoModel
                    {
                        Success = false,
                        ErrorDescription = "Não foi possível criar sessão de upload."
                    };
                }
                #endregion

                #region upload the file fragment
                while (true)
                {
                    byte[] bytes = await ReadFileFragmentAsync(stream, position, length);
                    if (position >= totalLength)
                    {
                        break;
                    }

                    result = await UploadFileFragmentAsync(bytes, uploadUrl, position, totalLength, conta.AccessToken);

                    position += bytes.Length;
                }
                #endregion

                #region Identificar as informações do ficheiro na onedrive
                if (result != null && conta != null)
                {
                    string itemId = "", itemName = "";
                    using (Stream dataStream = result.GetResponseStream()) // Recolher a stream que contem o conteudo 
                    {
                        StreamReader reader = new StreamReader(dataStream); // Abrir a stream
                        string responseFromServer = reader.ReadToEnd(); // Ler o conteudo  
                        JObject content = JObject.Parse(responseFromServer); // Converter a resposta para um objeto json
                        itemId = (string)content["id"]; // Recolher o id da imagem na onedrive
                        itemName = (string)content["name"]; // Recolher o nome da imagem na onedrive

                        if (!String.IsNullOrEmpty(itemId) && !String.IsNullOrEmpty(itemName))
                        {
                            #region Atualizar o espaço da conta
                            await UpdateDriveInfoAsync(conta);
                            #endregion

                            return new UploadedPhotoModel
                            {
                                ItemId = itemId,
                                ItemName = itemName,
                                Conta = conta,
                                Success = true
                            };
                        }
                    }
                    result.Close();
                }
                #endregion
            }

            return new UploadedPhotoModel
            {
                Success = false,
                ErrorDescription = "Não foi possível fazer upload do ficheiro."
            };
        }

        /// <summary>
        /// Procura uma conta onedrive com espaço suficiente para alojar o tamanho do ficheiro que chega por parametro.
        /// </summary>
        /// <param name="fileSize">Tamanho do ficheiro utilizado para upload</param>
        /// <returns>Conta Onedrive</returns>
        public ContaOnedrive GetAccountToUpload(long fileSize)
        {
            // Seleciona um conta com espaço sufeciente para o triplo do tamanho do ficheiro, como salvaguarda
            return _context.ContasOnedrive.Where(c => Int64.Parse(c.Quota_Remaining) * 3 > fileSize).FirstOrDefault();
        }

        /// <summary>
        /// Cria uma sessão de upload
        /// </summary>
        /// <param name="conta">Conta Onedrive</param>
        /// <param name="fileName">Nome do ficheiro. Este irá ser utilizado na OneDrive.</param>
        /// <returns>Url da sessão</returns>
        public async Task<string> GetUploadSessionAsync(ContaOnedrive conta, string fileName)
        {
            #region Preparar pedido HTTP
            string url = "https://graph.microsoft.com/v1.0/me/drives/" + conta.DriveId + "/root:/" + fileName + ":/createUploadSession";
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("Authorization", "Bearer " + conta.AccessToken);
            // Especificar que em caso de nomes iguais, a onedrive altera o nome e carrega a imagem na mesma
            request.Content = new StringContent("{\"item\":{\"@microsoft.graph.conflictBehavior\": \"rename\"}}", Encoding.UTF8, "application/json");
            #endregion

            // Fazer o pedido e obter resposta
            var response = await _client.SendAsync(request);

            #region Tratar resposta ao pedido
            // Caso retorne OK 2xx
            if (response.IsSuccessStatusCode)
            {
                // Converter a resposta para um objeto json
                JObject content = JObject.Parse(await response.Content.ReadAsStringAsync());

                return (string)content["uploadUrl"];
            }
            else
            {
                return "Error";
            }
            #endregion
        }

        /// <summary>
        /// upload file fragment
        /// </summary>
        /// <param name="datas">file fragment</param>
        /// <param name="uploadUri">upload uri</param>
        /// <param name="position">postion of the file bytes</param>
        /// <param name="totalLength">the file bytes lenght</param>
        /// <returns>expire time with json format</returns>
        private async Task<WebResponse> UploadFileFragmentAsync(byte[] data, string uploadUri, long position, long totalLength, string token)
        {
            #region Preparar pedido HTTP
            //var request = await InitAuthRequest(uploadUri, HTTPMethod.Put, datas, null);
            var request = WebRequest.Create(uploadUri) as HttpWebRequest;
            request.Method = "Put";

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Add("Authorization", $"bearer {token}");
            }
            request.Headers.Add("Content-Range", $"bytes {position}-{position + data.Length - 1}/{totalLength}");

            try
            {
                if (data != null)
                {
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(data, 0, data.Length);
                    dataStream.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            #endregion

            return await request.GetResponseAsync();
        }

        /// <summary>
        /// read file fragment
        /// </summary>
        /// <param name="stream">file stream</param>
        /// <param name="startPos">start position</param>
        /// <param name="count">take count</param>
        /// <returns>the fragment of file with byte[]</returns>
        private async Task<byte[]> ReadFileFragmentAsync(FileStream stream, long startPos, int count)
        {
            if (startPos >= stream.Length || startPos < 0 || count <= 0)
                return null;

            long trimCount = startPos + count > stream.Length ? stream.Length - startPos : count;

            byte[] retBytes = new byte[trimCount];
            stream.Seek(startPos, SeekOrigin.Begin);
            await stream.ReadAsync(retBytes, 0, (int)trimCount);
            return retBytes;
        }

        #endregion Upload

        #region DeleteFiles
        /// <summary>
        /// Corre uma Thread que apaga todos os fiheiros passados no array de caminhos.
        /// </summary>
        /// <param name="paths">string[] - Todos os caminhos dos ficheiros a apagar.</param>
        public void DeleteFiles(List<string> paths)
        {
            foreach (var path in paths)
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        } 
        #endregion

        #region AuxMethods
        private async Task<bool> UpdateDriveInfoAsync(ContaOnedrive conta)
        {
            if (IsTokenValid(conta))
            {
                try
                {
                    // Faz o pedido HTTP.GET para pedir as informações da Onedrive
                    // Para que estas possam ser associadas ao objeto 'conta'
                    JObject driveInfo = await GetDriveInfoAsync(conta.AccessToken);

                    // Transformar o array num array de objetos
                    JObject[] values = driveInfo["value"].Select(s => (JObject)s).ToArray();

                    JObject quota = (JObject)values[0]["quota"];
                    string quota_Total = (string)quota["total"];
                    string quota_Used = (string)quota["used"];
                    string quota_Remaining = (string)quota["remaining"];

                    // Atualizar a conta com as informações recolhidas da API da Onedrive
                    conta.Quota_Remaining = quota_Remaining;
                    conta.Quota_Total = quota_Total;
                    conta.Quota_Used = quota_Used;
                }
                catch (Exception)
                {
                    return false;
                }

                try
                {
                    _context.Update(conta);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }

        private bool GaleriaExists(int id)
        {
            return _context.Galerias.Any(e => e.ID == id);
        }
        #endregion
    }
}
