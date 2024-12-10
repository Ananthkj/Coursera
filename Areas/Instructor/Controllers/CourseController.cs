using Coursera.Areas.Instructor.Models;
using Coursera.Data;
using Coursera.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        //Add Courses Single-Step Process
        public IActionResult AddCourse()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCourse(AddCourseViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

          var courseName= await _context.courses.Select(c=>c.CourseName).ToListAsync();
           
                if(courseName.Contains(model.CourseName))
                {
                    ModelState.AddModelError("", "This Course Name already Exists");
                    return View(model);

                }   

            var InstructorId = GetInstructorId();
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
            return RedirectToAction("MyCourses");
            
        }

        public async Task<IActionResult> AddSection()
        {
           var courses= await _context.courses.ToListAsync();
            ViewBag.courses = new SelectList(courses, "Id", "CourseName");
            return View();
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

            return RedirectToAction("MyCourses");
        }


        [HttpGet]
        public async Task<IActionResult> AddLesson()
        {
            var courses = await _context.courses.ToListAsync();
            ViewBag.courses = new SelectList(courses, "Id", "CourseName");
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> AddLesson(AddLessonViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var courses = await _context.courses.ToListAsync();
                ViewBag.courses = new SelectList(courses, "Id", "CourseName");
                return View(model);
            }
            var lesson = new CourseLesson
            {
                SectionId = model.SectionId,
                CourseLessonName = model.CourseLessonName,
                ContentType = model.ContentType,
                ContentUrl = model.ContentUrl
            };
            _context.courseLessons.Add(lesson);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyCourses");
        }

        public async Task<IActionResult> GetSectionsByCourse(int CourseId)
        {
            var sections=await _context.courseSections.Where(s => s.CourseId == CourseId).Select(s => new {s.Id,s.CourseSectionName}).ToListAsync();
            return Json(sections);
        }

        //End of Add Courses Multi-Step Process


        [HttpGet]
        public async Task<IActionResult> MyCourses()
        {
            var courses = await _context.courses.Where(c => c.InstructorId == GetInstructorId()).Include(c => c.sections).ThenInclude(c => c.courseLessons).ToListAsync();
            return View(courses);
        }

        public IActionResult MyCourses2()
        {
            return View();
        }

        public int GetInstructorId()
        {
            var InstructorId = User.FindFirst(ClaimTypes.NameIdentifier);
            if (InstructorId != null)
            {
                return int.Parse(InstructorId.Value);
            }
            throw new Exception("User ID not found in claims.");
        }


        public async Task<IActionResult> GetCourse(int CourseId)
        {
            var courseData = await _context.courses.Where(c => c.Id == CourseId).Select(s => new EditCourseViewModel
            { Id = s.Id, CourseName = s.CourseName, CourseDescription = s.CourseDescription }
            ).FirstOrDefaultAsync();

            if (courseData == null)
            {
                return NotFound("Course Not Found");
            }

            return PartialView("_EditCourseModal", courseData);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCourse(EditCourseViewModel model)
        {
            Console.WriteLine(model.Id);
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            var course = await _context.courses.FirstOrDefaultAsync(c => c.Id == model.Id);
            if (course == null)
            {
                return NotFound("Course not found.");
            }

            // Update course details
            course.CourseName = model.CourseName;
            course.CourseDescription = model.CourseDescription;

            await _context.SaveChangesAsync();

            return Ok("Course updated successfully.");
        }


        public async Task<IActionResult> GetSections(int CourseId)
        {
            var sections = await _context.courseSections.Where(c => c.CourseId == CourseId).Select(s => new CourseSection
            {
                Id = s.Id,
                CourseSectionName = s.CourseSectionName
            }).ToListAsync();
            return PartialView("_SectionsPartial", sections);
        }


        public async Task<IActionResult> GetSectionEdit(int SectionId)
        {
            var sections = await _context.courseSections.Where(c => c.Id == SectionId).Select(s => new EditSectionViewModel
            {
                Id = s.Id,
                CourseSectionName = s.CourseSectionName
            }).FirstOrDefaultAsync();

            if (sections == null)
            {
                return NotFound("Section Not Found");
            }
            return PartialView("_EditSectionModal", sections);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateSection(EditSectionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }
            var sections = await _context.courseSections.FirstOrDefaultAsync(s => s.Id == model.Id);
            Console.WriteLine(sections);
            if (sections == null)
            {
                return NotFound("Section Not found");
            }
            sections.CourseSectionName = model.CourseSectionName;
            await _context.SaveChangesAsync();
            return Ok("Section Update Succesfully.");
        }

        public async Task<IActionResult> GetLessons(int SectionId)
        {
            var lessons = await _context.courseLessons.Where(l => l.SectionId == SectionId).Select(s => new CourseLesson
            {
                Id = s.Id,
                CourseLessonName = s.CourseLessonName,
                ContentUrl = s.ContentUrl,
                ContentType = s.ContentType
            }).ToListAsync();
            return PartialView("_LessonsPartial", lessons);
        }

        public async Task<IActionResult> GetLessonEdit(int LessonId)
        {
            var lessons = await _context.courseLessons.Where(l => l.Id == LessonId).Select(l => new EditLessonViewModel
            {
                Id = l.Id,
                CourseLessonName = l.CourseLessonName,
                ContentUrl = l.ContentUrl,
                ContentType = l.ContentType
            }).FirstOrDefaultAsync();

            if (lessons == null)
            {
                return NotFound("Lesson Not Found");
            }

            return PartialView("_EditLessonModal", lessons);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLesson(EditLessonViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }
            var lesson = await _context.courseLessons.FirstOrDefaultAsync(l => l.Id == model.Id);
            if (lesson == null)
            {
                return NotFound("Lesson Not Found");
            }
            lesson.CourseLessonName = model.CourseLessonName;
            lesson.ContentUrl = model.ContentUrl;
            lesson.ContentType = model.ContentType;
            await _context.SaveChangesAsync();

            return Ok("Lesson Updated Succesfully");
        }



       
    }
}
