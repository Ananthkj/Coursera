using Coursera.Areas.Instructor.Models;
using Coursera.Controllers;
using Coursera.Data;
using Coursera.Services.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace Coursera.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles ="Instructor")]
    public class DashboardController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IProfileService _profileService;
        private readonly IMemoryCache _cache;

        public DashboardController(ApplicationDbContext context,IProfileService profileService,IMemoryCache cache): base(profileService, cache)
        {
            this._context = context;
            this._profileService = profileService;
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        /*public async Task<IActionResult> Index()
        {
            var instructorId = GetInstructorId(); // Fetch the instructor ID
            var userProfile = await _profileService.GetProfile(instructorId); // Get profile data
            var courses = await _context.courses
                .Where(c => c.InstructorId == instructorId)
                .Include(c => c.sections)
                .ThenInclude(s => s.courseLessons)
                .ToListAsync(); // Fetch associated courses

            var model = new MyProfileModel
            {
                UserName = userProfile.UserName,
                RoleName = userProfile.RoleName,
                Email = userProfile.Email,
                Photo = userProfile.Photo,
                Subject = userProfile.Subject,
                UserId = userProfile.UserId,
                Website = userProfile.Website,
                Twitter = userProfile.Twitter,
                Facebook = userProfile.Facebook,
                LinkedIn = userProfile.LinkedIn,
                Instagram = userProfile.Instagram,
                Courses = courses // Include courses
            };

            return View(model);
        }*/

        /*public int GetInstructorId()
        {
            var InstructorId = User.FindFirst(ClaimTypes.NameIdentifier);
            if (InstructorId != null)
            {
                return int.Parse(InstructorId.Value);
            }
            throw new Exception("User ID not found in claims.");
        }*/
    }
}
