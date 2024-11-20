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

        public IActionResult AccessDenied()
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


                return RedirectToAction("Index","Home");
            }
            return View(model);
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserModel model)
        {
            if (ModelState.IsValid)
            {
                var userFromDb=await _usersService.GetUserByEmailAsync(model.Email);
              
                if(userFromDb==null||!BCrypt.Net.BCrypt.Verify(model.Password,userFromDb.PasswordHash))
                {
                    ModelState.AddModelError("","Invalid Email or Password");
                    return View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
                    new Claim(ClaimTypes.Email, userFromDb.Email),
                    new Claim(ClaimTypes.Role, userFromDb.Role.RoleName) // Assuming the role is stored
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Set authentication properties
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Remember user across sessions
                    ExpiresUtc = DateTime.UtcNow.AddDays(30) // Cookie expiration time
                };

                // Sign the user in
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Redirect based on user role or preference
                if (userFromDb.Role.RoleName == "Admin")
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }

                if(userFromDb.Role.RoleName=="Instructor")
                {
                    return RedirectToAction("Index", "Dashboard", new {area="Instructor"});
                }

                if(userFromDb.Role.RoleName=="Student")
                {
                    return RedirectToAction("Index","Dashboard",new {area="Student"});
                }
            }
            return View(model);
        }
    }
}
