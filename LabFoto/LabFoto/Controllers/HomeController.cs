using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LabFoto.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace LabFoto.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            //try
            //{ 
            //    var request = new HttpRequestMessage(HttpMethod.Get,
            //    "https://graph.microsoft.com/v1.0/me/drives/b!0812_G3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29-ASFmlYSqg3p9xBheG7/root/search(q='levantamento')");
            //    request.Headers.Add("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJub25jZSI6IkFRQUJBQUFBQUFEQ29NcGpKWHJ4VHE5Vkc5dGUtN0ZYMTlod3B6WmJNNXZOSzk2bVFDU3JwZmNZenhDSzltRFF1cUhaeGVQdlQzTlQ0dDNhUTZfby1tckFUU1ZidGlSSTh6bkhKMXdhUGZiWk5fOG5JVHRldWlBQSIsImFsZyI6IlJTMjU2IiwieDV0IjoiSEJ4bDltQWU2Z3hhdkNrY29PVTJUSHNETmEwIiwia2lkIjoiSEJ4bDltQWU2Z3hhdkNrY29PVTJUSHNETmEwIn0.eyJhdWQiOiJodHRwczovL2dyYXBoLm1pY3Jvc29mdC5jb20iLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8yMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAvIiwiaWF0IjoxNTU4ODE4MjQwLCJuYmYiOjE1NTg4MTgyNDAsImV4cCI6MTU1ODgyMjE0MCwiYWNjdCI6MCwiYWNyIjoiMSIsImFpbyI6IjQyWmdZRGlhOWxaaDE4eWFabmFUNHFid2cvZi8zRXRpanpIV0N0Qm0yZmFVa1NuVit6RUEiLCJhbXIiOlsicHdkIl0sImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJhcHBpZCI6IjJkYTc0ODRjLTllZWEtNDlhMy1iMzM3LWY1OWE5N2Y3OWU0NyIsImFwcGlkYWNyIjoiMSIsImZhbWlseV9uYW1lIjoiQWxleGFuZHJlIiwiZ2l2ZW5fbmFtZSI6IlRlbG1vIiwiaXBhZGRyIjoiMTg4LjI1MS4yMjYuMTM4IiwibmFtZSI6IlRlbG1vIE9saXZlaXJhIEFsZXhhbmRyZSIsIm9pZCI6ImM3MmRlNTZiLTVkZmUtNDMyOC05MzBkLTA0YTNkNzJmMjUzMyIsIm9ucHJlbV9zaWQiOiJTLTEtNS0yMS0zMTAwNjczNzQ2LTQyNzA1ODM2MjYtMjQyNTI0MjA0OS0xNzgyOSIsInBsYXRmIjoiMyIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0Iiwic2NwIjoiRmlsZXMuUmVhZCBGaWxlcy5SZWFkLkFsbCBGaWxlcy5SZWFkV3JpdGUgRmlsZXMuUmVhZFdyaXRlLkFsbCBwcm9maWxlIG9wZW5pZCBlbWFpbCIsInNpZ25pbl9zdGF0ZSI6WyJrbXNpIl0sInN1YiI6Ill1UmFBUEtFWUp5T3h5eUVjbWRSMk1kbmFMTzk0OTVBM2RDZ0VaVDkzOUEiLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1bmlxdWVfbmFtZSI6ImFsdW5vMTkwODlAaXB0LnB0IiwidXBuIjoiYWx1bm8xOTA4OUBpcHQucHQiLCJ1dGkiOiJhYVVMZ0JsMjNrQ1NYM1czQkV3UEFBIiwidmVyIjoiMS4wIiwieG1zX3N0Ijp7InN1YiI6ImV3aGxrN1JiaFY3blhtNzR1NVNBSVBONzBiS2lucExYTU5CUHdUQVNRdFkifSwieG1zX3RjZHQiOjE0MjkyNjcyMDh9.cxcDSbfDQZexzBSLumJNVOibDH_JJ6wnS3Woo0fFJUgcyc64ie07jLBbfFARY4jbdUEWzWfp7lXwsgAc__3meN47nlCgINlJfvZyxk4IVdioEXM2agj1QDm8hFotwDwQw4VLqDwdzMLGr2p3XI4M5VyBFP-TKx43vtzW2OZ11sehCsau-I221VlbV52Hph0-CvdFCRwlZ4Mzgc5kF1Z90EWYL8II6bPXjKL2IThFSE2cy57h1euZrIXR_CWUK_QTeV4P3NxotggosNGUPt_MoTNL6obQFf-Jw_5pIkJvtVih_m1KjfF7FrH1FyJqLtCYIZA-tXSCGsQThY9zprvJlg");

            //    var client = _clientFactory.CreateClient();

            //    // Resposta do GET efetuado
            //    var response = await client.SendAsync(request);

            //    // Caso retorne OK 200
            //    if (response.IsSuccessStatusCode)
            //    {
            //        // Conteudo da resposta em string
            //        string sContent = await response.Content.ReadAsStringAsync();

            //        // Converter a resposta para um objeto json
            //        JObject contentJson = JObject.Parse(sContent);

            //        // Individualizar o array 'value' da resposta da api da onedrive
            //        JArray values = (JArray)contentJson["value"];

            //        // Transformar o array json num array de objetos json
            //        JObject[] arrayObjetos = values.Select(v => (JObject)v).ToArray();

            //        // Percorrer o array de objetos
            //        foreach (JObject objeto in arrayObjetos)
            //        {
            //            // Recolher o atributo "name" em cada objeto do array de objetos
            //            string name = (string)objeto["name"];
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 4_294_967_295)] // 4 GB - 1
        public IActionResult Upload(List<IFormFile> files, [FromServices] IHostingEnvironment env)
        {
            double totalLength = 0;
            foreach (var file in files)
            {
                totalLength += file.Length;
                string fileName = $"ImagensUpload\\{file.FileName}";
                using (FileStream fs = System.IO.File.Create(fileName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            double totalRounded = ((totalLength / 1024) / 1024);
            if (totalRounded >= 1024)
            {
                ViewData["message"] = $"{Math.Round(totalRounded/1024, 2)} Gigabytes carregados com sucesso!";
            }
            else
            {
                ViewData["message"] = $"{Math.Round(totalRounded, 0)} Megabytes carregados com sucesso!";
            }
            return View("Privacy");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
