using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Domain;

namespace Dot.Net.WebApi.Data
{
    public class P7Referential : DbContext
    {
        public P7Referential(DbContextOptions<P7Referential> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<User> Users { get; set;}
    }
}