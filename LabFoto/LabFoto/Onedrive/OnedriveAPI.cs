using LabFoto.Data;
using LabFoto.Models.Tables;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LabFoto.Onedrive
{
    public class OnedriveAPI
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient client;

        public OnedriveAPI(ApplicationDbContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;

            // Este cliente vai ser utilizado para envio e recepção pedidos Http
            client = _clientFactory.CreateClient();
        }

        public async Task<bool> GetThumbnailsAsync(List<Fotografia> photos)
        {
            try
            {
                foreach (var photo in photos)
                {
                    // Verificar se o token está válido
                    await RefreshTokenAsync(photo.ContaOnedrive);
                    
                    string driveId = photo.ContaOnedrive.DriveId;
                    string url = "https://graph.microsoft.com/v1.0/me" +
                        "/drives/" + driveId +
                        "/items/" + photo.ItemId +
                        "/thumbnails";
                    var request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Headers.Add("Authorization", "Bearer " + photo.ContaOnedrive.AccessToken);

                    // Fazer o pedido e obter resposta
                    var response = await client.SendAsync(request);

                    // Caso retorne OK 2xx
                    if (response.IsSuccessStatusCode)
                    {
                        // Converter a resposta para um objeto json
                        JObject content = JObject.Parse(await response.Content.ReadAsStringAsync());

                        JObject value = (JObject)content["value"][0];
                        photo.Thumbnail_Large = (string)value["large"]["url"];
                        photo.Thumbnail_Medium = (string)value["medium"]["url"];
                        photo.Thumbnail_Small = (string)value["small"]["url"];

                        _context.Update(photo);
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
                    string url = "https://login.microsoftonline.com/21e90dfc-54f1-4b21-8f3b-7fb9798ed2e0/oauth2/v2.0/token";
                    var request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Content = new StringContent(
                        "client_id=2da7484c-9eea-49a3-b337-f59a97f79e47" +
                        "&scope=offline_access+files.read+files.read.all" +
                        "&refresh_token=" + conta.RefreshToken +
                        "&redirect_uri=" + "https://localhost:44354/ContasOnedrive/Create" +
                        "&grant_type=refresh_token" +
                        "&client_secret=3*4Mm%3DHY8M4%40%2FgcZ3GdV*BO7l0%5DvKeu0",
                        Encoding.UTF8, "application/x-www-form-urlencoded"
                    );

                    // Fazer o pedido e obter resposta
                    var response = await client.SendAsync(request);

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
                // Inicializar o pedido com o codigo recebido quando foram pedidas as permissões ao utilizador
                string url = "https://login.microsoftonline.com/21e90dfc-54f1-4b21-8f3b-7fb9798ed2e0/oauth2/v2.0/token";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Content = new StringContent(
                    "client_id=2da7484c-9eea-49a3-b337-f59a97f79e47" +
                    "&scope=offline_access+files.read+files.read.all" +
                    "&code=" + code +
                    "&redirect_uri=" + "https://localhost:44354/ContasOnedrive/Create" +
                    "&grant_type=authorization_code" +
                    "&client_secret=3*4Mm%3DHY8M4%40%2FgcZ3GdV*BO7l0%5DvKeu0",
                    Encoding.UTF8, "application/x-www-form-urlencoded"
                );

                // Fazer o pedido e obter resposta
                var response = await client.SendAsync(request);

                // Caso retorne OK 200
                if (response.IsSuccessStatusCode)
                {
                    // Converter a resposta para um objeto json
                    return JObject.Parse(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }

        public async Task<JObject> GetDriveInfoAsync(string token)
        {
            try
            {
                // Inicializar o pedido com o token de autenticação
                var request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me/drives/");
                request.Headers.Add("Authorization", "Bearer " + token);

                // Fazer o pedido e obter resposta
                var response = await client.SendAsync(request);

                // Caso retorne OK 2xx
                if (response.IsSuccessStatusCode)
                {
                    // Converter a resposta para um objeto json
                    return JObject.Parse(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }

        public string GetPermissionsUrl()
        {
            string permissionsUrl =
                "https://login.microsoftonline.com/21e90dfc-54f1-4b21-8f3b-7fb9798ed2e0/oauth2/v2.0/authorize?" +
                "client_id=2da7484c-9eea-49a3-b337-f59a97f79e47" +
                "&response_type=code" +
                "&redirect_uri=https://localhost:44354/ContasOnedrive/Create&response_mode=query" +
                "&scope=offline_access%20files.read%20files.read.all%20files.readwrite%20files.readwrite.all" +
                "&state=12345";

            return permissionsUrl;
        }

        private bool GaleriaExists(int id)
        {
            return _context.Galerias.Any(e => e.ID == id);
        }
    }
}
