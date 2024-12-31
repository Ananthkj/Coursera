using Coursera.Areas.Instructor.Models;
using Coursera.Models;
using Coursera.Services.Base;
using Coursera.Services.Profile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace Coursera.Controllers
{
    public class BaseController : Controller,IBaseController
    {
        private readonly IProfileService _profileService;

        private readonly IMemoryCache _cache;
       
        public BaseController(IProfileService profileService,IMemoryCache cache) 
        { 
            _profileService = profileService;
            _cache = cache;
        }

        //Better approach - Override OnActionExecutionAsync in BaseController
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await SetLayoutDataAsync();
            await next(); // Execute the action
        }
        public async Task SetLayoutDataAsync()
        {
            int instructorId = GetInstructorId();
            var userProfile = await _profileService.GetProfile(instructorId);
            //var Instructors = await _profileService.GetInstructorDetails();
            ViewData["UserPhoto"] = userProfile?.Photo ?? "/assets2/img/avatars/userProfile3.jpg";
            ViewData["UserName"] = userProfile?.UserName ?? "User";
            ViewData["RoleName"] = userProfile?.RoleName ?? "Guest";

            var cacheKey = $"InstructorCourses_{instructorId}";
            if (!_cache.TryGetValue(cacheKey,out List<MyProfileModel> courses))
            {
                courses = await _profileService.GetInstructorDetails();
                var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(30));
                _cache.Set(cacheKey, courses, cacheOptions);
            }
            ViewData["Instructor"] = courses;

             cacheKey = $"InstructorCourses_{instructorId}";
            if (!_cache.TryGetValue(cacheKey, out List<CourseViewModel> courseDetails))
            {
                courseDetails = await _profileService.DisplayCourseDetails();
                var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(30));
                _cache.Set(cacheKey, courseDetails, cacheOptions);
            }
            ViewData["CourseDetails"] = courseDetails;

        }


        protected int GetInstructorId()
        {
            var instructorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return instructorIdClaim != null && int.TryParse(instructorIdClaim.Value, out int userId)
                ? userId
                : 0;
        }

        /*  protected int GetInstructorId()
          {
              var InstructorId = User.FindFirst(ClaimTypes.NameIdentifier);
              if (InstructorId == null || string.IsNullOrEmpty(InstructorId.Value) || (! int.TryParse(InstructorId.Value,out int UserId)))
              {
                  return 0;
              }
              return UserId;
          }*/
    }
}
