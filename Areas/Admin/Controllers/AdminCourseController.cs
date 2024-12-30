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
            var course = await _context.courses.FirstOrDefaultAsync(c => c.Id == model.courseId);
            if (course == null)
            {
                TempData["CourseError"] = "Course Not Found";
                return await PendingCourses(); // Re-fetch and return the pending courses
            }

            course.ApprovalStatus = model.ApprovalStatus;
            await _context.SaveChangesAsync();

            TempData["CourseSuccess"] = "Successfully Approved!";
            return await PendingCourses(); // Re-fetch and return the pending courses
        }





        /* public async Task<IActionResult> pendingCourses()
         {
             var pendingCourses= await _context.courses
                 .Where(c=>c.ApprovalStatus==ApprovalStatus.Pending)
                 .Select(c=>new AdminCourseViewModel
                 {
                     courseId=c.Id,
                     courseImage=c.CourseImage,
                     courseName=c.CourseName,
                     ApprovalStatus=c.ApprovalStatus,
                     IsPublished=c.IsPublished,
                 })
                 .ToListAsync();

             *//* var approvalStatus = Enum.GetValues(typeof(ApprovalStatus))
                  .Cast<ApprovalStatus>()
                  .Select(c => new SelectListItem
                  {
                      Text = c.ToString(),
                      Value = ((int)c).ToString()

                  }).ToList();*/


        /* var statusSelected = new List<SelectListItem>()
         {
             new SelectListItem(){Text="Pending",Value="0"},
             new SelectListItem(){Text="Approved",Value="1"},
             new SelectListItem(){Text="Rejected",Value="2"}
         };*//*
        ViewBag.ApprovalStatus = new SelectList(Enum.GetValues(typeof(ApprovalStatus)).Cast<ApprovalStatus>(), "Value", "Text");


        return View(pendingCourses);
    }*/

        /*  [HttpPost]
          public async Task<IActionResult> pendingCourses(AdminCourseViewModel model)
          {
              var courses = await _context.courses.FirstOrDefaultAsync(c=>c.Id==model.courseId);
              if (courses==null)
              {
                  TempData["CourseError"] = "Course Not Found";
                  return View();
              }
              courses.ApprovalStatus = model.ApprovalStatus;
             await _context.SaveChangesAsync();
              TempData["CourseSuccess"] = "Successfully Approved!";
              return View();
          }*/

        public IActionResult Success()
        {
            return View();
        }

    }
}
