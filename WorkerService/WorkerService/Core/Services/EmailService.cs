using System.Threading.Tasks;
using WorkerService.Core.Interfaces;
using System.Net;
using System.Net.Mail;

namespace WorkerService.Core.Services
{
    public class EmailService : IEmailService
    {
        public async Task<string> SendEmail(string appName, string statusCode)
        {
            try
            {
                // Credentials
                var credentials = new NetworkCredential("ihalilkoca@gmail.com", "your-password");
                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress("ihalilkoca@gmail.com"),
                    Subject = "Application Down",
                    Body = string.Format("The app is currently down: Application Name:{0} Code:{1}", appName, statusCode)
                };
                mail.IsBodyHtml = true;
                mail.To.Add(new MailAddress("maydinlik@nuevo.com.tr"));
                // Smtp client
                var client = new SmtpClient()
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Credentials = credentials
                };
                client.Send(mail);
                return "Başarılı";
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }
}
