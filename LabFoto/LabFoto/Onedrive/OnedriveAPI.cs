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
            string token = "eyJ0eXAiOiJKV1QiLCJub25jZSI6IkFRQUJBQUFBQUFEQ29NcGpKWHJ4VHE5Vkc5dGUtN0ZYT3lVcW5rOVBpMkNUQ2t4Ym8yMjlBanh5STg0N3U4ZGhXN1JmYV81emQwa1RKSVpnWEl5TmxkbWJXQ2V3ZC00VVhKc2c3TVJYSzBXZjFFZHNvWmNKSWlBQSIsImFsZyI6IlJTMjU2IiwieDV0IjoiSEJ4bDltQWU2Z3hhdkNrY29PVTJUSHNETmEwIiwia2lkIjoiSEJ4bDltQWU2Z3hhdkNrY29PVTJUSHNETmEwIn0.eyJhdWQiOiJodHRwczovL2dyYXBoLm1pY3Jvc29mdC5jb20iLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8yMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAvIiwiaWF0IjoxNTU5MjI5MTA3LCJuYmYiOjE1NTkyMjkxMDcsImV4cCI6MTU1OTIzMzAwNywiYWNjdCI6MCwiYWNyIjoiMSIsImFpbyI6IkFTUUEyLzhMQUFBQWdQa2ovSzRQeStaNTlkWUtXWXdYclEzWDR4eTFGdWVuOUZZSUxidEZLVkU9IiwiYW1yIjpbInB3ZCJdLCJhcHBfZGlzcGxheW5hbWUiOiJMYWJGb3RvIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJhcHBpZGFjciI6IjEiLCJmYW1pbHlfbmFtZSI6IkFsZXhhbmRyZSIsImdpdmVuX25hbWUiOiJUZWxtbyIsImlwYWRkciI6IjE4OC4yNTEuMjI2LjEzOCIsIm5hbWUiOiJUZWxtbyBPbGl2ZWlyYSBBbGV4YW5kcmUiLCJvaWQiOiJjNzJkZTU2Yi01ZGZlLTQzMjgtOTMwZC0wNGEzZDcyZjI1MzMiLCJvbnByZW1fc2lkIjoiUy0xLTUtMjEtMzEwMDY3Mzc0Ni00MjcwNTgzNjI2LTI0MjUyNDIwNDktMTc4MjkiLCJwbGF0ZiI6IjMiLCJwdWlkIjoiMTAwMzdGRkU5MDg5NDYzNCIsInNjcCI6IkZpbGVzLlJlYWQgRmlsZXMuUmVhZC5BbGwgRmlsZXMuUmVhZFdyaXRlIEZpbGVzLlJlYWRXcml0ZS5BbGwgcHJvZmlsZSBvcGVuaWQgZW1haWwiLCJzaWduaW5fc3RhdGUiOlsia21zaSJdLCJzdWIiOiJZdVJhQVBLRVlKeU94eXlFY21kUjJNZG5hTE85NDk1QTNkQ2dFWlQ5MzlBIiwidGlkIjoiMjFlOTBkZmMtNTRmMS00YjIxLThmM2ItN2ZiOTc5OGVkMmUwIiwidW5pcXVlX25hbWUiOiJhbHVubzE5MDg5QGlwdC5wdCIsInVwbiI6ImFsdW5vMTkwODlAaXB0LnB0IiwidXRpIjoiQWoyNDhRamJNVXFjT2paWWpTMjJBQSIsInZlciI6IjEuMCIsInhtc19zdCI6eyJzdWIiOiJld2hsazdSYmhWN25YbTc0dTVTQUlQTjcwYktpbnBMWE1OQlB3VEFTUXRZIn0sInhtc190Y2R0IjoxNDI5MjY3MjA4fQ.P-oSYa3cSIU2Nm8SpF9hqbULVQKKTW0tqDXdm3QSNQFNvFALdT8ig0QBadSjsTumQpFDIBH_N-HNmlxevtstk6OsyxW-TAjhKY0paMClj7gK6zzrj36ytKq69aMSXCnF7XxkebCFt7F4-nk2zgYPt6qqyGkqCyLn42BdNfBn9eO_DjVJQzLe8x4YGX9K5DfCrjljTf2xGVET9yVlgwmIkvTmw0ONViF8I3XI6hE1JH6qoII_dIAiXYQgjVg-ndCOafXAFOMGda1LZL2jpA5b7Wtgtza7BtteQ8s2QzePhEuFxPAblmdvFVoM1Agnnqr5XyqhL_hB1CD7O5pPV4dEiQ";
            try
            {
                foreach (var photo in photos)
                {
                    // Verificar se o token está válido
                    await RefreshToken(photo.ContaOnedrive);
                    
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
            return ((DateTime.Now - conta.TokenDate).TotalSeconds < 3000);
        }

        private async Task<bool> RefreshToken(ContaOnedrive conta)
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



        public async Task<JObject> GetInitialToken(string code)
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

        public async Task<JObject> GetDriveInfo(string token)
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
