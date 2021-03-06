﻿using LabFoto.Data;
using LabFoto.Models.Tables;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LabFoto.APIs
{
    #region Interface
    public interface ILoggerAPI
    {
        Task<bool> LogError(string descricao, string classe, string metodo, string erro);
    }
    #endregion
    public class LoggerAPI : ILoggerAPI
    {
        private readonly ApplicationDbContext _context;
        private readonly ClaimsPrincipal _user;

        public LoggerAPI(ApplicationDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _user = accessor.HttpContext.User;
        }

        /// <summary>
        /// Faz o log de um erro que ocorra na aplicação
        /// </summary>
        /// <param name="descricao">Descrição do erro.</param>
        /// <param name="classe">Classe onde o erro ocorreu.</param>
        /// <param name="metodo">Metodo onde o erro ocorreu.</param>
        /// <param name="erro">Detalhes do erro.</param>
        /// <returns>Caso o log tenha ocorrido com sucesso</returns>
        public async Task<bool> LogError(string descricao, string classe, string metodo, string erro)
        {
            var log = new Log {
                Utilizador = _user.Identity.Name,
                Descricao = descricao,
                Classe = classe,
                Metodo = metodo,
                Erro = erro,
                Timestamp = DateTime.Now
            };

            try
            {
                _context.Add(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }


}
