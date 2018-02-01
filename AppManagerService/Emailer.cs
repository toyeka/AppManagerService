using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

using AppManagerService.Extensions;

using log4net;

namespace AppManagerService
{
    public class Emailer
    {
        private readonly ILog logger;
        private readonly string smtpHost;
        private readonly string smtpLogin;
        private readonly string smtpPass;
        private readonly string fromAddress;
        private readonly string toAddress;

        public Emailer(ILog logger)
        {
            this.logger = logger;
            try
            {
                fromAddress = ConfigurationManager.AppSettings["EmailFrom"].ThrowIfNull("EmailFrom");
                toAddress = ConfigurationManager.AppSettings["EmailTo"].ThrowIfNull("EmailTo");
                smtpHost = ConfigurationManager.AppSettings["SmtpHost"].ThrowIfNull("SmtpHost");
                smtpLogin = ConfigurationManager.AppSettings["SmtpLogin"].ThrowIfNull("SmtpLogin");
                smtpPass = ConfigurationManager.AppSettings["SmtpPass"].ThrowIfNull("SmtpPass");
            }
            catch (Exception ex)
            {
                logger.Error("Config Init error", ex);
            }
        }

        public void SendMail(string caption, string message)
        {
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress(fromAddress);
                mail.To.Add(new MailAddress(toAddress));
                mail.Subject = caption;
                mail.Body = message;

                var client = new SmtpClient(smtpHost);
                client.Credentials = new NetworkCredential(smtpLogin, smtpPass);
                client.Send(mail);

                mail.Dispose();
            }
            catch (Exception ex)
            {
                logger.Error("Send mail error.", ex);
            }
        }
    }
}