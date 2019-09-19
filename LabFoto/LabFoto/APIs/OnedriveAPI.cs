using LabFoto.Data;
using LabFoto.Models;
using LabFoto.Models.Tables;
using Microsoft.Extensions.Logging;
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

namespace LabFoto.APIs
{
    #region Interface
    public interface IOnedriveAPI
    {
        Task<bool> RefreshPhotoUrlsAsync(List<Fotografia> photos);
        Task<JObject> GetInitialTokenAsync(string code);
        Task<JObject> GetDriveInfoAsync(string token);
        Task<ContaOnedrive> GetAccountToUploadAsync(long fileSize);
        Task<string> GetUploadSessionAsync(ContaOnedrive conta, string fileName);
        string GetPermissionsUrl(int state = 0);
        Task<bool> DeleteFilesAsync(List<Fotografia> photos);
        Task<bool> UpdateDriveQuotaAsync(ContaOnedrive conta);
    }
    #endregion
    public class OnedriveAPI : IOnedriveAPI
    {
        #region Construtor
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _client;
        private readonly string _redirectUrl;
        private readonly AppSettings _appSettings;
        private readonly ILoggerAPI _logger;
        private readonly IEmailAPI _emailAPI;
        private readonly string _applicationClientId;
        private readonly string _applicationClientSecret;

        public OnedriveAPI(ApplicationDbContext context,
            IHttpClientFactory clientFactory,
            IOptions<AppSettings> settings,
            ILoggerAPI logger,
            IEmailAPI emailAPI)
        {
            _context = context;
            _appSettings = settings.Value;
            _emailAPI = emailAPI;
            _logger = logger;
            _applicationClientId = _appSettings.ApplicationClientId;
            _applicationClientSecret = _appSettings.ApplicationClientSecret;

            // Utilizado para fazer pedidos Http
            _client = clientFactory.CreateClient();

            // Url registado no portal da Azure
            _redirectUrl = _appSettings.SiteUrl + "/ContasOnedrive/Create";
        } 
        #endregion

        #region Thumbnails
        /// <summary>
        /// Refresca as thumbnails de uma lista de fotos.
        /// </summary>
        /// <param name="photos">Lista de fotos a serem refrecadas.</param>
        /// <returns></returns>
        public async Task<bool> RefreshPhotoUrlsAsync(List<Fotografia> photos)
        {
            try
            {
                foreach (var photo in photos)
                {
                    if (photo != null)
                    {
                        // Verificar se o token está válido
                        if(await RefreshTokenAsync(photo.ContaOnedrive, 2600))
                        {
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
                            else
                            {
                                // Caso não encontre a fotografia na onedrive, é porque esta não existe. Apaga o seu registo da BD
                                if ((int)response.StatusCode == 404)
                                {
                                    _context.Remove(photo);
                                }
                                else // Noutra situação, apenas apagar as thumbnails antigas para ser mostrado um thumbnail default
                                {
                                    await DeleteOldThumbnails(photo);
                                }
                            }
                            #endregion
                        }
                        else // Caso não consiga renovar o token, apaga as thumbnails antigas pois estas já não se encontram válidas
                        {
                            await DeleteOldThumbnails(photo);
                        }
                    }
                }
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                await _logger.LogError(
                    descricao: "Erro ao refrescar thumbnails.", 
                    classe: "OneDriveAPI", 
                    metodo: "RefreshPhotoUrlsAsync", 
                    erro: e.Message
                );
                return false;
            }
        }

        /// <summary>
        /// Apaga os urls das thumbnails antigos de uma fotografia
        /// </summary>
        /// <param name="photo">Fotografia que se pretende apagar urls.</param>
        private async Task DeleteOldThumbnails(Fotografia photo)
        {
            photo.DownloadUrl =
            photo.Thumbnail_Large =
            photo.Thumbnail_Medium =
            photo.Thumbnail_Small = null;

            _context.Update(photo);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Token
        /// <summary>
        /// Verifica se o token de uma conta já expirou
        /// </summary>
        /// <param name="conta"></param>
        /// <returns></returns>
        private bool IsTokenValid(ContaOnedrive conta, int seconds)
        {
            return ((DateTime.Now - conta.TokenDate).TotalSeconds < seconds);
        }

        /// <summary>
        /// Refresca o token de uma conta caso este esteja expirado.
        /// </summary>
        /// <param name="conta">Conta Onedrive.</param>
        /// <returns></returns>
        private async Task<bool> RefreshTokenAsync(ContaOnedrive conta, int seconds = 3400)
        {
            if (!IsTokenValid(conta, seconds))
            {
                #region Preparar pedido HTTP
                string url = "https://login.microsoftonline.com/21e90dfc-54f1-4b21-8f3b-7fb9798ed2e0/oauth2/v2.0/token";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Content = new StringContent(
                    "client_id=" + _applicationClientId +
                    "&scope=offline_access+files.read+files.read.all" +
                    "&refresh_token=" + conta.RefreshToken +
                    "&redirect_uri=" + _appSettings.SiteUrl +
                    "&grant_type=refresh_token" +
                    "&client_secret=" + _applicationClientSecret,
                    Encoding.UTF8, "application/x-www-form-urlencoded"
                );
                #endregion

                #region Fazer o pedido HTTP
                HttpResponseMessage response = null;
                try
                {
                    // Fazer o pedido e obter resposta
                    response = await _client.SendAsync(request);
                }
                catch (Exception e)
                {
                    await _logger.LogError(
                        descricao: "Erro no pedido HTTP de um token.",
                        classe: "OneDriveAPI",
                        metodo: "RefreshTokenAsync",
                        erro: e.Message
                    );
                    return false;
                }
                #endregion

                #region Tratar resposta
                if (response != null)
                {
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        try
                        {
                            JObject content = JObject.Parse(await response.Content.ReadAsStringAsync());

                            // Atualizar a data do token
                            conta.TokenDate = DateTime.Now;
                            conta.AccessToken = (string)content["access_token"];
                            conta.RefreshToken = (string)content["refresh_token"];
                        }
                        catch (Exception e)
                        {
                            await _logger.LogError(
                                descricao: "Erro ao interpretar o JSON da resposta.",
                                classe: "OneDriveAPI",
                                metodo: "RefreshTokenAsync",
                                erro: e.Message
                            );
                            return false;
                        }

                        try
                        {
                            _context.Update(conta);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception e)
                        {
                            await _logger.LogError(
                                descricao: "Erro ao dar update à conta Onedrive localmente.",
                                classe: "OneDriveAPI",
                                metodo: "RefreshTokenAsync",
                                erro: e.Message
                            );
                            return false;
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                #endregion
            }

            return true;
        }

        /// <summary>
        /// Pedir o primeiro token da conta via o Código returnado pelos servidores da Microsoft.
        /// </summary>
        /// <param name="code">Código retornado pela Microsoft.</param>
        /// <returns></returns>
        public async Task<JObject> GetInitialTokenAsync(string code)
        {
            #region Preparar pedido HTTP
            // Inicializar o pedido com o codigo recebido quando foram pedidas as permissões ao utilizador
            string url = "https://login.microsoftonline.com/21e90dfc-54f1-4b21-8f3b-7fb9798ed2e0/oauth2/v2.0/token";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Content = new StringContent(
                "client_id=" + _applicationClientId +
                "&scope=offline_access+files.read+files.read.all" +
                "&code=" + code +
                "&redirect_uri=" + _redirectUrl +
                "&grant_type=authorization_code" +
                "&client_secret=" + _applicationClientSecret,
                Encoding.UTF8, "application/x-www-form-urlencoded"
            );
            #endregion

            #region Fazer o pedido HTTP
            HttpResponseMessage response = null; ;
            try
            {
                // Fazer o pedido e obter resposta
                response = await _client.SendAsync(request);
            }
            catch (Exception e)
            {
                await _logger.LogError(
                    descricao: "Erro no pedido HTTP de um token.",
                    classe: "OneDriveAPI",
                    metodo: "GetInitialTokenAsync",
                    erro: e.Message
                );
                return null;
            }
            #endregion

            #region Tratar resposta
            // Caso retorne OK 200
            if (response != null)
            {
                if (response.IsSuccessStatusCode)
                {
                    // Converter a resposta para um objeto json
                    try
                    {
                        return JObject.Parse(await response.Content.ReadAsStringAsync());
                    }
                    catch (Exception e)
                    {
                        await _logger.LogError(
                            descricao: "Erro ao tratar o JSON da resposta.",
                            classe: "OneDriveAPI",
                            metodo: "GetInitialTokenAsync",
                            erro: e.Message
                        );
                        return null;
                    }
                }
            }
            #endregion

            return null;
        }
        #endregion

        #region DriveInfo
        /// <summary>
        /// Recolhe as informações necessárias da conta Onedrive para futuros pedidos.
        /// </summary>
        /// <param name="token">Token da conta Onedrive.</param>
        /// <returns></returns>
        public async Task<JObject> GetDriveInfoAsync(string token)
        {
            #region Preparar pedido HTTP
            // Inicializar o pedido com o token de autenticação
            var request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me/drives/");
            request.Headers.Add("Authorization", "Bearer " + token);
            #endregion

            #region Fazer o pedido HTTP
            HttpResponseMessage response = null;
            try
            {
                // Fazer o pedido
                response = await _client.SendAsync(request);
            }
            catch (Exception e)
            {
                await _logger.LogError(
                    descricao: "Erro no pedido HTTP das informações da drive.",
                    classe: "OneDriveAPI",
                    metodo: "GetDriveInfoAsync",
                    erro: e.Message
                );
                return null;
            }
            #endregion

            #region Interpretar resposta
            // Caso retorne OK 2xx
            if (response != null)
            {
                if (response.IsSuccessStatusCode)
                {
                    // Converter a resposta para um objeto json
                    try
                    {
                        return JObject.Parse(await response.Content.ReadAsStringAsync());
                    }
                    catch (Exception e)
                    {
                        await _logger.LogError(
                            descricao: "Erro ao interpretar o JSON da resposta.",
                            classe: "OneDriveAPI",
                            metodo: "GetDriveInfoAsync",
                            erro: e.Message
                        );
                        return null;
                    }
                }
            }
            #endregion

            return null;
        }

        /// <summary>
        /// Atualiza as informações sobre o espaço ocupado na conta Onedrive.
        /// </summary>
        /// <param name="conta">Conta Onedrive.</param>
        /// <returns></returns>
        public async Task<bool> UpdateDriveQuotaAsync(ContaOnedrive conta)
        {
            if (await RefreshTokenAsync(conta, 3400)) // Refrescar token
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

                    // Quando a conta só tiver 50 gb, envia email ao admins para notificar
                    if(Int64.Parse(quota_Remaining) < 53687091200)
                    {
                        _emailAPI.Send(_appSettings.AdminEmails, "Conta Onedrive com pouco espaço", $"A conta onedrive {conta.Username} já só tem menos de 50 Gb.");
                    }
                }
                catch (Exception e)
                {
                    await _logger.LogError(
                        descricao: "Erro ao interpretar o JSON da resposta.",
                        classe: "OneDriveAPI",
                        metodo: "UpdateDriveQuotaAsync",
                        erro: e.Message
                    );
                    return false;
                }

                try
                {
                    _context.Update(conta);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception e)
                {
                    await _logger.LogError(
                        descricao: "Erro ao dar update à conta Onedrive localmente.",
                        classe: "OneDriveAPI",
                        metodo: "UpdateDriveQuotaAsync",
                        erro: e.Message
                    );
                }
            }

            return false;
        }
        #endregion
        
        #region Permissions
        /// <summary>
        /// Retorna o url onde será necessário dar permissões à aplicação.
        /// </summary>
        /// <param name="state">Um valor que será retornado da API da Microsoft. Este será utilizado para identificar
        /// o ID da conta quando o cliente retorna à aplicação</param>
        /// <returns></returns>
        public string GetPermissionsUrl(int state = 0)
        {
            string permissionsUrl =
                "https://login.microsoftonline.com/21e90dfc-54f1-4b21-8f3b-7fb9798ed2e0/oauth2/v2.0/authorize?" +
                "client_id=" + _applicationClientId +
                "&response_type=code" +
                "&redirect_uri=" + _redirectUrl + "&response_mode=query" +
                "&scope=offline_access%20files.read%20files.read.all%20files.readwrite%20files.readwrite.all" +
                "&state=" + state;

            return permissionsUrl;
        }
        #endregion

        #region Upload

        /// <summary>
        /// Procura uma conta onedrive com espaço suficiente para alojar o tamanho do ficheiro que chega por parametro.
        /// </summary>
        /// <param name="fileSize">Tamanho do ficheiro utilizado para upload</param>
        /// <returns>Conta Onedrive</returns>
        public async Task<ContaOnedrive> GetAccountToUploadAsync(long fileSize)
        {
            ContaOnedrive conta = null;

            // Tenta encontrar conta com espaço
            try
            {
                while (true)
                {
                    conta = _context.ContasOnedrive.Where(c => Int64.Parse(c.Quota_Remaining) * 3 > fileSize).FirstOrDefault();

                    // Caso não encontre conta, é porque já não existem contas com espaço
                    if (conta == null)
                    {
                        _emailAPI.Send(_appSettings.AdminEmails, "Já não existem contas Onedrive com espaço", $"Todas as contas Onedrive estão cheias.");
                        return null;
                    }

                    // Confirmar que a conta tem mesmo o espaço suficiente
                    if (await UpdateDriveQuotaAsync(conta))
                    {
                        if (conta.Quota_Remaining != null && Int64.Parse(conta.Quota_Remaining) > fileSize)
                        {
                            return conta;
                        }
                        else
                        {
                            _emailAPI.Send(_appSettings.AdminEmails, "Conta Onedrive com pouco espaço", $"A conta onedrive {conta.Username} já só tem menos de 50 Gb.");
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                await _logger.LogError(
                    descricao: "Erro ao encontrar conta com espaço para upload.",
                    classe: "OneDriveAPI",
                    metodo: "GetAccountToUploadAsync",
                    erro: e.Message
                );
                return null;
            }
        }

        /// <summary>
        /// Cria uma sessão de upload
        /// </summary>
        /// <param name="conta">Conta Onedrive</param>
        /// <param name="fileName">Nome do ficheiro. Este irá ser utilizado na OneDrive.</param>
        /// <returns>Url da sessão</returns>
        public async Task<string> GetUploadSessionAsync(ContaOnedrive conta, string fileName)
        {
            if (await RefreshTokenAsync(conta, 1800)) // Refrescar token
            {
                #region Preparar pedido HTTP
                string url = "https://graph.microsoft.com/v1.0/me/drives/" + conta.DriveId + "/root:/" + fileName + ":/createUploadSession";
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("Authorization", "Bearer " + conta.AccessToken);
                // Especificar que em caso de nomes iguais, a onedrive altera o nome e carrega a imagem na mesma
                request.Content = new StringContent("{\"item\":{\"@microsoft.graph.conflictBehavior\": \"rename\"}}", Encoding.UTF8, "application/json");
                #endregion

                #region Fazer pedido HTTP
                HttpResponseMessage response = null;
                try
                {
                    // Fazer o pedido e obter resposta
                    response = await _client.SendAsync(request);
                }
                catch (Exception e)
                {
                    await _logger.LogError(
                        descricao: "Erro ao enviar pedido HTTP.",
                        classe: "OneDriveAPI",
                        metodo: "GetUploadSessionAsync",
                        erro: e.Message
                    );
                }
                #endregion

                #region Tratar resposta ao pedido
                // Caso retorne OK 2xx
                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            // Converter a resposta para um objeto json
                            JObject content = JObject.Parse(await response.Content.ReadAsStringAsync());

                            return (string)content["uploadUrl"];
                        }
                        catch (Exception e)
                        {
                            await _logger.LogError(
                                descricao: "Erro ao tratar o JSON da resposta.",
                                classe: "OneDriveAPI",
                                metodo: "GetUploadSessionAsync",
                                erro: e.Message
                            );
                        }
                    }
                }
            }

            return "Error";
            #endregion
        }
        #endregion Upload

        #region Delete

        /// <summary>
        /// Apaga ficheiros na onedrive e em seguida da base de dados.
        /// </summary>
        /// <param name="photos">Lista de fotografias a serem apagadas.</param>
        /// <returns>Verdade caso tenha sucesso, falso caso falhe.</returns>
        public async Task<bool> DeleteFilesAsync(List<Fotografia> photos)
        {
            try
            {
                foreach (Fotografia photo in photos)
                {
                    if (await RefreshTokenAsync(photo.ContaOnedrive, 3400)) // Refrescar token
                    { 
                        #region Preparar pedido HTTP
                        string driveId = photo.ContaOnedrive.DriveId;
                        string url = "https://graph.microsoft.com/v1.0/me/" +
                        "drives/" + driveId +
                        "/items/" + photo.ItemId;

                        var request = new HttpRequestMessage(HttpMethod.Delete, url);
                        request.Headers.Add("Authorization", "Bearer " + photo.ContaOnedrive.AccessToken);
                        #endregion

                        // Fazer o pedido e obter resposta
                        var response = await _client.SendAsync(request);

                        #region Tratar resposta
                        // Caso retorne OK 2xx
                        if (response.IsSuccessStatusCode)
                        {
                            // Este pedido nunca retorna json de resposta
                            // Remove ficheiro da base de dados
                            _context.Remove(photo);
                        }
                        #endregion

                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                await _logger.LogError(
                    descricao: "Erro ao apagar ficheiros na onedrive.",
                    classe: "OneDriveAPI",
                    metodo: "DeleteFiles",
                    erro: e.Message
                );
                return false;
            }

            return true;
        }
        #endregion

        #region Upload Back-end (Não utilizado)
        /// <summary>
        /// Upload do ficheiro para a Onedrive.
        /// </summary>
        /// <param name="filePath">Caminho do ficheiro no disco.</param>
        /// <param name="fileName">Nome do ficheiro.</param>
        /// <returns>Booleano que indica o sucesso do método.</returns>
        private async Task<UploadedPhotoModel> UploadFileAsync(string filePath, string fileName)
        {
            WebResponse result = null;
            ContaOnedrive conta = null;

            using (FileStream stream = File.OpenRead(filePath))
            {
                long position = 0;
                long totalLength = stream.Length;
                int uploadFragmentSizeInMB = _appSettings.UploadFragmentSizeInMB;
                int length = uploadFragmentSizeInMB * 1024 * 1024;

                #region Encontrar conta e refrescar token
                // Encontrar a conta onedrive a ser utilizada para o upload
                conta = await GetAccountToUploadAsync(totalLength);
                // Se a conta retornar null, poderá ser porque já não existem contas com espaço
                if (conta == null)
                {
                    return new UploadedPhotoModel
                    {
                        Success = false,
                        ErrorDescription = "Não foi possível adquirir uma conta com espaço suficiente."
                    };
                }
                await RefreshTokenAsync(conta, 2000);
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

                #region Upload do fragmento do ficheiro
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
                        try
                        {
                            StreamReader reader = new StreamReader(dataStream); // Abrir a stream
                            string responseFromServer = reader.ReadToEnd(); // Ler o conteudo  
                            JObject content = JObject.Parse(responseFromServer); // Converter a resposta para um objeto json
                            itemId = (string)content["id"]; // Recolher o id da imagem na onedrive
                            itemName = (string)content["name"]; // Recolher o nome da imagem na onedrive
                        }
                        catch (Exception e)
                        {
                            _emailAPI.NotifyError("Erro ao recolher as informações do ficheiro que foi feito o upload.", "OnedriveAPI", "UploadFileAsync", e.Message);
                            return new UploadedPhotoModel
                            {
                                Success = false,
                                ErrorDescription = "Não foi possível fazer upload do ficheiro."
                            };
                        }

                        if (!String.IsNullOrEmpty(itemId) && !String.IsNullOrEmpty(itemName))
                        {
                            #region Atualizar o espaço da conta
                            await UpdateDriveQuotaAsync(conta);
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
            catch (Exception e)
            {
                _emailAPI.NotifyError("Erro ao preparar os dados fragmentados do ficheiro para envio.", "OnedriveAPI", "UploadFileFragmentAsync", e.Message);
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

        /// <summary>
        /// Corre uma Thread que apaga todos os fiheiros passados no array de caminhos.
        /// </summary>
        /// <param name="paths">string[] - Todos os caminhos dos ficheiros a apagar.</param>
        private async Task DeleteFilesFromServerDisk(List<string> paths)
        {
            foreach (var path in paths)
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception e)
                {
                    await _logger.LogError(
                        descricao: "Erro ao tentar apagar um ficheiro.",
                        classe: "OneDriveAPI",
                        metodo: "DeleteFilesFromServerDisk",
                        erro: e.Message
                    );
                }
            }
        }
        #endregion
    }
}
