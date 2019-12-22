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
                {
                    // send email
                    var sendedMail = await _emailService.SendEmail(checkApp.AppUrl, statusCode.ToString());

                    if (!sendedMail.Equals("Başarılı"))
                    {
                        if (sendedMail.Length > 1023)
                            sendedMail = sendedMail.Substring(0, 1023);
                        // her execute için EF scope gerekiyor
                        using var scope = _serviceScopeFactoryLocator.CreateScope();
                        var repository = scope.ServiceProvider.GetService<IRepository>();

                        // Http status - response time kaydet
                        repository.Add(new AppHistory
                        {
                            RequestId = requestId,
                            StatusCode = statusCode,
                            CheckAppId = checkApp.Id,
                            RequestDateUtc = DateTime.Now,
                            ErrorMessage = sendedMail
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
