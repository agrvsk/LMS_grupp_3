using Domain.Models.Entities;
using LMS.Infractructure.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ModuleActivity> Activities { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Document>()
        .HasOne(d => d.Uploader)
        .WithMany() // or .WithMany(u => u.Documents) if you add a collection
        .HasForeignKey(d => d.UploaderId)
        .OnDelete(DeleteBehavior.Restrict);

            builder.ApplyConfiguration(new ApplicationUserConfigurations());
        }

    }
}
