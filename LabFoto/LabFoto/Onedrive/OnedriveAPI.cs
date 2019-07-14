using LabFoto.Data;
using LabFoto.Models.Tables;
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
        private readonly HttpClient client;

        public OnedriveAPI(ApplicationDbContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;

            // Este cliente vai ser utilizado para envio e recepção pedidos Http
            client = _clientFactory.CreateClient();
        }

        public async Task<bool> RefreshPhotoUrlsAsync(List<Fotografia> photos)
        {
            try
            {
                foreach (var photo in photos)
                {
                    if(photo != null) { 
                        // Verificar se o token está válido
                        await RefreshTokenAsync(photo.ContaOnedrive);

                        string driveId = photo.ContaOnedrive.DriveId;
                        string url = "https://graph.microsoft.com/v1.0/me" +
                            "/drives/" + driveId +
                            "/items/" + photo.ItemId +
                            "?$expand=thumbnails";
                        var request = new HttpRequestMessage(HttpMethod.Get, url);
                        request.Headers.Add("Authorization", "Bearer " + photo.ContaOnedrive.AccessToken);

                        // Fazer o pedido e obter resposta
                        var response = await client.SendAsync(request);

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

        /// <summary>
        /// Upload file to onedrive
        /// </summary>
        /// <param name="filePath">file path in local dick</param>
        /// <returns>uploaded file info with json format</returns>
        public async Task<string> UploadFileAsync(string filePath)
        {
            #region create upload session
            string uploadUri = "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_api/v2.0/drives/b!0812_G3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29-ASFmlYSqg3p9xBheG7/items/0127OBJ5N6Y2GOVW7725BZO354PWSELRRZ/uploadSession?guid='e2583d2f-b520-44bf-b873-d8ff262428b3'&path='~tmp6E_tedsdsdsadasds.CR2'&overwrite=True&rename=False&dc=0&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTYzMTIzMDI2IiwiZXhwIjoiMTU2MzIwOTQyNiIsImVuZHBvaW50dXJsIjoiU2tDbmI1ZS9PU3FZOGZNemtKb2VWV1prUHQzZkRtNVV0N3FhcjZtU1R5Yz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjMxOCIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiTlRVeFl6QTBZVEl0WkRRNU1DMDBaVEptTFRnek4yRXRaR0kyTURVeU1XUmxNV00zIiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJnaXZlbl9uYW1lIjoiVGVsbW8iLCJmYW1pbHlfbmFtZSI6IkFsZXhhbmRyZSIsInNpZ25pbl9zdGF0ZSI6IltcImttc2lcIl0iLCJhcHBpZCI6IjJkYTc0ODRjLTllZWEtNDlhMy1iMzM3LWY1OWE5N2Y3OWU0NyIsInRpZCI6IjIxZTkwZGZjLTU0ZjEtNGIyMS04ZjNiLTdmYjk3OThlZDJlMCIsInVwbiI6ImFsdW5vMTkwODlAaXB0LnB0IiwicHVpZCI6IjEwMDM3RkZFOTA4OTQ2MzQiLCJjYWNoZWtleSI6IjBoLmZ8bWVtYmVyc2hpcHwxMDAzN2ZmZTkwODk0NjM0QGxpdmUuY29tIiwic2NwIjoibXlmaWxlcy5yZWFkIGFsbGZpbGVzLnJlYWQgbXlmaWxlcy53cml0ZSBhbGxmaWxlcy53cml0ZSIsInR0IjoiMiIsInVzZVBlcnNpc3RlbnRDb29raWUiOm51bGx9.VnkzTG9SRDlrNmt3Nk9kSzBuVjZyT2dOd2ZlckxxYlJ4dHBWR2JjdGR6QT0";
            #endregion

            #region upload the file fragment
            string result = string.Empty;
            using (FileStream stream = File.OpenRead(filePath))
            {
                long position = 0;
                long totalLength = stream.Length;
                int length = 3 * 1024 * 1024;

                while (true)
                {
                    byte[] bytes = await ReadFileFragmentAsync(stream, position, length);
                    if (position >= totalLength)
                    {
                        break;
                    }

                    result = await UploadFileFragmentAsync(bytes, uploadUri, position, totalLength);

                    position += bytes.Length;
                }
            }
            #endregion

            return result;
        }

        /// <summary>
        /// upload file fragment
        /// </summary>
        /// <param name="datas">file fragment</param>
        /// <param name="uploadUri">upload uri</param>
        /// <param name="position">postion of the file bytes</param>
        /// <param name="totalLength">the file bytes lenght</param>
        /// <returns>expire time with json format</returns>
        private async Task<string> UploadFileFragmentAsync(byte[] data, string uploadUri, long position, long totalLength)
        {
            string token = "eyJ0eXAiOiJKV1QiLCJub25jZSI6IkFRQUJBQUFBQUFBUDB3TGxxZExWVG9PcEE0a3d6U254LV91OXR3NlZ4UVBGdGxNOHZmUWhUSVBjVllBMGlOQzg2bVJnUVZnbjZLM3hqMzdUMzkxYXNmakNqamZwcWx5R0RFRmNvTExZTzQtVHZ6UkRtN3lBN1NBQSIsImFsZyI6IlJTMjU2IiwieDV0IjoidTRPZk5GUEh3RUJvc0hqdHJhdU9iVjg0TG5ZIiwia2lkIjoidTRPZk5GUEh3RUJvc0hqdHJhdU9iVjg0TG5ZIn0.eyJhdWQiOiJodHRwczovL2dyYXBoLm1pY3Jvc29mdC5jb20iLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8yMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAvIiwiaWF0IjoxNTYzMTIxNTMxLCJuYmYiOjE1NjMxMjE1MzEsImV4cCI6MTU2MzEyNTQzMSwiYWNjdCI6MCwiYWNyIjoiMSIsImFpbyI6IjQyRmdZSGk1V0thYVo3R3ZhbzNQQlJIOU85My96dDdzYnp2QUZEYS8wV0dWbmFGeXloMEEiLCJhbXIiOlsicHdkIl0sImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJhcHBpZCI6IjJkYTc0ODRjLTllZWEtNDlhMy1iMzM3LWY1OWE5N2Y3OWU0NyIsImFwcGlkYWNyIjoiMSIsImZhbWlseV9uYW1lIjoiQWxleGFuZHJlIiwiZ2l2ZW5fbmFtZSI6IlRlbG1vIiwiaXBhZGRyIjoiMTg4LjI1MS4yMjYuMTM4IiwibmFtZSI6IlRlbG1vIE9saXZlaXJhIEFsZXhhbmRyZSIsIm9pZCI6ImM3MmRlNTZiLTVkZmUtNDMyOC05MzBkLTA0YTNkNzJmMjUzMyIsIm9ucHJlbV9zaWQiOiJTLTEtNS0yMS0zMTAwNjczNzQ2LTQyNzA1ODM2MjYtMjQyNTI0MjA0OS0xNzgyOSIsInBsYXRmIjoiMyIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0Iiwic2NwIjoiRmlsZXMuUmVhZCBGaWxlcy5SZWFkLkFsbCBGaWxlcy5SZWFkV3JpdGUgRmlsZXMuUmVhZFdyaXRlLkFsbCBwcm9maWxlIG9wZW5pZCBlbWFpbCIsInNpZ25pbl9zdGF0ZSI6WyJrbXNpIl0sInN1YiI6Ill1UmFBUEtFWUp5T3h5eUVjbWRSMk1kbmFMTzk0OTVBM2RDZ0VaVDkzOUEiLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1bmlxdWVfbmFtZSI6ImFsdW5vMTkwODlAaXB0LnB0IiwidXBuIjoiYWx1bm8xOTA4OUBpcHQucHQiLCJ1dGkiOiI3bFcyamI3UGxFMkRHWDN2V1JJbUFBIiwidmVyIjoiMS4wIiwieG1zX3N0Ijp7InN1YiI6ImV3aGxrN1JiaFY3blhtNzR1NVNBSVBONzBiS2lucExYTU5CUHdUQVNRdFkifSwieG1zX3RjZHQiOjE0MjkyNjcyMDh9.ZAlD4P-AcFAnyY6yeJ6RGTHOQPPH-0CoQJEpoXscphCFFOs5bTm8C5hhC_EKW7IFkyy5Tsx_STQ3aR4_ghZkrFi4OGdl-1NVVeP3p6r6vr2Hozgvw0uzYhwYC_Q9DWCabmKTFakH6M8TXfuGnyGHMqjb-aeRidYZQDxLqHZzlbx8qqn3nvOI8M1PwO2786VlYBWRd3IVjzbQFKb6lb29iUvmiGvWHQHXgrWacd1rVuu5tPEcc8vQWRhQ7dc8aGmvYbEBiB5Hcr2NfDHqAPf-0BVb9CCWSnEcQYj0hqCI25A4dFBxOgYJWg8lOi6ln8fzK1PMWxAjnVSMBhlmPsncRg";

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
            catch (Exception ex)
            {
                throw;
            }

            var response = await request.GetResponseAsync();

            return "kappa";
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

    }
}
