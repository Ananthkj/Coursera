using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Coursera.Models;
using Microsoft.Data.SqlClient;

namespace Coursera.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;
        private readonly ILogger<HomeController> _logger;
        private readonly EmailServices _emailService;

        public HomeController(ILogger<HomeController> logger,EmailServices emailService, IConfiguration configuration)
        {
            _logger = logger;
            _emailService = emailService;
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }


        public IActionResult index()
        {
            var model = new CombinedFormViewModel();
            return View(model);
            //return View();
        }

        public IActionResult contactform()
        {
            return View();
        }

        public IActionResult about()
        {
            return View();
        }

        public IActionResult courses()
        {
            return View();
        }

        public IActionResult contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(Student stu)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileName(stu.File.FileName);
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                string uploadFilePath = Path.Combine(uploadPath, fileName);

                if (System.IO.File.Exists(uploadFilePath))
                {
                    Random random = new Random();
                    fileName = random.Next(1000, 10000) + "_" + fileName;
                    uploadFilePath = Path.Combine(uploadPath, fileName);
                }

                using (var stream = new FileStream(uploadFilePath, FileMode.Create))
                {
                    await stu.File.CopyToAsync(stream);
                }

                return RedirectToAction("Success");
            }

            return View("Registration", stu);
        }



        public IActionResult registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult registration(Student stu)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Fname = stu.Fname;
                ViewBag.Lname = stu.Lname;
                ViewBag.Email = stu.Email;
                ViewBag.Phone = stu.Phone;
                ViewBag.Gender = stu.Gender;
                ViewBag.DateOfBirth = stu.DateOfBirth;
                ViewBag.City = stu.City;
                ViewBag.State = stu.State;
                ViewBag.Country = stu.Country;

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sqlQuery = "INSERT INTO Employees (Fname, Lname) " +
                                      "VALUES (@Fname, @Lname)";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Fname", stu.Fname);
                        cmd.Parameters.AddWithValue("@Lname", stu.Lname);
                       
                        cmd.ExecuteNonQuery(); // Execute the insert query
                    }
                }

                return View("success");

            }
            return View();
        }

        public IActionResult success()
        {
            return View();
        }

        public IActionResult errormsg()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
