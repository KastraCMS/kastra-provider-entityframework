using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Kastra.DAL.EntityFramework
{
    public class KastraDbContextFactory : IDesignTimeDbContextFactory<KastraDbContext>
    {
        public KastraDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath($"{Directory.GetCurrentDirectory()}")
                .AddJsonFile("appsettings.json")
                .Build();
    
            var builder = new DbContextOptionsBuilder<KastraDbContext>();
    
            var connectionString = configuration.GetConnectionString("DefaultConnection");
    
            builder.UseSqlServer(connectionString);
    
            return new KastraDbContext(builder.Options);
        }
    }
}