using LabFoto.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LabFoto.Onedrive
{
    public class Email
    {
        private readonly AppSettings _appSettings;
        private readonly string _email;
        private readonly string _emailPassword;
        private readonly string _nomeEmail;

        public Email(IOptions<AppSettings> settings)
        {
            _appSettings = settings.Value;
            _email = _appSettings.Email;
            _emailPassword = _appSettings.EmailPassword;
            _nomeEmail = _appSettings.EmailNomeApresentado;
        }

        /// <summary>
        /// Envia um email a um destinatário à escolha.
        /// </summary>
        /// <param name="to">Destinatário.</param>
        /// <param name="nomeCliente">Nome do cliente a ser apresentado.</param>
        /// <param name="subject">Assunto do email.</param>
        /// <param name="body">Corpo do email. HTML aceite.</param>
        public void Send(string to, string nomeCliente, string subject, string body)
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.To.Add(new MailAddress(to, nomeCliente));
                    message.From = new MailAddress(_email, _nomeEmail);
                    //message.CC.Add(new MailAddress("cc@email.com", "CC Name"));
                    //message.Bcc.Add(new MailAddress("bcc@email.com", "BCC Name"));
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    using (var client = new SmtpClient("smtp.gmail.com"))
                    {
                        client.Port = 587;
                        client.Credentials = new NetworkCredential(_email, _emailPassword);
                        client.EnableSsl = true;
                        client.Send(message);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Notifica os administradores de um erro na aplicação.
        /// </summary>
        /// <param name="subject">Assunto do email.</param>
        /// <param name="title">Titulo do erro.</param>
        /// <param name="location">Localização do erro no código.</param>
        /// <param name="details">Detalhes do erro.</param>
        public void NotifyError(string title, string classFile, string method, string details)
        {
            SendToAdmins("Notificação Erro - " + title,
                "<h2 style='color:red;'>" + title + "</h2>" +
                "</br>" +
                "<p>Classe:" + classFile + "</p>" +
                "<p>Método:" + method + "</p>" +
                "</br>" +
                "<p style='font-weight: bold;'>Detalhes: " + details + "</p>");
        }

        /// <summary>
        /// Envia um email aos administradores da aplicação.
        /// </summary>
        /// <param name="subject">Assunto do email.</param>
        /// <param name="body">Corpo do email. HTML aceite.</param>
        public void SendToAdmins(string subject, string body)
        {
            string[] adminsEmails = _appSettings.AdminEmails;

            try
            {
                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(_email, _emailPassword);
                    client.EnableSsl = true;

                    foreach (string admin in adminsEmails)
                    {
                        using (var message = new MailMessage())
                        {
                            message.To.Add(new MailAddress(admin));
                            message.From = new MailAddress(_email, _nomeEmail);
                            message.Subject = subject;
                            message.Body = body;
                            message.IsBodyHtml = true;

                            client.Send(message);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
