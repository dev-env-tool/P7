using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Domain;
using System.Data;

namespace P7CreateRestApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        private IDbConnection DbConnection { get; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration config) :
            base(options)
        {
            DbConnection = new SqlConnection(config.GetConnectionString("P7Identity"));
        }

        public ApplicationDbContext(DbContextOptions options, ConfigurationBuilder configuration) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DbConnection.ConnectionString, providerOptions => providerOptions.EnableRetryOnFailure());
            }
        }

        public override DbSet<User> Users { get; set; }
        public ConfigurationBuilder Configuration { get; }
    }
}