using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kastra.Core.Services.Contracts;
using Kastra.Core.Configuration;
using Kastra.Core.Modules;
using System.Threading.Tasks;

namespace Kastra.Business
{
    public class ModuleManager : IModuleManager
    {
        private readonly IViewManager _viewManager;
        private readonly IServiceProvider _serviceProvider;

        public ModuleManager(IServiceProvider serviceProvider, IViewManager viewManager)
        {
            _viewManager = viewManager;
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc cref="IModuleManager.InstallModulesAsync" />
        public async Task InstallModulesAsync()
        {
            IModuleRegister dependancyRegister = null;
            Type serviceType = typeof(IModuleRegister);

            IEnumerable<Type> modulesToRegister = KastraAssembliesContext.Instance.GetModuleAssemblies().SelectMany(assembly => assembly.GetTypes())
                                                 .Where(type => serviceType.IsAssignableFrom(type) &&  !type.GetTypeInfo().IsAbstract);

            foreach (Type implementationType in modulesToRegister)
            {
                dependancyRegister = Activator.CreateInstance(implementationType) as IModuleRegister;
                
                await dependancyRegister.InstallAsync(_serviceProvider, _viewManager);
            }
        }

        /// <inheritdoc cref="IModuleManager.UninstallModulesAsync" />
        public async Task UninstallModulesAsync()
        {
            IModuleRegister dependancyRegister = null;
            Type serviceType = typeof(IModuleRegister);

            IEnumerable<Type> modulesToRegister = KastraAssembliesContext.Instance.GetModuleAssemblies().SelectMany(assembly => assembly.GetTypes())
                                                 .Where(type => serviceType.IsAssignableFrom(type) &&  !type.GetTypeInfo().IsAbstract);

            foreach (Type implementationType in modulesToRegister)
            {
                dependancyRegister = Activator.CreateInstance(implementationType) as IModuleRegister;
                
                await dependancyRegister.UninstallAsync();
            }
        }

        /// <inheritdoc cref="IModuleManager.InstallModuleAsync(Assembly)" />
        public async Task InstallModuleAsync(Assembly assembly)
        {
            IModuleRegister dependancyRegister = null;
            Type serviceType = typeof(IModuleRegister);

            IEnumerable<Type> modulesToRegister = assembly.GetTypes()
                .Where(type => serviceType.IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract);

            foreach (Type implementationType in modulesToRegister)
            {
                dependancyRegister = Activator.CreateInstance(implementationType) as IModuleRegister;
                
                await dependancyRegister.InstallAsync(_serviceProvider, _viewManager);
            }
        }

        /// <inheritdoc cref="IModuleManager.UninstallModuleAsync(Assembly)" />
        public async Task UninstallModuleAsync(Assembly assembly)
        {
            IModuleRegister dependancyRegister = null;
            Type serviceType = typeof(IModuleRegister);

            IEnumerable<Type> modulesToRegister = assembly.GetTypes()
                .Where(type => serviceType.IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract);

            foreach (Type implementationType in modulesToRegister)
            {
                dependancyRegister = Activator.CreateInstance(implementationType) as IModuleRegister;
                
                await dependancyRegister.UninstallAsync();
            }
        }
    }
}