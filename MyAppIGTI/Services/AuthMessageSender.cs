using System.Net.Mail;
using System.Net;

namespace MyAppIGTI.Services
{    
    public class AuthMessageSender
    {
        private const string sToEmail = "nathanrgonzales@hotmail.com";
        private const string sUserName = "gonzales.nathan@gmail.com";
        private const string sPassword = "36341387";
        private const string sPrimaryDomain = "smtp.gmail.com";
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
            try
            {
                string toEmail = string.IsNullOrEmpty(email) ? sToEmail : email;

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(sUserName, "Test - IGTI PA")
                };

                mail.To.Add(new MailAddress(toEmail));                

                mail.Subject = "Result IGTI PA " + subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                if(!string.IsNullOrEmpty(attachFile))
                    mail.Attachments.Add(new Attachment(attachFile));
                
                using (SmtpClient smtp = new SmtpClient(sPrimaryDomain, sPrimaryPort))
                {
                    smtp.Credentials = new NetworkCredential(sUserName, sPassword);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }                                                                                                            
        }
    }
}