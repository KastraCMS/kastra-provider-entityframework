using Kastra.Core;
using Kastra.Core.Business;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        }
    }
}