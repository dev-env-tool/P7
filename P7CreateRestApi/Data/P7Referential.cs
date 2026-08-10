using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Repositories;
using Microsoft.EntityFrameworkCore;

//namespace Dot.Net.WebApi.Data
namespace P7CreateRestApi.Data
{
    public class P7Referential : DbContext
    {
        public P7Referential(DbContextOptions<P7Referential> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<BidList> BidLists { get; set;}
        public DbSet<CurvePoint> CurvePoints { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<RuleName> RuleNames { get; set; }
        public DbSet<Trade> Trades { get; set; }

    }
}