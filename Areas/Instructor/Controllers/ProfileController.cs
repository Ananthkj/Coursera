using Coursera.Areas.Instructor.Models;
using Coursera.Data;
using Coursera.Models;
using Coursera.Services.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Coursera.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor")]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProfileService _profileService;
        public ProfileController(ApplicationDbContext context,IProfileService profileService) 
        { 
            _context = context;
            _profileService = profileService;
        }


        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> MyProfile()
        {
            var UserClaimId = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier);
            /*int? InstructorId = int.Parse(UserClaimId.Value);*/
          /*  bool InstructorId = int.TryParse(UserClaimId.Value, out int UserId);*/

            //var MyProfile= _context.userProfiles.Where(u => u.user.Id == InstructorId).Select(u => new { Name=u.user?.Name??null,Email=u.user?.Email??null}).ToList();         
            if (UserClaimId == null || !int.TryParse(UserClaimId.Value, out int InstructorUserId))
            {
                return NotFound("Instructor Id is Null");
            }

          var UserProfileData= await _context.userProfiles.FirstOrDefaultAsync(u=>u.UserId== InstructorUserId);
            if (UserProfileData==null)
            {
                var newProfile = new UserProfile
                {
                    UserId = InstructorUserId,
                };
                _context.userProfiles.Add(newProfile);
                _context.SaveChanges();

                var newMyProfile=await _profileService.GetProfile(InstructorUserId);
                return View(newMyProfile);
            }
            
            var MyProfile=await _profileService.GetProfile(InstructorUserId);
            return View(MyProfile);
        }


        public async Task<IActionResult> UpdateProfile()
        {
            var UserClaimId = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier);
            /*int? InstructorId = int.Parse(UserClaimId.Value);*/
            bool InstructorId = int.TryParse(UserClaimId.Value, out int UserId);

            //var MyProfile= _context.userProfiles.Where(u => u.user.Id == InstructorId).Select(u => new { Name=u.user?.Name??null,Email=u.user?.Email??null}).ToList();         
            if (UserClaimId == null || !InstructorId)
            {
                return NotFound("Instructor Id is Null");
            }
            var MyProfile =await _profileService.GetProfile(UserId);

            return View(MyProfile);
        }

        [HttpPost]
        public IActionResult UpdateProfile(MyProfileModel model)
        {
            if (ModelState.IsValid)
            {
                var userProfile = _context.userProfiles
                .Include(u => u.user)
                .Where(u => u.user.Id == model.UserId)
                .FirstOrDefault();
                if (userProfile == null)
                {
                    return NotFound("User Profile Not Found");
                }
                userProfile.user.Name= model.UserName;
                userProfile.Photo = model.Photo;
                userProfile.Subject = model.Subject;
                userProfile.Website = model.Website;
                userProfile.Twitter = model.Twitter;
                userProfile.Facebook = model.Facebook;
                userProfile.LinkedIn = model.LinkedIn;
                userProfile.Instagram = model.Instagram;

                _context.SaveChanges();
                return RedirectToAction("MyProfile");
            }
            
            return View(model);
        }

       
    }
}
