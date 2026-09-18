using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Domain;
using System.Data;
using System.Data.Common;

//namespace Dot.Net.WebApi.Data
namespace P7CreateRestApi.Data
{
    public class P7Referential : DbContext
    {

        private IDbConnection? DbConnection { get; }
        public P7Referential(DbContextOptions<P7Referential> options) : base(options) { }




        public DbSet<BidList> BidLists { get; set;}
        public DbSet<CurvePoint> CurvePoints { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<RuleName> RuleNames { get; set; }
        public DbSet<Trade> Trades { get; set; }


        // If optionsBuilder is not setup, then configure it.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DbConnection?.ConnectionString, providerOptions => providerOptions.EnableRetryOnFailure());
            }

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }
    }
}