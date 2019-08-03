using LabFoto.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LabFoto.APIs
{
    public class EmailAPI : IEmailAPI
    {
        private readonly AppSettings _appSettings;
        private readonly string _email;
        private readonly string _emailPassword;
        private readonly string _emailNome;
        private readonly string _emailSmtp;

        public EmailAPI(IOptions<AppSettings> settings)
        {
            _appSettings = settings.Value;
            _email = _appSettings.Email;
            _emailPassword = _appSettings.EmailPassword;
            _emailNome = _appSettings.EmailNomeApresentado;
            _emailSmtp = _appSettings.EmailSmtp;
        }

        /// <summary>
        /// Envia um email a um destinatário à escolha.
        /// </summary>
        /// <param name="destinations">Destinatários.</param>
        /// <param name="subject">Assunto do email.</param>
        /// <param name="body">Corpo do email. HTML aceite.</param>
        public void Send(string[] destinations, string subject, string body)
        {
            try
            {
                using (var client = new SmtpClient(_emailSmtp))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(_email, _emailPassword);
                    client.EnableSsl = true;

                    foreach (string destination in destinations)
                    {
                        using (var message = new MailMessage())
                        {
                            message.From = new MailAddress(_email, _emailNome);
                            message.To.Add(new MailAddress(destination));
                            //message.CC.Add(new MailAddress("cc@email.com", "CC Name"));
                            //message.Bcc.Add(new MailAddress("bcc@email.com", "BCC Name"));
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

        /// <summary>
        /// Envia um email a um destinatário à escolha.
        /// </summary>
        /// <param name="destination">Destinatário.</param>
        /// <param name="subject">Assunto do email.</param>
        /// <param name="body">Corpo do email. HTML aceite.</param>
        public void Send(string destination, string subject, string body)
        {
            try
            {
                using (var client = new SmtpClient(_emailSmtp))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(_email, _emailPassword);
                    client.EnableSsl = true;

                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(_email, _emailNome);
                        message.To.Add(new MailAddress(destination));
                        //message.CC.Add(new MailAddress("cc@email.com", "CC Name"));
                        //message.Bcc.Add(new MailAddress("bcc@email.com", "BCC Name"));
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true;

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
            string[] admins = _appSettings.AdminEmails;
            string fullTitle = "Notificação Erro - " + title;
            string body = 
                "<h2 style='color:red;'>" + title + "</h2>" +
                "</br>" +
                "<p>Classe: " + classFile + "</p>" +
                "<p>Método: " + method + "</p>" +
                "</br>" +
                "<p style='font-weight: bold;'>Detalhes: " + details + "</p>";

            Send(admins, fullTitle, body);
        }
    }

    public interface IEmailAPI
    {
        void Send(string[] destinations, string subject, string body);
        void Send(string destinations, string subject, string body);
        void NotifyError(string title, string classFile, string method, string details);
    }
}
