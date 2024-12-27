using Coursera.Areas.Instructor.Models;
using Coursera.Controllers;
using Coursera.Data;
using Coursera.Models;
using Coursera.Models.Account;
using Coursera.Services.Profile;
using Coursera.Services.SignUp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.IO;
using System.Security.Claims;

namespace Coursera.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor")]
    public class ProfileController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IProfileService _profileService;
         private readonly IUsersService _usersService;
        private readonly IMemoryCache _cache;
        public ProfileController(ApplicationDbContext context,IProfileService profileService,IUsersService usersService,IMemoryCache cache): base(profileService, cache)
        { 
            _context = context;
            _profileService = profileService;
            this._usersService = usersService;
            _cache = cache;
        }


        public async Task<IActionResult> Index()
        {
            //await SetLayoutDataAsync();
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
        //C:\Users\jayso\source\repos\CourseraNew\.vs\wwwroot\assets2\img\Profile\Instructor\

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile photo)
        {
            if (photo != null && photo.Length > 0)
            {
                // Validate file type and size
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(photo.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    return BadRequest(new { success = false, message = "Invalid file type." });
                }
                if (photo.Length > 800 * 1024) // 800 KB max
                {
                    return BadRequest(new { success = false, message = "File size exceeds the limit." });
                }

                // Save the file to wwwroot/uploads (create the folder if it doesn't exist)
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets2/img/Profile/Instructor");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                // Generate the URL for the saved image
                var imageUrl = Url.Content("~/assets2/img/Profile/Instructor/" + uniqueFileName);

                var UserClaimId = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier);
                /*int? InstructorId = int.Parse(UserClaimId.Value);*/
                bool InstructorId = int.TryParse(UserClaimId.Value, out int UserId);

                //var MyProfile= _context.userProfiles.Where(u => u.user.Id == InstructorId).Select(u => new { Name=u.user?.Name??null,Email=u.user?.Email??null}).ToList();         
                if (UserClaimId == null || !InstructorId)
                {
                    return NotFound("Instructor Id is Null");
                }
                var userProfile = _context.userProfiles
               .Include(u => u.user)
               .Where(u => u.user.Id == UserId)
               .FirstOrDefault();
                if (userProfile == null)
                {
                    return NotFound("User Profile Not Found");
                }
                userProfile.Photo = imageUrl;

                _context.SaveChanges();

                return Ok(new { success = true, imageUrl });
            }

            return BadRequest(new { success = false, message = "No file uploaded." });
        }

        [HttpPost]
         public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home");
        }

                public IActionResult Login()
        {
            return View();
        }

          
        /*public async Task<IActionResult> UploadPhoto(IFormFile photo)
        {
            if(photo!=null && photo.Length>0)
            {
                // Validate file type and size
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(photo.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    return BadRequest(new { success = false, message = "Invalid file type." });
                }
                if (photo.Length > 800 * 1024) // 800 KB max
                {
                    return BadRequest(new { success = false, message = "File size exceeds the limit." });
                }
                // Save the file to wwwroot/uploads (create the folder if it doesn't exist)
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets2/img/Profile/Instructor/");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var fileName = Guid.NewGuid().ToString() + "_" + photo.FileName + extension;
                var filePath=Path.Combine(uploadsFolder, fileName);
                using (var fileStream=new FileStream(filePath,FileMode.Create))
                {
                    await photo.CopyToAsync(fileStream);
                }
                var imageUrl = Url.Content("~/Instructor/" + fileName);

                return Ok(new { success = true, imageUrl });

            }
            else
            {
                return BadRequest(new {succes=false,message="No file Uploaded"});
            }
            
        }*/

    }
}
