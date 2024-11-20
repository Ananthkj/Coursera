using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coursera.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles ="Instructor")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
