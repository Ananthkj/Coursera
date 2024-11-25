using Coursera.Areas.Admin.Models;
using Coursera.Data;
using Coursera.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Coursera.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class UserManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserManagementController(ApplicationDbContext context) 
        {
            this._context = context;
        }
        public async Task<IActionResult> Index()
        {
         var userList= await _context.users.Include(u=>u.Role).ToListAsync();
            return View(userList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Fetch roles from the database (replace with your actual data source)
            var roles = _context.roles
                .Select(r => new { r.Id, r.RoleName }) // Fetch Role Id and Name
                .ToList();

            // Pass the roles list to ViewBag
            ViewBag.Roles = new SelectList(roles, "Id", "RoleName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash); // Hash password
                user.CreatedDate = DateTime.Now;
                user.IsActive = false;

                _context.users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index"); // Redirect to user list or success page
            }

            // Repopulate roles dropdown if validation fails
            var roles = await _context.roles.Select(r => new { r.Id, r.RoleName }).ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "RoleName");

            return View(user); // Return view with validation errors
        }



        public async Task<IActionResult> Edit(int Id)
        {
          var user= await _context.users.FindAsync(Id);
            if(user==null)
            {
                return NotFound();
            }

            var editUserViewModel = new EditUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                RoleId = user.RoleId,
                IsActive = user.IsActive
            };

            var roles= await _context.roles.Select(r => new { r.Id, r.RoleName }).ToListAsync();

            ViewBag.Roles = new SelectList(roles,"Id","RoleName");
            return View(editUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel user)
        {
            if(ModelState.IsValid)
            {
                var existingUser = await _context.users.FindAsync(user.Id);

                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.Name = user.Name;
                existingUser.RoleId = user.RoleId;
                existingUser.IsActive = user.IsActive;


                _context.users.Update(existingUser);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var roles = await _context.roles.Select(r => new { r.Id, r.RoleName }).ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "RoleName");
            return View(user);
        }



        public async Task<IActionResult> Delete(int id)
        {
           var user= await _context.users.FindAsync(id);
            if(user == null)
            {
                return NotFound();
            }
             _context.users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
