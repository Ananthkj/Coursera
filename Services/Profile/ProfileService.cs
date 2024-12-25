using Coursera.Areas.Instructor.Models;
using Coursera.Data;
using Microsoft.EntityFrameworkCore;

namespace Coursera.Services.Profile
{
    public class ProfileService:IProfileService
    {
        private readonly ApplicationDbContext _context;
        public ProfileService(ApplicationDbContext context) 
        { 
            _context = context;
        }

        public async Task<MyProfileModel> GetProfile(int InstructorUserId)
        {
            var newMyProfile = await _context.userProfiles
                .Include(u => u.user) // Include the related user
                .Where(u => u.user.Id == InstructorUserId) // Filter by InstructorId
                .Select(u => new MyProfileModel
                {
                    UserName = u.user.Name,
                    RoleName=u.user.Role.RoleName,
                    Email = u.user.Email,
                    Photo = u.Photo,
                    Subject = u.Subject,
                    UserId = u.user.Id,
                    Website = u.Website,
                    Twitter = u.Twitter,
                    Facebook = u.Facebook,
                    LinkedIn = u.LinkedIn,
                    Instagram = u.Instagram
                }).FirstOrDefaultAsync();

            return newMyProfile;
        }

    }
}
