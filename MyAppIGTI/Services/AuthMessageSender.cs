using System.Net.Mail;
using System.Net;

namespace MyAppIGTI.Services
{    
    public class AuthMessageSender
    {
        private const string sToEmail = "nathanrgonzales@hotmail.com";
        private const string sUserMail = "gonzales.nathan@gmail.com";
        private const string sUserName = "AKIAW6JAYQHKF7RTIJQ5"; //"gonzales.nathan@gmail.com";
        private const string sPassword = "BOPpMOTweUdCXmx7i+zNNZZSY3mIfibMAaK0XpXlG0OY";
        private const string sPrimaryDomain = "email-smtp.sa-east-1.amazonaws.com";  //"smtp.gmail.com";
        private const int sPrimaryPort = 587;

        public Task SendEmailAsync(string email, string subject, string message, string attachFile)
        {
            try
            {
                Execute(email, subject, message, attachFile).Wait();
                return Task.FromResult(0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Execute(string email, string subject, string message, string attachFile)
        {

            string toEmail = string.IsNullOrEmpty(email) ? sToEmail : email;

            MailMessage mail = new MailMessage()
            {
                From = new MailAddress(sUserMail, "Test - IGTI PA")
            };

            mail.To.Add(new MailAddress(toEmail));

            mail.Subject = "Result IGTI PA " + subject;
            mail.Body = message;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            if (!string.IsNullOrEmpty(attachFile))
                mail.Attachments.Add(new Attachment(attachFile));

            /*
            using (SmtpClient smtp = new SmtpClient(sPrimaryDomain, sPrimaryPort))
            {
                smtp.Credentials = new NetworkCredential(sUserName, sPassword);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(mail);
            }
            */

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient
            {
                Host = sPrimaryDomain,
                Port = sPrimaryPort,
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(sUserName, sPassword),
                Timeout = 50000
            };

            try
            {
                await smtp.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                smtp.Dispose();

            }
        }
    }
}