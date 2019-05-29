using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabFoto.Data;
using LabFoto.Models.Tables;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text;

namespace LabFoto.Controllers
{
    [Authorize]
    public class ContasOnedriveController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient client;

        public ContasOnedriveController(ApplicationDbContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;

            // Este cliente vai ser utilizado para envio e recepção pedidos Http
            client = _clientFactory.CreateClient();
        }

        // GET: ContaOnedrives
        public async Task<IActionResult> Index()
        {
            // Fornecer feedback ao cliente caso este exista.
            // Este feedback é fornecido na view a partir de uma notificação 'Noty'
            if (TempData["Feedback"] != null)
            {
                ViewData["Feedback"] = TempData["Feedback"];
            }

            return View(await _context.ContasOnedrive.ToListAsync());
        }

        // GET: ContaOnedrives/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contaOnedrive = await _context.ContasOnedrive
                .FirstOrDefaultAsync(m => m.ID == id);
            if (contaOnedrive == null)
            {
                return NotFound();
            }

            return View(contaOnedrive);
        }

        public IActionResult AskPermitions()
        {
            string permissionsUrl =
                "https://login.microsoftonline.com/21e90dfc-54f1-4b21-8f3b-7fb9798ed2e0/oauth2/v2.0/authorize?" +
                "client_id=2da7484c-9eea-49a3-b337-f59a97f79e47" +
                "&response_type=code" +
                "&redirect_uri=https://localhost:44354/ContasOnedrive/Create&response_mode=query" +
                "&scope=offline_access%20files.read%20files.read.all%20files.readwrite%20files.readwrite.all" +
                "&state=12345";

            return Redirect(permissionsUrl);
        }

        // GET: ContaOnedrives/Create
        public async Task<IActionResult> Create(string code, string state)
        {
            try
            {
                // Faz o pedido HTTP.GET para pedir o token utilizando o codigo das permissões
                JObject tokenResponse = await GetInitialToken(code);
                if (tokenResponse == null)
                {
                    // Feeback ao utilizador - Vai ser redirecionado para o Index
                    TempData["Feedback"] = "Ocorreu obter o token da conta.";
                    return RedirectToAction(nameof(Index));
                }

                // Recolher os tokens
                string access_token = (string)tokenResponse["access_token"];
                string refresh_token = (string)tokenResponse["refresh_token"];

                // Faz o pedido HTTP.GET para pedir as informações da Onedrive
                // Para que estas possam ser associadas ao objeto 'conta'
                JObject driveInfo = await GetDriveInfo(access_token);
                if(driveInfo == null)
                {
                    // Feeback ao utilizador - Vai ser redirecionado para o Index
                    TempData["Feedback"] = "Ocorreu obter informações da conta.";
                    return RedirectToAction(nameof(Index));
                }

                // Transformar o array num array de objetos
                JObject[] values = driveInfo["value"].Select(s => (JObject)s).ToArray();

                if (values.Length != 0)
                {
                    string driveId = (string)values[0]["id"];

                    JObject quota = (JObject)values[0]["quota"];
                    string quota_Total = (string)quota["total"];
                    string quota_Used = (string)quota["used"];
                    string quota_Remaining = (string)quota["remaining"];

                    // Criar o objeto 'conta' com informação sobre os tokens
                    ContaOnedrive conta = new ContaOnedrive
                    {
                        AccessToken = access_token,
                        RefreshToken = refresh_token,
                        DriveId = driveId,
                        Quota_Remaining = quota_Remaining,
                        Quota_Total = quota_Total,
                        Quota_Used = quota_Used
                    };

                    _context.ContasOnedrive.Add(conta);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Feeback ao utilizador - Vai ser redirecionado para o Index
                    TempData["Feedback"] = "Ocorreu obter informações da conta.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                // Feeback ao utilizador - Vai ser redirecionado para o Index
                TempData["Feedback"] = "Ocorreu um erro ao criar a conta.";
                return RedirectToAction(nameof(Index));
            }

            // Feeback ao utilizador - Vai ser redirecionado para o Index
            TempData["Feedback"] = "Conta Onedrive adiconada com sucesso.";
            return RedirectToAction(nameof(Index));
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

        private bool ContaOnedriveExists(int id)
        {
            return _context.ContasOnedrive.Any(e => e.ID == id);
        }
    }
}
