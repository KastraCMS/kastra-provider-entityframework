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
        }

        public void SetExternalViewComponents(IServiceCollection services, IConfigurationRoot configuration)
        {
            
        }
    }
}
