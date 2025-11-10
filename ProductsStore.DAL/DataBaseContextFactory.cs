using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ProductsStore.DAL
{
    public class DataBaseContextFactory : IDesignTimeDbContextFactory<DataBaseContext>
    {
        //Фабрика для создания контекста для работы миграций
        public DataBaseContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("TestConnection");
            if (connectionString == null)
            {
                throw new Exception("no db connection string");
            }
            var optionsBuilder = new DbContextOptionsBuilder<DataBaseContext>();
            optionsBuilder.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
            return new DataBaseContext(optionsBuilder.Options);
        }
    }
}
