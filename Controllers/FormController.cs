using Microsoft.AspNetCore.Mvc;

using Coursera.Models;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Coursera.Services;
using Coursera.Data;

namespace Coursera.Controllers
{
    public class FormController : Controller
    {

        public readonly ApplicationDbContext _Context;
        public readonly IEmailService _emailService;

        public FormController(IEmailService emailService,ApplicationDbContext context) { 

        this._emailService = emailService;
        this._Context = context;
        }

        public IActionResult Index()
        {
            var model = new CombinedFormViewModel();           
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitContactForm(ContactViewModel contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var email = contact.Email;
                    var subject = contact.Subject;
                    var message = contact.Message;

                    _Context.contactUs.Add(contact);
                   await _Context.SaveChangesAsync();
                    await _emailService.SendEmailAsync(email, subject, message);
                    // Handle contact form submission logic here (e.g., save to database, send email, etc.)
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Occured: {ex.Message}");
                    return Json(new { success=false, error ="An error occurred while processing your request." });
                }
               
            }
            else
            {
                var errors = ModelState
                    .Where(x => x.Key.StartsWith("contact"))
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => string.Join(", ", kvp.Value.Errors.Select(e => e.ErrorMessage))
                    );

                return Json(new { success = false, errors });
            }
        }

        [HttpPost]
        public IActionResult SubmitRegisterForm(newStudent register)
        {
            if (ModelState.IsValid)
            {
                _Context.newStudents.Add(register);
                _Context.SaveChanges(true);
                // Handle registration logic here
                return Json(new { success = true });
            }
            else
            {
                var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                                       .ToDictionary(kvp => kvp.Key, kvp => string.Join(", ", kvp.Value.Errors.Select(e => e.ErrorMessage)));
                return Json(new { success = false, errors });
            }
        }




        public IActionResult Universityform()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Universityform(UniversityFormModel model)
        {             
            if (ModelState.IsValid)
            {
                string subject = "University Registration Form Confirmation";
                string Message = "<p>We Confirmed your Registration. We will get back to you soon..</p>";
                var Email = model.Email;
                ViewBag.Lname = model.Lname;
                
                var myfile = model.File;
                try
                {
                    
                    string file_name = myfile.FileName;
                    file_name = Path.GetFileName(file_name);
                    string Upload_Folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads");
                    if (!Directory.Exists(Upload_Folder))
                    {
                        Directory.CreateDirectory(Upload_Folder);
                    }
                    string Upload_Path = Path.Combine(Upload_Folder, file_name);
                    if (System.IO.File.Exists(Upload_Path))
                    {
                        ViewBag.Upload_Status += file_name + " already Exists\n";
                        Random random = new Random();
                        file_name = random.Next(100, 1000).ToString() + file_name;                      
                        Upload_Path = Path.Combine(Upload_Folder, file_name);
                    }
                    else
                    {
                        ViewBag.Upload_Status += file_name + " Successfully Uploaded";
                    }
                    
                    var Upload_Stream = new FileStream(Upload_Path, FileMode.Create);
                    myfile.CopyToAsync(Upload_Stream);

                }catch(Exception ex)
                {
                    ViewBag.Upload_Status = $"Unable to upload file {ex.Message}";
                }
                _Context.universityFormModels.Add(model);
                _Context.SaveChanges();
                //create Email Message
               await _emailService.SendEmailAsync(Email,subject,Message);
                ViewBag.ConfirmationMessage = "A mail has been send Succesfully..";
                TempData["Fname"] = ViewBag.Upload_Status;
                TempData["ConfirmationMessage"] = ViewBag.ConfirmationMessage;
                return RedirectToAction("success");
            }     
            return View();
        }


        public IActionResult success()
        {
            ViewBag.Fname = TempData["Fname"];
            ViewBag.ConfirmationMessage = TempData["ConfirmationMessage"];
            
            return View();
        }
    }
}
