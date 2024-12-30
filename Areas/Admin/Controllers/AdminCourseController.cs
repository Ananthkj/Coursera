using Coursera.Areas.Admin.Models;
using Coursera.Data;
using Coursera.Models;
using Coursera.Services.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Coursera.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class AdminCourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProfileService _profileService;
        public AdminCourseController(ApplicationDbContext context,IProfileService profileService) { 
            _context = context;
            _profileService = profileService;
        }


        public IActionResult Index()
        {
            return View();
        }


  
        public async Task<IActionResult> PendingCourses()
        {
            var pendingCourses = await _context.courses
                .Where(c => c.ApprovalStatus == ApprovalStatus.Pending)
                .Select(c => new AdminCourseViewModel
                {
                    courseId = c.Id,
                    courseImage = c.CourseImage,
                    courseName = c.CourseName,
                    ApprovalStatus = c.ApprovalStatus,
                    IsPublished = c.IsPublished
                })
                .ToListAsync();

            ViewBag.ApprovalStatus = Enum.GetValues(typeof(ApprovalStatus))
                .Cast<ApprovalStatus>()
                .Select(c => new SelectListItem
                {
                    Text = c.ToString(),
                    Value = ((int)c).ToString()
                })
                .ToList();

            return View(pendingCourses);
        }

        [HttpPost]
        public async Task<IActionResult> PendingCourses(AdminCourseViewModel model)
        {
            if(model.ApprovalStatus==ApprovalStatus.Pending)
            {
                TempData["ApprovalError"] = "Status Not selected";
                return await PendingCourses();
            }
            var course = await _context.courses.FirstOrDefaultAsync(c => c.Id == model.courseId);
            if (course == null)
            {
                TempData["CourseError"] = "Course Not Found";
                return await PendingCourses(); // Re-fetch and return the pending courses
            }

            course.ApprovalStatus = model.ApprovalStatus;
            await _context.SaveChangesAsync();

            TempData["CourseSuccess"] = "Successfully Updated!";
            return await PendingCourses(); // Re-fetch and return the pending courses
        }

        
        public IActionResult Success()
        {
            return View();
        }

    }
}
