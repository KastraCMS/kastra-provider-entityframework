using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Kastra.Core.Configuration;
using Kastra.Core.Modules;
using Kastra.Core.Identity;
using Microsoft.AspNetCore.Identity;

namespace Kastra.DAL.EntityFramework
{
    public class DependencyRegister : IDependencyRegister
    {
        public void SetDependencyInjections(IServiceCollection services, IConfiguration configuration)
        {
            // Add database context.
            services.AddDbContext<KastraDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Add identity
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<KastraDbContext>()
                .AddDefaultTokenProviders();

            // Check if the database should be updated automatically
            AppSettings appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();

            if (appSettings != null && appSettings.Configuration.EnableDatabaseUpdate)
            {
                UpdateDatabase(services);
            }
        }

        public void SetExternalViewComponents(IServiceCollection services, IConfigurationRoot configuration)
        {

        }

        /// <summary>
        /// Updates the database.
        /// </summary>
        /// <param name="services">Services.</param>
        private static void UpdateDatabase(IServiceCollection services)
        {
            using(ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                using (var context = serviceProvider.GetService<KastraDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
