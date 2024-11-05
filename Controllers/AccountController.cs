using System;
using System.Collections.Generic;
using System.Web.Mvc;
using signedup.Models;
using signedup.Repositories;

namespace signedup.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserRepository userRepository;

        public AccountController()
        {
            string connectionString = "Data Source=DESKTOP-ENRUGTD\\SQLEXPRESS;Initial Catalog=suguna;Integrated Security=True;";
            userRepository = new UserRepository(connectionString); // Provide the connection string here
        }

        public ActionResult SignUp()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(User model)
        {
            if (ModelState.IsValid)
            {
                if (model.DateOfBirth < new DateTime(1753, 1, 1))
                {
                    ModelState.AddModelError("DateOfBirth", "Date of birth must be after 1/1/1753.");
                    return View(model);
                }
                userRepository.InsertUser(model);
                return RedirectToAction("Login");
            }
            return View(model);
        }

        public ActionResult Index()
        {
            List<User> users = userRepository.GetAllUsers();
            return View(users);
        }

        public ActionResult Edit(int id)
        {
            User user = userRepository.GetUserById(id);
            if (user == null)
            {   
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User model)
        {
            if (!ModelState.IsValid)
            {
                userRepository.UpdateUser(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            User user = userRepository.GetUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            userRepository.DeleteUser(id);
            return RedirectToAction("Index");
        }
        //public ActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(string username, string password)
        //{
        //    var user = userRepository.ValidateUser(username, password);

        //    if (user != null)
        //    {
        //        // You can set authentication cookie or session here
        //        Session["UserId"] = user.Id;
        //        Session["Username"] = user.Username;

        //        // Redirect to home page or dashboard after login
        //        return RedirectToAction("Index", "Home"); // Replace "Index" and "Home" with your actual home controller/action
        //    }

        //    ModelState.AddModelError("", "Invalid username or password");
        //    return View(); // Return to the Login view with an error
        //}

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (username == "Suguna" && password == "1234567A@a")
            {
                // Admin credentials matched; redirect to Admin page
                return RedirectToAction("AdminHome", "Admin"); // Adjust the action/controller as needed
            }
            else
            {
                // For other users, validate using userRepository or redirect to Home page
                var user = userRepository.ValidateUser(username, password);

                if (user != null)
                {
                    // Set session variables if required
                    Session["UserId"] = user.Id;
                    Session["Username"] = user.Username;

                    // Redirect to home page or user dashboard
                    return RedirectToAction("Index", "Employee"); // Adjust as needed
                }
            }

            // If login fails, show an error
            ModelState.AddModelError("", "Invalid username or password");
            return View(); // Return to the Login view with an error
        }

        public ActionResult GetCities(string state)     
        {
            var cities = new List<SelectListItem>();

            if (state == "Tamilnadu")
            {
                cities.Add(new SelectListItem { Text = "Chennai", Value = "Chennai" });
                cities.Add(new SelectListItem { Text = "Madurai", Value = "Madurai" });
                cities.Add(new SelectListItem { Text = "Tirunelveli", Value = "Tirunelveli" });
            }
            else if (state == "Karnataka")
            {
                cities.Add(new SelectListItem { Text = "Bangalore", Value = "Bangalore" });
                cities.Add(new SelectListItem { Text = "Mysore", Value = "Mysore" });
                cities.Add(new SelectListItem { Text = "Hubli", Value = "Hubli" });
            }
            else if (state == "Kerala")
            {
                cities.Add(new SelectListItem { Text = "Kochi", Value = "Kochi" });
                cities.Add(new SelectListItem { Text = "Thiruvananthapuram", Value = "Thiruvananthapuram" });
                cities.Add(new SelectListItem { Text = "Kollam", Value = "Kollam" });

            }

            return Json(cities, JsonRequestBehavior.AllowGet);
        }
    }
}
