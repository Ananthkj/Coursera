using Coursera.Areas.Instructor.Models;
using Coursera.Data;
using Coursera.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Coursera.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor")]
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CourseController(ApplicationDbContext Context) 
        { 
            this._context = Context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddCourse()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCourse(AddCourseViewModel model)
        {
            if(!ModelState.IsValid)return View(model);

            var InstructorId=GetInstructorId();
            var course = new Course
            {
                CourseName = model.CourseName,
                CourseDescription = model.CourseDescription,
                InstructorId = InstructorId,
                CreatedDate = DateTime.Now,
                ApprovalStatus = ApprovalStatus.Pending,
                IsPublished = false
            };
            _context.courses.Add(course);   
            await _context.SaveChangesAsync();
          

            return RedirectToAction("AddSection",new {CourseId=course.Id});
        }

        public IActionResult AddSection(int CourseId)
        {
            var sectionData = new AddSectionViewModel()
            {
                CourseId = CourseId,

            };
            return View(sectionData);
        }


        [HttpPost]
        public async Task<IActionResult> AddSection(AddSectionViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var Section = new CourseSection
            {
                CourseId = model.CourseId,
                CourseSectionName = model.CourseSectionName
            };
            _context.courseSections.Add(Section);
            await _context.SaveChangesAsync();
            return RedirectToAction("AddLesson", new {SectionId=Section.Id});
        }

        public IActionResult AddLesson(int SectionId)
        {
            var Lesson = new AddLessonViewModel
            {
                CourseId = SectionId
            };
            return View(Lesson);
        }

        [HttpPost]
        public async Task<IActionResult> AddLesson(AddLessonViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var lesson = new CourseLesson
            {
                SectionId = model.CourseId,
                CourseLessonName = model.CourseLessonName,
                ContentType = model.ContentType,
                ContentUrl = model.ContentUrl
            };
            _context.courseLessons.Add(lesson);
            await _context.SaveChangesAsync();
            return RedirectToAction("AddLesson",new {SectionId=model.CourseId});
        }

        [HttpPost]
        public async Task<IActionResult> CompleteLesson(AddLessonViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var lesson = new CourseLesson
            {
                SectionId = model.CourseId,
                CourseLessonName = model.CourseLessonName,
                ContentType = model.ContentType,
                ContentUrl = model.ContentUrl
            };
            _context.courseLessons.Add(lesson);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> MyCourses()
        {
         var courses= await _context.courses.Where(c=>c.InstructorId==GetInstructorId()).Include(c=>c.sections).ThenInclude(c=>c.courseLessons).ToListAsync();          
            return View(courses);
        }


        public int GetInstructorId()
        {
          var InstructorId =  User.FindFirst(ClaimTypes.NameIdentifier);
            if(InstructorId!=null)
            {
                return int.Parse(InstructorId.Value);
            }          
            throw new Exception("User ID not found in claims.");
        }

    }
}
