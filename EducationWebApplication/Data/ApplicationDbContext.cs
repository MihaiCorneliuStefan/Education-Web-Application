using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EducationWebApplication.Models;

namespace EducationWebApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<EducationWebApplication.Models.Course> Course { get; set; }
        public DbSet<EducationWebApplication.Models.Quiz> Quiz { get; set; }
    }
}