using Kastra.Core.Services.Contracts;
using Kastra.Core.Modules;
using Kastra.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Kastra.Business
{
    public class DependancyRegister : IDependencyRegister
    {
        public void SetDependencyInjections(IServiceCollection services, IConfiguration configuration)
        {
            // Add dependency injections
            services.AddScoped<IApplicationManager, ApplicationManager>();
            services.AddScoped<IModuleManager, ModuleManager>();
            services.AddScoped<IParameterManager, ParameterManager>();
            services.AddScoped<IViewManager, ViewManager>();
            services.AddScoped<ISecurityManager, SecurityManager>();
            services.AddScoped<IStatisticsManager, StatisticsManager>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IEmailManager, EmailManager>();
            services.AddScoped<IFileManager, FileManager>();

            // HCaptcha
            services.AddScoped<ICaptchaService, CaptchaService>();

            services.AddHttpClient("hCaptcha", c =>
            {
                c.BaseAddress = new Uri("https://hcaptcha.com/");
            });
        }
    }
}