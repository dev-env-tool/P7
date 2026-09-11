using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using P7CreateRestApi.Data;

namespace P3AddNewFunctionalityDotNetCore.Models
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<P7Referential>
    {
        //        //public DbContext CreateDbContext(string[] args)
        //        //{
        //        //    IConfigurationRoot configuration = new ConfigurationBuilder()
        //        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        //        .AddJsonFile("appsettings.json")
        //        //        .Build();
        //        //    var builder = new DbContextOptionsBuilder<DbContext>();
        //        //    var connectionString = configuration.GetConnectionString("P7ReferentialForTests");
        //        //    builder.UseSqlServer(connectionString);
        //        //    var optionBuilder = new DbContextOptionsBuilder().UseSqlServer(connectionString);
        //        //    //return new DbContext(builder.Options, configuration);
        //        //    return new P7Referential(DbContextOptions<P7Referential> optionBuilder);

        //        //}

        public P7Referential CreateDbContext(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                //.AddJsonFile($"appsettings.{env}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("P7ReferentialForTests");

            var optionsBuilder = new DbContextOptionsBuilder<P7Referential>();
            optionsBuilder.UseSqlServer(
                connectionString,
                sql => sql.MigrationsAssembly(typeof(P7Referential).Assembly.GetName().Name)
            );

            return new P7Referential(optionsBuilder.Options);
        }

    }
}
