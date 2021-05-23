using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kastra.Core.Services.Contracts;
using Kastra.Core.Configuration;
using Kastra.Core.Modules;
using Kastra.DAL.EntityFramework;

namespace Kastra.Business
{
    public class ModuleManager : IModuleManager
    {
        private readonly KastraDbContext _dbContext;
        private readonly IViewManager _viewManager;
        private readonly IServiceProvider _serviceProvider;

        public ModuleManager(KastraDbContext dbContext, IServiceProvider serviceProvider, IViewManager viewManager)
        {
            _dbContext = dbContext;
            _viewManager = viewManager;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Installs the Kastra modules which contain a class with IModuleRegister interface.
        /// </summary>
        public void InstallModules()
        {
            IModuleRegister dependancyRegister = null;
            Type serviceType = typeof(IModuleRegister);

            IEnumerable<Type> modulesToRegister = KastraAssembliesContext.Instance.GetModuleAssemblies().SelectMany(assembly => assembly.GetTypes())
                                                 .Where(type => serviceType.IsAssignableFrom(type) &&  !type.GetTypeInfo().IsAbstract);

            foreach (Type implementationType in modulesToRegister)
            {
                dependancyRegister = Activator.CreateInstance(implementationType) as IModuleRegister;
                dependancyRegister.Install(_serviceProvider, _viewManager);
            }
        }

        /// <summary>
        /// Uninstalls the Kastra modules which contain a class with IModuleRegister interface.
        /// </summary>
        public void UninstallModules()
        {
            IModuleRegister dependancyRegister = null;
            Type serviceType = typeof(IModuleRegister);

            IEnumerable<Type> modulesToRegister = KastraAssembliesContext.Instance.GetModuleAssemblies().SelectMany(assembly => assembly.GetTypes())
                                                 .Where(type => serviceType.IsAssignableFrom(type) &&  !type.GetTypeInfo().IsAbstract);

            foreach (Type implementationType in modulesToRegister)
            {
                dependancyRegister = Activator.CreateInstance(implementationType) as IModuleRegister;
                dependancyRegister.Uninstall();
            }
        }

        /// <summary>
        /// Installs the module.
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        public void InstallModule(Assembly assembly)
        {
            IModuleRegister dependancyRegister = null;
            Type serviceType = typeof(IModuleRegister);

            IEnumerable<Type> modulesToRegister = assembly.GetTypes()
                .Where(type => serviceType.IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract);

            foreach (Type implementationType in modulesToRegister)
            {
                dependancyRegister = Activator.CreateInstance(implementationType) as IModuleRegister;
                dependancyRegister.Install(_serviceProvider, _viewManager);
            }
        }

        /// <summary>
        /// Uninstalls the module.
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        public void UninstallModule(Assembly assembly)
        {
            IModuleRegister dependancyRegister = null;
            Type serviceType = typeof(IModuleRegister);

            IEnumerable<Type> modulesToRegister = assembly.GetTypes()
                .Where(type => serviceType.IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract);

            foreach (Type implementationType in modulesToRegister)
            {
                dependancyRegister = Activator.CreateInstance(implementationType) as IModuleRegister;
                dependancyRegister.Uninstall();
            }
        }
    }
}