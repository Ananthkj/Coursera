﻿using Coursera.Data;
using Coursera.Models;
using Microsoft.EntityFrameworkCore;

namespace Coursera.Services.SeedService
{
    public class SeedService
    {
        private readonly ApplicationDbContext _context;
        public SeedService(ApplicationDbContext context)
        { 
            this._context = context;
        }

        public async void Seed()
        {
            /*_context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Users',RESEED,0)");*/
            // Check if roles already exist
           if(!_context.roles.Any(r=>r.RoleName=="Admin"))
            {
                _context.roles.Add(new Role() { RoleName="Admin"});
                _context.SaveChanges();
            }

            if (!_context.roles.Any(r => r.RoleName == "Instructor"))
            {
                _context.roles.Add(new Role() { RoleName = "Instructor" });
                _context.SaveChanges();
            }

            if (!_context.roles.Any(r => r.RoleName == "Student"))
            {
                _context.roles.Add(new Role() { RoleName = "Student" });
                _context.SaveChanges();
            }

            if (!_context.users.Any(u => u.Email == "admin@gmail.com"))
            {
                if(!_context.users.Any())
                {
                    await seedTableColumn(_context);
                }


                var adminRoleId = _context.roles.FirstOrDefault(r => r.RoleName == "Admin").Id;
                _context.users.Add(new User()
                {
                    
                    Name="Admin",
                    Email= "admin@gmail.com",
                    PasswordHash=BCrypt.Net.BCrypt.HashPassword("admin@123"),
                    CreatedDate= DateTime.Now,
                    IsActive= false,
                    RoleId=adminRoleId
                });
                _context.SaveChanges();
            }

        }

        private async Task seedTableColumn(ApplicationDbContext _context)
        {
            var sqlQuery = $"DBCC CHECKIDENT('users',RESEED,0)";
           await _context.Database.ExecuteSqlRawAsync(sqlQuery);
        }

        public void Seed2()
        {
            if(!_context.userProfiles.Any())
            {
                var userProfile = new UserProfile()
                {
                    Photo = "path/to/photo1.jpg",
                    Subject = "Web Development",
                    UserId = 2,
                    Website = "https://www.example.com",
                    Twitter = "https://twitter.com/user1",
                    Facebook = "https://www.facebook.com/user1",
                    LinkedIn = "https://www.linkedin.com/in/user1",
                    Instagram = "https://www.instagram.com/user1"
                };
                _context.userProfiles.Add(userProfile);
                _context.SaveChanges();
            }
        }
    }
}
