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
using LabFoto.Onedrive;

namespace LabFoto.Controllers
{
    [Authorize]
    public class ContasOnedriveController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly OnedriveAPI _onedrive;

        public ContasOnedriveController(ApplicationDbContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _onedrive = new OnedriveAPI(context, clientFactory);
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
            return Redirect(_onedrive.GetPermissionsUrl());
        }

        // GET: ContaOnedrives/Create
        public async Task<IActionResult> Create(string code, string state)
        {
            try
            {
                // Faz o pedido HTTP.GET para pedir o token utilizando o codigo das permissões
                JObject tokenResponse = await _onedrive.GetInitialToken(code);
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
                JObject driveInfo = await _onedrive.GetDriveInfo(access_token);
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

        private bool ContaOnedriveExists(int id)
        {
            return _context.ContasOnedrive.Any(e => e.ID == id);
        }
    }
}
