using Kastra.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Kastra.DAL.EntityFramework
{
    public class DependencyRegister : IDependencyRegister
    {
        public void SetDependencyInjections(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<KastraContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

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
                using (var context = serviceProvider.GetService<KastraContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
