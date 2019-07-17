using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LabFoto.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.IO;
using LabFoto.Data;
using LabFoto.Onedrive;
using Microsoft.AspNetCore.Http;
using LabFoto.Models.Tables;

namespace LabFoto.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly OnedriveAPI _onedrive;

        public HomeController(ApplicationDbContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _onedrive = new OnedriveAPI(context, clientFactory);
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> upload(List<IFormFile> files)
        {
            // Irá conter todos os caminhos para os ficheiros temporários
            List<string> filePaths = new List<string>();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    #region Recolher informações do ficheiro e guardar-lo no servidor
                    string fileName = formFile.FileName;

                    // Criar um caminho temporário para o ficheiro
                    var filePath = Path.GetTempFileName();
                    filePaths.Append(filePath);

                    // Guardar o ficheiro temporário no servidor.
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    #endregion

                    #region Upload do ficheiro
                    // Dar upload do ficheiro para a onedrive
                    // Será selecionada uma conta com espaço de forma automática
                    UploadedPhotoModel response = await _onedrive.UploadFileAsync(filePath, fileName);
                    #endregion

                    // Caso o upload tenha tido sucesso
                    if (response.Success)
                    {
                        #region Adicionar foto à Bd
                        try
                        {
                            Fotografia foto = new Fotografia
                            {
                                Nome = response.ItemName,
                                ItemId = response.ItemId,
                                ContaOnedriveFK = response.Conta.ID,
                                GaleriaFK = 1,
                                Formato = GetFileFormat(response.ItemName)
                            };

                            await _context.AddAsync(foto);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                        #endregion
                    }
                    else
                    {
                        // Tratar da do insucesso
                    }
                }
            }

            #region Apagar os ficheiros temporários
            _onedrive.DeleteFiles(filePaths); // Apagar todos os ficheiros temporário do servidor 
            #endregion

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region AuxMethods

        /// <summary>
        /// Retorna o formato a partir de um nome.
        /// </summary>
        /// <param name="name">Nome do ficheiro. Tem que conter o formato</param>
        /// <returns></returns>
        private string GetFileFormat(string name)
        {
            try
            {
                return name.Split(".")[2];
            }
            catch (Exception)
            {
                return "";
            }
        }

        #endregion
    }
}
