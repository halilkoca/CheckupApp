using System.Threading.Tasks;
using WorkerService.Core.Interfaces;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.DependencyInjection;
using Data;
using Data.Models;
using System;

namespace WorkerService.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly IServiceLocator _serviceScopeFactoryLocator;

        public EmailService(
            IServiceLocator serviceScopeFactoryLocator
            )
        {
            _serviceScopeFactoryLocator = serviceScopeFactoryLocator;
        }

        public async Task<string> SendEmail(CheckApp app, int statusCode, string errorMessage = null)
        {
            try
            {
                string bodyMessage = errorMessage != null && errorMessage != "" ? errorMessage : string.Format("The app is currently down: Application Name:{0} Code:{1}", app.AppName, statusCode);
                // Credentials
                var credentials = new NetworkCredential("ihalilkoca@gmail.com", "your-password");
                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress("ihalilkoca@gmail.com"),
                    Subject = "Application Down",
                    Body = bodyMessage
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
                string errorMessageIn = ex.Message;
                if (errorMessageIn.Length > 1023)
                    errorMessageIn = errorMessageIn.Substring(0, 1023);
                // her execute için EF scope gerekiyor
                using var scope = _serviceScopeFactoryLocator.CreateScope();
                var repository = scope.ServiceProvider.GetService<IRepository>();

                // Http status - response time kaydet
                repository.Add(new AppHistory
                {
                    StatusCode = statusCode,
                    CheckAppId = app.Id,
                    RequestDateUtc = DateTime.Now,
                    ErrorMessage = errorMessageIn
                });

                return ex.Message;
            }
        }
    }
}
