using Coursera.Areas.Instructor.Models;
using Coursera.Controllers;
using Coursera.Data;
using Coursera.Models;
using Coursera.Services.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Security.Claims;

namespace Coursera.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor")]
    public class CourseController : InstructorBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IProfileService _profileService;
        private readonly IMemoryCache _cache;
        public CourseController(ApplicationDbContext Context,IProfileService profileService,IMemoryCache cache):base(profileService, cache)
        {
            this._context = Context;
            this._profileService = profileService;
            this._cache = cache;
        }
        public async Task<IActionResult> Index()
        {
            //await SetLayoutDataAsync();
            return View();
        }
        public async Task<IActionResult> success()
        {
            //await SetLayoutDataAsync();
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

            var courseName = await _context.courses.Select(c => c.CourseName).ToListAsync();

            if (courseName.Contains(model.CourseName))
            {
                ModelState.AddModelError("CourseName", "This Course Name already Exists");
                return View(model);

            }

            string imageUrl = null;

            if (model.formFileImage != null)
            {
                string fileName = model.formFileImage.FileName;
                var extensions = new List<string>() { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(fileName);
                if (extensions.Contains(fileExtension.ToLower()))
                {
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets2/img/courseImage");
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }
                    var courseImage = Guid.NewGuid().ToString() + "_" + fileName;
                    var UploadFilePath = Path.Combine(uploadFolder, courseImage);

                    try
                    {
                        using (var filestream = new FileStream(UploadFilePath, FileMode.Create))
                        {
                            await model.formFileImage.CopyToAsync(filestream);
                        }
                        imageUrl = Url.Content("~/assets2/img/courseImage/" + courseImage);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "File upload failed. Please try again.");
                        return View(model);
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Invalid Image Format");
                    return View(model);
                }
            }


            var InstructorId = GetInstructorId();
            var course = new Course
            {
                CourseName = model.CourseName,
                CourseDescription = model.CourseDescription,
                InstructorId = InstructorId,
                CourseImage= imageUrl ?? "/assets2/img/avatars/userProfile3.jpg",
                CreatedDate = DateTime.Now,
                ApprovalStatus = ApprovalStatus.Pending,
                IsPublished = false
            };

            try
            {
                _context.courses.Add(course);
                await _context.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the course. Please try again.");
                return View(model);
            }
            return RedirectToAction("MyCourses");

        }

        public async Task<IActionResult> AddSection()
        {
            var courses = await _context.courses.ToListAsync();
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
            var sections = await _context.courseSections.Where(s => s.CourseId == CourseId).Select(s => new { s.Id, s.CourseSectionName }).ToListAsync();
            return Json(sections);
        }

        //End of Add Courses Multi-Step Process


        [HttpGet]
        public async Task<IActionResult> MyCourses()
        {
            // Fetch courses for the current instructor
            var instructorId = GetInstructorId(); // Implement this method to fetch logged-in instructor ID
            var courses = await _context.courses
                .Where(c => c.InstructorId == instructorId)
                .Include(c => c.sections)
                .ThenInclude(s => s.courseLessons)
                .ToListAsync();

         /*   var profile = new MyProfileModel
            {
                Photo = "/assets2/img/sample-avatar.png", // Replace with actual photo path
                UserName = "Instructor Name",            // Replace with actual username
                RoleName = "Instructor"                  // Replace with actual role name
            };*/

            var viewModel = new MyProfileModel
            {               
                Courses = courses
            };

            // Pass the list of courses to the view
            return View(viewModel);
        }


        //All Courses Section
        public async Task<IActionResult> MyCourses2(string errorMessage)
        {
          var allCourses = await _context.courses
                .Select(c => new CourseViewModel{
                    CourseId = c.Id,
                    CourseName = c.CourseName,
                    CourseImage = c.CourseImage,
                    ApprovalStatus=c.ApprovalStatus,
                    IsPublished=c.IsPublished,
                }).ToListAsync();

            var courses = new MyProfileModel
            {
                AllCourses = allCourses
            };
            ViewBag.ErrorMessage = errorMessage;    

            return View(courses);
        }

        public async Task<IActionResult> EditAllCourses(int CourseId)
        {
          var userCourse=  await _context.courses
                .Select(c=>new CourseViewModel
                {
                    CourseId = c.Id,
                    CourseName = c.CourseName,
                    CourseImage = c.CourseImage,
                    ApprovalStatus=c.ApprovalStatus,
                    IsPublished=c.IsPublished
                }).FirstOrDefaultAsync(c=>c.CourseId==CourseId);    
            
           /* if (userCourse==null)
            {
                return BadRequest("No Course Find");
            }*/
            if (userCourse == null)
            {
                return RedirectToAction("MyCourses2",new { errorMessage= "No Couse Found" });
            }
            var newCourse = new MyProfileModel
            {
                courseView = userCourse
            };

            return View(newCourse);
        }

        [HttpPost]
        public async Task<IActionResult> EditAllCourses(MyProfileModel model)
        {
            var course = await _context.courses.FirstOrDefaultAsync(c => c.Id == model.courseView.CourseId);
            if (course == null)
            {
                TempData["ErrorMessage"] = "Course not found.";
                return RedirectToAction("MyCourses2");
            }

            // Update course details
            course.CourseName = model.courseView.CourseName;
            course.ApprovalStatus = model.courseView.ApprovalStatus;
            course.IsPublished = model.courseView.IsPublished;

            if (model.courseView.CourseImageFile == null)
            {
                ModelState.AddModelError("", "File upload failed. No file was provided.");
                return View(model);
            }

            // Handle image upload if a new file is provided
            if (model.courseView.CourseImageFile != null && model.courseView.CourseImageFile.Length > 0)
            {
                var imageFileName = model.courseView.CourseImageFile.FileName;
                var imageExtension = Path.GetExtension(imageFileName).ToLower();

                // Valid image extensions
                var validExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
                if (!validExtensions.Contains(imageExtension))
                {
                    ModelState.AddModelError("", "Invalid file format. Only JPG, JPEG, and PNG are allowed.");
                    return View(model);
                }

                // Ensure upload folder exists
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets2/img/courseImage");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // Generate a unique file name and save the file
                var fileName = Guid.NewGuid().ToString() + imageExtension;
                var uploadFilePath = Path.Combine(uploadFolder, fileName);

                try
                {
                    using (var fileStream = new FileStream(uploadFilePath, FileMode.Create))
                    {
                        await model.courseView.CourseImageFile.CopyToAsync(fileStream);
                    }
                    course.CourseImage = "/assets2/img/courseImage/" + fileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error uploading file: {ex.Message}");
                    return View(model);
                }
            }

            // Save changes to the database
            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Course updated successfully.";
                return RedirectToAction("MyCourses2");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error saving changes: {ex.Message}");
                return View(model);
            }
        }





        public async Task<IActionResult> DeleteAllCourses(int CourseId)
        {
            var deleteCourse = await _context.courses.FirstOrDefaultAsync(c => c.Id == CourseId);
            if (deleteCourse == null)
            {
                TempData["ErrorMessage"] = "Course not found.";
                return RedirectToAction("MyCourses2");
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Course updated successfully.";
            return RedirectToAction("MyCourses2");
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
            //course.CourseImage=model.CourseImagePath;
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
