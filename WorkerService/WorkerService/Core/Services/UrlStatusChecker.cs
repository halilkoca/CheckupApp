using Data;
using Data.Models;
using System;
using System.Threading.Tasks;
using WorkerService.Core.Extension;
using WorkerService.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace WorkerService.Core.Services
{
    public class UrlStatusChecker : IUrlStatusChecker
    {
        private readonly IHttpService _httpService;
        private readonly IServiceLocator _serviceScopeFactoryLocator;
        private readonly IEmailService _emailService;

        public UrlStatusChecker(
            IHttpService httpService
            , IServiceLocator serviceScopeFactoryLocator
            , IEmailService emailService
            )
        {
            _httpService = httpService;
            _serviceScopeFactoryLocator = serviceScopeFactoryLocator;
            _emailService = emailService;
        }

        public async Task CheckUrlAsync(CheckApp checkApp, string requestId)
        {
            ClauseExtensions.Null(checkApp, nameof(checkApp));
            await Task.Delay(checkApp.Interval * 1000); // milisaniyeye çeviri

            try
            {
                var statusCode = await _httpService.GetUrlResponseStatusCodeAsync(checkApp.AppUrl);

                if (statusCode >= 300 && statusCode < 200)
                    await SendMail(checkApp, "", statusCode);
            }
            catch (Exception ex)
            {
                await SendMail(checkApp, ex.Message, 404);
            }
        }



        public async Task SendMail(CheckApp checkApp, string errorMessage, int statusCode)
        {
            if (!errorMessage.Equals(""))
            {
                // her execute için EF scope gerekiyor
                using var scope = _serviceScopeFactoryLocator.CreateScope();
                var repository = scope.ServiceProvider.GetService<IRepository>();

                // Http status - response time kaydet
                repository.Add(new AppHistory
                {
                    StatusCode = statusCode,
                    CheckAppId = checkApp.Id,
                    RequestDateUtc = DateTime.Now,
                    ErrorMessage = errorMessage
                });
            }

            // send email
            await _emailService.SendEmail(checkApp, statusCode, errorMessage);

            
        }
    }
}
