using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizzCreate.Models.Identity;

namespace QuizzCreate.Data
{
    public class DbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


    }
}
