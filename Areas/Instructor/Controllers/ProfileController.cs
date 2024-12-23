using Coursera.Areas.Instructor.Models;
using Coursera.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coursera.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor")]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProfileController(ApplicationDbContext context) 
        { 
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MyProfile()
        {
            var InstructorId = 2;
            //var MyProfile= _context.userProfiles.Where(u => u.user.Id == InstructorId).Select(u => new { Name=u.user?.Name??null,Email=u.user?.Email??null}).ToList();

         

            var MyProfile = _context.userProfiles
                .Include(u => u.user) // Include the related user
                .Where(u => u.user.Id == InstructorId) // Filter by InstructorId
                .Select(u => new MyProfileModel
                {
                    UserName = u.user.Name,
                    Email = u.user.Email,
                    Photo=u.Photo,
                    Subject=u.Subject
                }).FirstOrDefault();

           /* MyProfileModel profile = new MyProfileModel
            {
                UserName=MyProfile.UserPro
            };*/


               
            return View(MyProfile);
        }
    }
}
