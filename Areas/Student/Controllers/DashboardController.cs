using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coursera.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles ="Student")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult tables()
        {
            return View();
        }

        public IActionResult Index2()
        {
            return View();
        }
    }
}
