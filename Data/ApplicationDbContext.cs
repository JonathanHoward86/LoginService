using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoginService.Models
{
    // Inherit from IdentityDbContext to get Identity-related DbSets like Users, Roles, etc.
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        // Constructor that takes DbContextOptions and passes it to the base class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}