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
using LabFoto.APIs;
using Microsoft.Extensions.Options;
using LabFoto.Models;
using LabFoto.Models.ViewModels;
using Microsoft.Extensions.Logging;

namespace LabFoto.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ContasOnedriveController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOnedriveAPI _onedrive;
        private readonly ILogger<ContasOnedriveController> _logger;

        public ContasOnedriveController(ApplicationDbContext context, IOnedriveAPI onedrive, ILogger<ContasOnedriveController> logger)
        {
            _context = context;
            _onedrive = onedrive;
            _logger = logger;
        }

        #region Index
        // GET: ContaOnedrives
        public async Task<IActionResult> Index()
        {
            // Fornecer feedback ao cliente caso este exista.
            // Este feedback é fornecido na view a partir de uma notificação 'Noty'
            if (TempData["Feedback"] != null)
            {
                ViewData["Feedback"] = TempData["Feedback"];
                ViewData["Type"] = TempData["Type"];
            }

            return View(await _context.ContasOnedrive.ToListAsync());
        }

        /// <summary>
        /// Método utilizado para atualizar a lista 
        /// de contas Onedrive a mostrar consoante o que vem por parametro
        /// </summary>
        /// <param name="Username">Username da conta</param>
        /// <returns>retorna uma PartialView com a lista de contas certa</returns>
        [HttpPost]
        public async Task<IActionResult> IndexFilter(string Username)
        {
            var contas = _context.ContasOnedrive.Select(c => c);

            if (!String.IsNullOrEmpty(Username))
            {
                contas = contas.Where(c => c.Username.Contains(Username));
            }

            return PartialView("PartialViews/_IndexCards", await contas.ToListAsync());
        }

        #endregion

        #region Create
        /// <summary>
        /// Método que redireciona para uma página da Microsoft que pede as credênciais e permissões necessárias.
        /// </summary>
        /// <returns>Retorna para a página Create das contas Onedrive com o code necessário.</returns>
        public ActionResult PermissionUrl()
        {
            return Redirect(_onedrive.GetPermissionsUrl());
        }

        // GET: ContaOnedrives/Create
        public IActionResult Create(string code)
        {
            return View(new ContaOnedriveCreateViewModel
            {
                ContaOnedrive = new ContaOnedrive(),
                Code = code
            });
        }

        // POST: ContaOnedrives/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Username,Password")] ContaOnedrive ContaOnedrive, string Code)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ContaOnedrive);
                await _context.SaveChangesAsync();
                return RedirectToAction("InitAccount", new { Code, id = ContaOnedrive.ID });
            }
            return View(new ContaOnedriveCreateViewModel
            {
                ContaOnedrive = ContaOnedrive,
                Code = Code
            });
        }
        #endregion

        #region InitAccount
        /// <summary>
        /// Método que recebe o code e o id da conta acabada de adicionar 
        /// à base de dados e atualiza o token, recolhe informações do espaço na drive e
        /// atualiza a conta na base de dados.
        /// </summary>
        /// <param name="Code">Código proveniente da microsoft que permite ter permissões sobre a conta Onedrive</param>
        /// <param name="id">Id da conta Onedrive na base de dados</param>
        /// <returns>Retorna ao Index com a mensagem que a conta foi adicionada com sucesso, 
        /// ou que ocorreu um erro ao criar a conta.</returns>
        public async Task<IActionResult> InitAccount(string Code, int id)
        {
            // Define se a operação foi executada com sucesso
            bool success = true;

            ContaOnedrive conta = await _context.ContasOnedrive.FindAsync(id);

            string access_token = "";

            #region Atualizar token
            try
            {
                // Faz o pedido HTTP.GET para pedir o token utilizando o codigo das permissões
                JObject tokenResponse = await _onedrive.GetInitialTokenAsync(Code);

                // Recolher os tokens
                access_token = (string)tokenResponse["access_token"];
                string refresh_token = (string)tokenResponse["refresh_token"];

                // Atualizar informações da conta
                conta.AccessToken = access_token;
                conta.RefreshToken = refresh_token;
                conta.TokenDate = DateTime.Now;
            }
            catch (Exception e)
            {
                success = false;
                _logger.LogError($"Erro ao obter token da conta. Message: {e.Message}");
                // Feeback ao utilizador - Vai ser redirecionado para o Index
                TempData["Feedback"] = "Ocorreu obter o token da conta.";
                TempData["Type"] = "error";
            }
            #endregion

            #region Recolher ID e informação do espaço na drive 
            // Caso exista acesso token e a operação acima tenha sido executada com sucesso
            if (!String.IsNullOrEmpty(access_token) && success)
            {
                try
                {
                    // Faz o pedido HTTP.GET para pedir as informações da Onedrive
                    // Para que estas possam ser associadas ao objeto 'conta'
                    JObject driveInfo = await _onedrive.GetDriveInfoAsync(access_token);

                    // Transformar o array num array de objetos
                    JObject[] values = driveInfo["value"].Select(s => (JObject)s).ToArray();

                    string driveId = (string)values[0]["id"];

                    JObject quota = (JObject)values[0]["quota"];
                    string quota_Total = (string)quota["total"];
                    string quota_Used = (string)quota["used"];
                    string quota_Remaining = (string)quota["remaining"];

                    // Atualizar a conta com as informações recolhidas da API da Onedrive
                    conta.DriveId = driveId;
                    conta.Quota_Remaining = quota_Remaining;
                    conta.Quota_Total = quota_Total;
                    conta.Quota_Used = quota_Used;

                }
                catch (Exception e)
                {
                    success = false;
                    _logger.LogError($"Erro ao obter informações da conta. Message: {e.Message}");
                    // Feeback ao utilizador - Vai ser redirecionado para o Index
                    TempData["Feedback"] = "Ocorreu um erro ao obter informações da conta.";
                    TempData["Type"] = "error";
                }
            }
            #endregion

            #region Atualizar a conta
            // Caso as operações acima tenham sido executadas com sucesso
            if (success)
            {
                try
                {
                    // Atualizar conta
                    _context.ContasOnedrive.Update(conta);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    success = false;
                    _logger.LogError($"Erro ao criar a conta. Message: {e.Message}");
                    // Feeback ao utilizador - Vai ser redirecionado para o Index
                    TempData["Feedback"] = "Ocorreu um erro ao criar a conta.";
                    TempData["Type"] = "error";
                }
            }
            #endregion

            if (success)
            {
                // Feeback ao utilizador - Vai ser redirecionado para o Index
                TempData["Feedback"] = "Conta Onedrive adiconada com sucesso.";
                TempData["Type"] = "success";
            }
            else
            {
                // Caso tenha acontecido algum erro, certificar que a conta não é criada
                _context.ContasOnedrive.Remove(conta);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        // GET: ContaOnedrives/Edit/5
        public async Task<IActionResult> Edit(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contaOnedrive = await _context.ContasOnedrive.FindAsync(id);
            if (contaOnedrive == null)
            {
                return NotFound();
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(contaOnedrive);
        }

        /// <summary>
        /// Metodo que apenas permite que sejam alterados os atributos Username e Password das contas Onedrive.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contaOnedrive"></param>
        /// <returns>Retorna para a página de index com a mensagem que
        /// a conta foi editada com sucesso ou que não foi possível editar a conta.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Username,Password")] ContaOnedrive contaOnedrive)
        {
            if (id != contaOnedrive.ID)
            {
                return NotFound();
            }

            ContaOnedrive conta = null;

            try
            {
                conta = await _context.ContasOnedrive.FindAsync(id);
            }
            catch (Exception)
            {
                // Feeback ao utilizador - Vai ser redirecionado para o Index
                TempData["Feedback"] = "Não foi possível editar a conta.";
                TempData["Type"] = "error";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Garantir que apenas estes dados são alterados
                    conta.Username = contaOnedrive.Username;
                    conta.Password = contaOnedrive.Password;
                    _context.Update(conta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContaOnedriveExists(conta.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Feeback ao utilizador - Vai ser redirecionado para o Index
                TempData["Feedback"] = "Conta editada com sucesso.";
                TempData["Type"] = "success";
                return RedirectToAction("Index");
            }
            return View(contaOnedrive);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Método que apenas deixa eliminar a conta caso não existam fotografias associadas à conta em questão.
        /// </summary>
        /// <param name="id">Id da conta Onedrive a ser eliminada</param>
        /// <returns>Retorna ao Index com a mensagem Conta Onedrive eliminada com sucesso,
        /// ou conta tem fotografias associadas ou erro ao eliminar a conta</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            ContaOnedrive contaOnedrive = null;
            try
            {
                contaOnedrive = await _context.ContasOnedrive.Include(c => c.Fotografias).Where(c => c.ID == id).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Erro ao encontrar a conta Onedrive. Erro:" + e.Message);
                return Json(new { success = false });
            }

            // Apenas deixar apagar a conta caso não existam fotografias na conta
            if (contaOnedrive != null && contaOnedrive.Fotografias.Count() == 0)
            {
                try
                {
                    _context.Remove(contaOnedrive);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Conta Onedrive removida.");
                }
                catch (Exception e)
                {
                    _logger.LogError($"Erro ao eliminar conta Onedrive. Erro: {e.Message}");
                    return Json(new { success = false });
                }

                // Feeback ao utilizador - Vai ser redirecionado para o Index
                TempData["Feedback"] = "Conta Onedrive eliminada com sucesso.";
                TempData["Type"] = "success";

                return Json(new { success = true });
            }

            return Json(new { success = false , hasPhotos = true} );

        }
        #endregion

        #region AuxMethods
        private bool ContaOnedriveExists(int id)
        {
            return _context.ContasOnedrive.Any(e => e.ID == id);
        }
        #endregion
    }
}
