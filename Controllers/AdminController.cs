using Microsoft.AspNetCore.Mvc;

namespace Coursera.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
