using Coursera.Models;
using Microsoft.EntityFrameworkCore;

namespace Coursera.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
          
        }
        public DbSet<UniversityFormModel> universityFormModels { get; set; }

        public DbSet<ContactViewModel> contactUs {  get; set; }

        public DbSet<newStudent> newStudents { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // Mark UniversityFormModel as a keyless entity
        //    modelBuilder.Entity<UniversityFormModel>().HasNoKey();
        //}
    }
}
