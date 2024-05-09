using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using ProjectScope.Models;
using ProjectScope.ViewModels;
using ProjectScope.Data;
using System.Diagnostics;
using System.Text.Json;
using ProjectScope.Entity;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using ProjectScope.Data.Repo;
using Microsoft.AspNetCore.Hosting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace ProjectScope.Controllers
{
    public class WebHomeController : Controller
    {
        private readonly ILogger<WebHomeController> _logger;
        private readonly IStudent _student;
        private readonly IContact _contact;
        private readonly ICourse _course;

        public WebHomeController(ILogger<WebHomeController> logger, IStudent student ,IContact contact,ICourse course)
        {
            _logger = logger;
            _student = student;
            _contact = contact;
            _course = course;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Aboutus()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Contact(ContactViewModel contactViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Contact", contactViewModel);
            }

            var contact = new Contact
            {
                Name = contactViewModel.Name,
                Email = contactViewModel.Email,
                Subject = contactViewModel.Subject,
                Message = contactViewModel.Message
            };

            try
            {
                _contact.Insert(contact);

                SendEmailNotification(contact.Email);

                ViewBag.Message = "Successfully submitted your details.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Unable to send email right now. Error: {ex.Message}";
            }

            return View("Contact", new ContactViewModel());
        }


        public void SendEmailNotification(string userEmail)
        {
            var message = new MimeMessage();
            message.To.Add(MailboxAddress.Parse(userEmail));
            message.From.Add(MailboxAddress.Parse("adrianprasad05@gmail.com"));
            message.Subject = "Successfully Received";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "<h3>Thank you for contacting us. We have received your details.</h3>"
            };

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtpClient.Authenticate("adrianprasad05@gmail.com", "mevd zmlw frhp qsfc");
                smtpClient.Send(message);
                smtpClient.Disconnect(true);
            }
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>  Registration(Student student,IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                return View("Registration", student);
            }
            student.Avatar = Image.FileName;
            if (Image != null && Image.Length > 0)
            {
                var uploadsDirectory = "C:\\Users\\Adrian\\source\\repos\\ProjectScope\\ProjectScope\\wwwroot\\images\\Avatar";

                if (!Directory.Exists(uploadsDirectory))
                {
                    Directory.CreateDirectory(uploadsDirectory);
                }

                var uniqueFileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(Image.FileName);

                var filePath = Path.Combine(uploadsDirectory, Image.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }
            }
         

                MimeMessage message = new MimeMessage();
            message.To.Add(MailboxAddress.Parse(student.Email));
            message.From.Add(MailboxAddress.Parse("adrianprasad05@gmail.com"));
            message.Subject = "Registration Successful";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"<h1>Registration successful</h1>" +
                $"<h2>First Name : {student.FirstName}</h2>" +
                $"<h2>Last Name : {student.LastName}</h2>" +
                $"<h2>First Name : {student.Gender}</h2>"+
                $"<h2>Date of Birth : {student.DateofBirth}</h2>" +
                $"<h2>Email : {student.Email}</h2>" +
                $"<h2>Phone : {student.Phone}</h2>" +
                $"<h2>Country : {student.Country}</h2>"+
                $"<h2>State : {student.State}</h2>" +
                $"<h2>City : {student.City}</h2>" +
                $"<h2>Hobbies : {student.Hobbies}</h2>"
            };

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

            smtpClient.Authenticate("adrianprasad05@gmail.com", "mevd zmlw frhp qsfc");
            smtpClient.Send(message);
            smtpClient.Disconnect(true);
            _student.Insert(student);
            
            return RedirectToAction(nameof(StudentLogin));
        }

        public IActionResult FirstTimeLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FirstTimeLogin(string Email)
        {
            if (!ModelState.IsValid)
            {
                return View("FirstTimeLogin");
            }

            var student = _student.Get(Email);
            if (student != null)
            {
                Random random = new Random();
                int tempPassword = random.Next(100000, 999999);

                student.TemporaryPassword = tempPassword.ToString();
                _student.Update(student);

                HttpContext.Session.SetString("LoginSessionEmail", Email);
                HttpContext.Session.SetString("LoginSessionTempPassword", tempPassword.ToString());

                SendTempPasswordEmail(Email, tempPassword);

                LoginViewModel viewModel = new LoginViewModel
                {
                    Email = Email
                };

                return View("Login", viewModel);
            }

            ModelState.AddModelError("Email", "Invalid Email");
            return View("FirstTimeLogin");
        }

        public void SendTempPasswordEmail(string email, int tempPassword)
        {
            var message = new MimeMessage();
            message.To.Add(MailboxAddress.Parse(email));
            message.From.Add(MailboxAddress.Parse("adrianprasad05@gmail.com"));
            message.Subject = "Temporary Password";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"<h1>Your temporary password is:</h1>" +
                       $"<h2>{tempPassword}</h2>"
            };

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtpClient.Authenticate("adrianprasad05@gmail.com", "rhpw abbf xphq vrva");
                smtpClient.Send(message);
                smtpClient.Disconnect(true);
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", loginModel);
            }

            var email = HttpContext.Session.GetString("LoginSessionEmail");
            var tempPassword = HttpContext.Session.GetString("LoginSessionTempPassword");

            if (email != null && tempPassword != null && loginModel.Otp == tempPassword)
            {
                var student = _student.Get(email);
                if (student != null)
                {
                    student.Password = loginModel.Password;
                    student.TemporaryPassword = null; 
                    _student.Update(student); 

                    HttpContext.Session.Remove("LoginSessionEmail");
                    HttpContext.Session.Remove("LoginSessionTempPassword");

                    return RedirectToAction("StudentLogin"); 
                }
            }

            ModelState.AddModelError("Otp", "Invalid Temporary Password");
            return View("Login", loginModel); 
        }



        public IActionResult StudentLogin()
        {
            LoginMainViewModel loginMainViewModel= new LoginMainViewModel();

            if (Request.Cookies["Authenticated"] == "true")
            {
                ViewBag.check="true";
            }
            else
            {
                ViewBag.check="false";
            }
                return View();
        }

        [HttpPost]
        public IActionResult StudentLogin(LoginMainViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("StudentLogin", model);
            }
            else
            {
                var student = _student.Get(model.Email);

                if (student != null && student.Password == model.Password)
                {
                    HttpContext.Session.SetString("StudentId", student.Id.ToString());

                    if (model.KeepLoggedIn)
                    {
                        Response.Cookies.Append("Authenticated", "true", new CookieOptions
                        {
                            Expires = DateTimeOffset.Now.AddDays(30) 
                        });
                    }
                    else
                    {
                        HttpContext.Session.SetString("KeepLoggedIn", "true");
                    }

                    return RedirectToAction("StudentDashboard", "WebHome");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password");
                    return View("StudentLogin", model);
                }
            }
        }



        public IActionResult ResetByEmail()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ResetByEmail(string Email)
        {
            if (!ModelState.IsValid)
            {
                return View("ResetByEmail");
            }

            var student = _student.Get(Email);
            if (student != null)
            {
                Random random = new Random();
                int tempPassword = random.Next(100000, 999999);

                student.TemporaryPassword = tempPassword.ToString();
                _student.Update(student);

                HttpContext.Session.SetString("LoginSessionEmail", Email);
                HttpContext.Session.SetString("LoginSessionTempPassword", tempPassword.ToString());

                SendTempPasswordEmail(Email, tempPassword);

                ResetPasswordViewModel viewModel = new ResetPasswordViewModel
                {
                    Email = Email
                };

                return View("ResetPassword", viewModel);
            }

            ModelState.AddModelError("Email", "Invalid Email");
            return View("FirstTimeLogin");
        }

        public void SendTempPassEmail(string email, int tempPassword)
        {
            var message = new MimeMessage();
            message.To.Add(MailboxAddress.Parse(email));
            message.From.Add(MailboxAddress.Parse("adrianprasad05@gmail.com"));
            message.Subject = "Temporary Password";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"<h1>Your temporary password is:</h1>" +
                       $"<h2>{tempPassword}</h2>"
            };

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtpClient.Authenticate("adrianprasad05@gmail.com", "rhpw abbf xphq vrva");
                smtpClient.Send(message);
                smtpClient.Disconnect(true);
            }
        }

        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel resetmodel)
        {
            if (!ModelState.IsValid)
            {
                return View("ResetPassword", resetmodel);
            }

            var email = HttpContext.Session.GetString("LoginSessionEmail");
            var tempPassword = HttpContext.Session.GetString("LoginSessionTempPassword");

            if (email != null && tempPassword != null && resetmodel.TempPassword == tempPassword)
            {
                var student = _student.Get(email);
                if (student != null)
                {
                    student.Password = resetmodel.NewPassword; 
                    student.TemporaryPassword = null; 
                    _student.Update(student); 

                    HttpContext.Session.Remove("LoginSessionEmail");
                    HttpContext.Session.Remove("LoginSessionTempPassword");

                    return RedirectToAction("StudentLogin"); 
                }
            }

            ModelState.AddModelError("Otp", "Invalid Temporary Password");
            return View("ResetPassword", resetmodel);
        }

        public IActionResult StudentDashboard()
        {
            ViewData["Nav"] = "Navbar";
            if (Request.Cookies["Authenticated"]== "true")
            { 
            return View();
            }
            
            else if(HttpContext.Session.GetString("keeplogin") != null&& HttpContext.Session.GetString("keeplogin") != string.Empty)
            {
                HttpContext.Session.Remove("keeplogin");

                return View();
            }
            else
            {
                return RedirectToAction("StudentLogin", "WebHome");
            }
        }

        [HttpGet]
        public IActionResult StudentDashboard(string a)
        {
            ViewData["Nav"] = "Navbar";

            var studentId = HttpContext.Session.GetString("StudentId");

            if (string.IsNullOrEmpty(studentId))
            {
                return RedirectToAction("StudentDashboard", "WebHome");
            }

            var student = _student.GetById(Convert.ToInt32(studentId));


            var courses = _course.GetAll();

            if(student.CourseId!=null)
            {
            string[] studentid=student.CourseId.Split(",");
                var query = from course in courses
                            where studentid.Contains(course.Id.ToString())
                            select new
                            {
                                CourseName = course.CourseName,
                                Duration = course.Duration,
                                Fee = course.Fee
                            };
                ViewBag.EnrolledCourses = query.ToList();
            }
            else
            {
                ViewBag.EnrolledCourses = string.Empty;

            }
            return View(courses);
        }

        public IActionResult SignUp(int Id)
        {
            var studentId = HttpContext.Session.GetString("StudentId");

            if (string.IsNullOrEmpty(studentId))
            {
                return RedirectToAction("StudentDashboard", "WebHome"); 
            }

            int studentIdInt = Convert.ToInt32(studentId);

           _student.SaveCourseId(Id, studentIdInt);

            return RedirectToAction("StudentDashboard");
        }



        public IActionResult Logout()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            HttpContext.Session.Clear();

            return RedirectToAction("StudentLogin", "WebHome");
        }


        public IActionResult Profile()
        {
            ViewData["Nav"] = "Navbar";
            return View();
        }
        [HttpGet]
        public IActionResult Profile(int id)
        {
            ViewData["Nav"] = "Navbar";
            id = Convert.ToInt16(HttpContext.Session.GetString("StudentId"));


            var student = _student.GetById(id);
            var json=JsonConvert.SerializeObject(student);
            HttpContext.Session.SetString("ProfileData", json);

            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }
        public IActionResult ChangePassword()
        {
            ViewData["Nav"] = "Navbar";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            ViewData["Nav"] = "Navbar";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var id = Convert.ToInt16(HttpContext.Session.GetString("StudentId"));

            var student = _student.GetById(id);

            if (student == null)
            {
                return RedirectToAction("Error");
            }

            if (student.Password != model.OldPassword)
            {
                ModelState.AddModelError("OldPassword", "Incorrect old password");
                return View(model);
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match");
                return View(model);
            }

            student.Password = model.NewPassword;
            _student.Update(student);
            _student.SaveChanges();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("StudentLogin");
        }


        public IActionResult EditProfile()
        {
            ViewData["Nav"] = "Navbar";

            var profiledata = HttpContext.Session.GetString("ProfileData");

            if (profiledata != null)
            {
                var student = JsonConvert.DeserializeObject<Student>(profiledata);

                ViewBag.State = student.State;
                ViewBag.City = student.City;

                var viewModel = new RegistrationViewModel
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Gender = student.Gender,
                    DateofBirth = student.DateofBirth,
                    Email = student.Email,
                    Phone = student.Phone,
                    Country = student.Country,
                    State = student.State,
                    City = student.City,
                    Hobbies = student.Hobbies,
                    Avatar = student.Avatar
                };

                return View(viewModel);
            }
            return View();
        }

        [HttpPost]
        public IActionResult EditProfile(RegistrationViewModel viewModel, IFormFile image)
        {
            ViewData["Nav"] = "Navbar";

            //if (ModelState.IsValid)
            //{
                var profiledata = HttpContext.Session.GetString("ProfileData");
                if (profiledata != null)
                {
                    var student = JsonConvert.DeserializeObject<Student>(profiledata);

                    student.FirstName = viewModel.FirstName;
                    student.LastName = viewModel.LastName;
                    student.Gender = viewModel.Gender;
                    student.DateofBirth = viewModel.DateofBirth;
                    student.Email = viewModel.Email;
                    student.Phone = viewModel.Phone;
                    student.Country = viewModel.Country;
                    student.State = viewModel.State;
                    student.City = viewModel.City;
                    student.Hobbies = viewModel.Hobbies;

                    _student.Update(student);
                    _student.SaveChanges();
                }

            //return View(viewModel);
            //}
            return RedirectToAction("Profile");
        }

        public IActionResult ProfileUpdated()
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
