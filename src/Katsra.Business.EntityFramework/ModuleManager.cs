using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kastra.Core;
using Kastra.Core.Business;
using Kastra.DAL.EntityFramework;

namespace Kastra.Business
{
    public class ModuleManager : IModuleManager
    {
        private readonly KastraContext _dbContext = null;
        private readonly IViewManager _viewManager = null;
        private readonly IServiceProvider _serviceProvider = null;

        public ModuleManager(KastraContext dbContext, IServiceProvider serviceProvider, IViewManager viewManager)
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
    }
}