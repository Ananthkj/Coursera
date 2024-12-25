using Coursera.Models;
using Microsoft.EntityFrameworkCore;

namespace Coursera.Data
{

    public class ApplicationDbContext:DbContext
    {
        public DbSet<UniversityFormModel> universityFormModels { get; set; }

        public DbSet<ContactViewModel> contactUs { get; set; }

        public DbSet<newStudent> newStudents { get; set; }

        public DbSet<User> users { get; set; }
        public DbSet<Role>roles { get; set; }

        public DbSet<Course> courses {  get; set; }
        
        public DbSet<CourseSection> courseSections { get; set; }

        public DbSet<CourseLesson> courseLessons { get; set; }

        public DbSet<UserProfile> userProfiles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
          
        }

        // Configure and Customize the relationship,constraints,and Behaviour of your database entities
        /*      protected override void OnModelCreating(ModelBuilder modelBuilder)
              {
                  base.OnModelCreating(modelBuilder);
                  modelBuilder.Entity<User>()
                  .HasOne(u => u.Profile)
                  .WithOne(p => p.user)
                  .HasForeignKey<UserProfile>(p => p.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

              }*/


        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "Admin" },
                new Role { Id = 2, RoleName = "Instructor" },
                new Role { Id = 3, RoleName = "Student" }
                );
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserProfile>().HasData(
                new UserProfile()
                {
                    Id = 1,
                    Photo = "path/to/photo1.jpg",
                    Subject = "Web Development",
                    UserId = 2, // Assuming you have a User with Id = 1
                    Website = "https://www.example.com",
                    Twitter = "https://twitter.com/user1",
                    Facebook = "https://www.facebook.com/user1",
                    LinkedIn = "https://www.linkedin.com/in/user1",
                    Instagram = "https://www.instagram.com/user1"
                }
                );

        }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // Mark UniversityFormModel as a keyless entity
        //    modelBuilder.Entity<UniversityFormModel>().HasNoKey();
        //}
    }
}
