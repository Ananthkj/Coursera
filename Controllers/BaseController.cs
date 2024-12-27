using Coursera.Services.Profile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Coursera.Controllers
{
    public class BaseController : Controller
    {
        private readonly IProfileService _profileService;
       
        public BaseController(IProfileService profileService) 
        { 
            _profileService = profileService;
        }

        //Better approach - Override OnActionExecutionAsync in BaseController
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await SetLayoutDataAsync();
            await next(); // Execute the action
        }
        protected async Task SetLayoutDataAsync()
        {
            int instructorId = GetInstructorId();
            var userProfile = await _profileService.GetProfile(instructorId);
            ViewData["UserPhoto"] = userProfile?.Photo ?? "/default.png";
            ViewData["UserName"] = userProfile?.UserName ?? "User";
            ViewData["RoleName"] = userProfile?.RoleName ?? "Guest";
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
