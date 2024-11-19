using Coursera.Data;
using Coursera.Models;
using Coursera.Models.Account;
using Coursera.Services.SignUp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Crypto.Generators;
using System.Security.Claims;

namespace Coursera.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUsersService _usersService;
        public AccountController(ApplicationDbContext context,IUsersService usersService)
        {
            this._context = context;
            this._usersService = usersService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpUserModel model)
        {
          
            if(ModelState.IsValid)
            {
              var existingEmail=  _context.users.FirstOrDefault(e=>e.Email==model.Email);
                if (existingEmail != null)
                {
                    ModelState.AddModelError("","Email Already Exist..");
                    return View();
                }
                var user = new User
                {
                    Name = model.Username,
                    Email = model.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    CreatedDate = DateTime.Now,
                    IsActive = false,
                    RoleId = 3

                };
                await _usersService.AddUserByAsync(user);

             var savedUser=  await _usersService.GetUserByEmailAsync(user.Email);

                HttpContext.Response.Cookies.Append("UserId",savedUser.Id.ToString(),new CookieOptions
                {
                    HttpOnly=true,
                    Expires = DateTime.UtcNow.AddDays(30)
                    
                });

                HttpContext.Session.SetString("RecentActivity", $"Registered: {DateTime.Now}");

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,savedUser.Id.ToString()),
                    new Claim(ClaimTypes.Email,savedUser.Email),
                    new Claim(ClaimTypes.Role,savedUser.Role.RoleName)
                };
                var claimsIdentity= new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);


                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
