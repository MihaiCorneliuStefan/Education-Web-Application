using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EducationWebApplication.Models;
using Microsoft.AspNetCore.Identity;

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
        public DbSet<EducationWebApplication.Models.Rating> Rating { get; set; }
        public DbSet<IdentityUser> Users { get; set; }


    }
}