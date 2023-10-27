using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TVShowTracker.Model;
using TVShowTracker.Contracts.Request;

namespace TVShowTracker.Data
{
     public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
     {
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

       public DbSet<Show> Shows { get; set; }
      public DbSet<Episode> Episodes { get; set; }

    }

}
