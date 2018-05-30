using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Kastra.DAL.EntityFramework
{
    public class KastraDbContextFactory : IDesignTimeDbContextFactory<KastraContext>
    {
        public KastraContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath($"{Directory.GetCurrentDirectory()}/../Kastra.Web")
                .AddJsonFile("appsettings.json")
                .Build();
    
            var builder = new DbContextOptionsBuilder<KastraContext>();
    
            var connectionString = configuration.GetConnectionString("DefaultConnection");
    
            builder.UseSqlServer(connectionString);
    
            return new KastraContext(builder.Options);
        }
    }
}