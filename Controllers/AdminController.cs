using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using signedup.Models;
using signedup.Repositories;
//using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Reflection;

//using Microsoft.AspNet.Identity; // Make sure you have this using directive

namespace signedup.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserRepository userRepository;
        private readonly TaskRepository taskRepository;

        public AdminController()
        {
            string connectionString = "Data Source=DESKTOP-ENRUGTD\\SQLEXPRESS;Initial Catalog=suguna;Integrated Security=True;";
            userRepository = new UserRepository(connectionString);
            taskRepository = new TaskRepository(connectionString);
        }
        public ActionResult AdminHomepage()
        {
            return View(); // Ensure this view exists in the Views/Admin folder
        }

        // GET: AdminHome
        public ActionResult AdminHome()
        {
            return View();
        }

        // GET: New Employees
        public ActionResult NewEmployees()
        {
            // Get all users from the database
            List<User> users = userRepository.GetAllUsers();
            return PartialView("NewEmployees", users);
        }

        // GET: Assigned Tasks
        public ActionResult UnassignedTasks()
        {
            // Fetch assigned tasks from the repository
            List<Task> unassignedTasks = taskRepository.GetUnassignedTasks(); // Ensure this method is implemented in your repository
            //return View(assignedTasks);
            return RedirectToAction("Index", "Admin");
        }

        // GET: Unassigned Tasks
        public ActionResult AssignedTasks()
        {
            var assignedTasks = taskRepository.GetAssignedTasks(); // Retrieve tasks assigned to users
            return View(assignedTasks);
        }

        //public ActionResult AssignTask(int taskId, int userId)
        //{
        //    // Call the repository method to assign the task
        //    taskRepository.AssignTaskToUser(taskId, userId);

        //    // Redirect to the index page after assigning the task
        //    return RedirectToAction("TaskOverview");
        //}

        //public ActionResult AssignTask(int taskId, int assignedUserId)
        //{
        //    // Assign the task to the user
        //    AssignTask(taskId, assignedUserId);

        //    // Redirect to the unassigned tasks view (or wherever appropriate)
        //    return RedirectToAction("UnassignedTasks");
        //}

        //public ActionResult AssignTask(int taskId, int assignedUserId)
        //{
        //    // Assign the task to the user
        //    AssignTask(taskId, assignedUserId);

        //    // Redirect to the unassigned tasks view (or wherever appropriate)
        //    return RedirectToAction("UnassignedTasks");
        //}

        //public ActionResult AssignTask(int taskId, string userId)
        //{
        //    // Step 1: Assign the task to the user
        //    var result = taskRepository.AssignTaskToUser(taskId, userId);

        //    if (result)
        //    {
        //        // Step 2: Remove the user from the new employees list
        //        userRepository.RemoveUserFromNewEmployees(userId); // Assuming you have a method for this

        //        // Redirect to the TaskOverview page or wherever you need
        //        return RedirectToAction("TaskOverview");
        //    }
        //    else
        //    {
        //        // Handle failure to assign the task
        //        ModelState.AddModelError("", "Unable to assign task.");
        //        return View("Error");
        //    }
        //}


        public ActionResult AssignTask(int taskId, string userId, DateTime deadline)
        {
             //Step 1: Assign the task to the user
            //var taskAssigned = taskRepository.AssignTaskToUser(taskId, userId);
            taskRepository.AssignTaskToUser(taskId, userId, deadline);

            // Step 2: Remove the user from the new employees list if the task assignment was successful
           
            
                userRepository.RemoveUserFromNewEmployees(userId);
            

            // Redirect to the TaskOverview page after updating
            //return RedirectToAction("TaskOverview");
            return RedirectToAction("Index", "Admin");

        }

        //public ActionResult AssignTask(int taskId, string userId)
        //{
        //    // Step 1: Check if the userId can be converted to an integer (if required)
        //    int userIdInt;
        //    if (!int.TryParse(userId, out userIdInt))
        //    {
        //        // Handle the error if the conversion fails, e.g., log it or return an error view
        //        ModelState.AddModelError("", "Invalid User ID format.");
        //        return RedirectToAction("Index", "Admin"); // Or another appropriate action
        //    }

        //    // Step 2: Assign the task to the user
        //    // Adjust the method call according to your method's definition
        //    try
        //    {
        //        // Step 2: Assign the task to the user
        //        taskRepository.AssignTaskToUser(taskId, userIdInt); // Assuming userId is now an int

        //        // Step 3: Remove the user from the new employees list after assignment
        //        userRepository.RemoveUserFromNewEmployees(userId);
        //    }
        //    catch (Exception )
        //    {
        //        //Logger.Error(ex, "An error occurred while assigning the task.");
        //        // Log the exception (ex) for debugging
        //        ModelState.AddModelError("", "An error occurred while assigning the task.");
        //        return RedirectToAction("Index", "Admin"); // Redirect on error
        //    }

        //    // Redirect to the TaskOverview page after updating
        //    return RedirectToAction("Index", "Admin");

            
        //}



        public ActionResult TaskOverview()
            {
                var unassignedTasks = taskRepository.GetUnassignedTasks();
                var assignedTasks = taskRepository.GetAssignedTasks();

                var viewModel = new TaskOverviewViewModel
                {
                    UnassignedTasks = unassignedTasks,
                    AssignedTasks = assignedTasks
                };
            //string userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;

            //ViewBag.selectedUserId = userId; // Pass it to the view

                return View(viewModel);
            }


        public ActionResult Index()
        {
            var tasks = taskRepository.GetAllTasks();
            return View(tasks);
        }

        public ActionResult CreateTask()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTask(Task task)
        {
            if (ModelState.IsValid)
            {
                task.AssignedUserId = null; // No user assigned by default

                taskRepository.AddTask(task);
                return RedirectToAction("Index");
            }
            return View(task);
        }

        public ActionResult EditTask(int id)
        {
            Task task = taskRepository.GetAllTasks().Find(t => t.TaskId == id);
            if (task == null)
            {
                return HttpNotFound(); // Return 404 if task not found
            }
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTask(Task task)
        {
            if (ModelState.IsValid)
            {
                taskRepository.UpdateTask(task);
                return RedirectToAction("Index", "Admin");
            }
            return View(task);
        }

        public ActionResult DeleteTask(int id)
        {
            var task = taskRepository.GetAllTasks().Find(t => t.TaskId == id);
            if (task == null)
            {
                return HttpNotFound("Task not found.");
            }

            return View(task);
        }

        [HttpPost, ActionName("DeleteTask")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTaskConfirmed(int id)
        {
            taskRepository.DeleteTask(id);
            return RedirectToAction("Index");
        }

        public ActionResult AssignUserTask(int id)
        {
            //if (id == null || id <= 0)
            //{
            //    return HttpNotFound(); // Or handle it appropriately
            //}

            var task = taskRepository.GetTaskById(id); // Retrieve the task by ID
            if (task == null)
            {
                return HttpNotFound();
            }
            var users = taskRepository.GetAllUsers(); // Assume this method retrieves all users
            var designers = taskRepository.GetDesigners(); // Get designers

            //var userSelectList = users.Select(u => new SelectListItem
            //{
            //    Value = u.Id.ToString(),
            //    Text = u.Username // or any other display property
            //});
            //var designerSelectList = designers.Select(d => new SelectListItem
            //{
            //    Value = d.Id.ToString(),
            //    Text = d.Username // Assuming Username is a property on your user model
            //});
            //var model = new AssignTaskViewModel
            //{
            //    Task = task,
            //    Users = userSelectList,
            //    //Designers = designerSelectList, // Set the designers list
            //    Roles = new List<string> { "Frontend", "Backend", "Designer" } // Populate roles
            //};

            //return View(model);
            var model = new AssignTaskViewModel
            {
                Task = task, // Ensure this is assigned
                Users = users.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Username
                }).ToList(),
                Designers = designers.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Username
                }).ToList(),
                Roles = new List<string> { "Frontend", "Backend", "Designer" }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignUserTask(AssignTaskViewModel model)
        {
            if (model == null || model.Task == null)
            {
                ModelState.AddModelError("", "Task or model cannot be null.");
                return View(model); // Return to the view with the model for user to correct input
            }
            if (ModelState.IsValid)
            {
                taskRepository.AssignUserTask(model.Task.TaskId,model.SelectedUserId, model.Deadline);
                return RedirectToAction("Index");
            }

            var designers = taskRepository.GetDesigners();
            model.Users = designers.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Username // Ensure users are repopulated
            }).ToList();
            // If model is not valid, re-populate the roles and return the view
            model.Roles = new List<string> { "Frontend", "Backend", "Designer" };
            return View(model);
        }

        [HttpGet]
        public JsonResult GetUsers(string role)
        {
            var users = taskRepository.GetUsersByRole(role);
            //return Json(users.Select(u => new
            //{
            //    Id = u.Id,
            //    Username = u.Username
            //}), JsonRequestBehavior.AllowGet);
            var userSelectList = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Username
            });

            return Json(userSelectList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUsersByRole(string role)
        {
            // Check if the role parameter is received correctly
            if (string.IsNullOrEmpty(role))
            {
                return Json(new List<SelectListItem>(), JsonRequestBehavior.AllowGet);
            }

            var users = taskRepository.GetUsersByRole(role); // Make sure this method is implemented correctly

            // Transform the user list into SelectListItem
            var userSelectList = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                //Text = u.Username // Ensure this property is available in your user model
                Text = u.FirstName + " " + u.LastName
            }).ToList();

            return Json(userSelectList, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult AssignTask(int taskId, int userId)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Open();
        //        using (var cmd = new SqlCommand("UPDATE Tasks SET AssignedUserId = @UserId WHERE TaskId = @TaskId", con))
        //        {
        //            cmd.Parameters.AddWithValue("@UserId", userId);
        //            cmd.Parameters.AddWithValue("@TaskId", taskId);
        //            cmd.ExecuteNonQuery();
        //        }
        //    }

        //    // Redirect to the index page after assigning the task
        //    return RedirectToAction("Index");
        //}


        //public JsonResult GetUsers(string role)
        //{
        //    var users = taskRepository.GetUsersByRole(role);
        //    return Json(users, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult AssignUserTask(int taskId)
        //{
        //    ViewBag.TaskId = taskId;
        //    ViewBag.Roles = new SelectList(new List<string> { "Frontend", "Backend", "Designer" });
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult AssignUserTask(int taskId, int userId, DateTime deadline)
        //{
        //    // Call the repository method to assign the task
        //    userRepository.AssignTaskToUser(taskId, userId, deadline);
        //    return RedirectToAction("TaskList"); // Redirect to the task list or relevant page
        //}
        //public ActionResult AssignTaskToUser(int userId, int taskId, DateTime deadline)
        //{
        //    userRepository.AssignTaskToUser(userId, taskId, deadline);
        //    return RedirectToAction("TaskList"); // Or the appropriate action after assigning
        //}

        [HttpPost]
        public ActionResult AssignTaskToUser(int taskId, string userId, DateTime deadline)
        {
            try
            {
                taskRepository.AssignTaskToUser(taskId, userId, deadline);
                TempData["Message"] = "Task assigned successfully!";
            }
            catch (Exception )
            {
                TempData["Error"] = "An error occurred while assigning the task.";
                // Log error if necessary
            }

            return RedirectToAction("UnassignedTasks");
        }

        //public ActionResult AssignTaskToNewEmployee()
        //{
        //    // Retrieve all unassigned tasks and new employees
        //    var unassignedTasks = taskRepository.GetUnassignedTasks(); // Method to get unassigned tasks
        //    var newEmployees = userRepository.GetNewEmployees(); // Method to get new employees

        //    ViewBag.UnassignedTasks = unassignedTasks;
        //    ViewBag.NewEmployees = newEmployees;

        //    return View();
        //}

        //// POST: Admin/AssignTaskToNewEmployee
        //[HttpPost]
        //public ActionResult AssignTaskToNewEmployee(int userId, int taskId)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Create a new UserTask entry
        //        var userTask = new UserTask
        //        {
        //            UserId = userId,
        //            TaskId = taskId,
        //            Deadline = DateTime.Now.AddDays(7) // Example deadline, adjust as needed
        //        };

        //        // Add to the UserTasks through the TaskRepository
        //        taskRepository.AssignTaskToUser(userTask);

        //        // Optionally, remove the user from the new employees list if needed
        //        var user = userRepository.GetUserById(userId);
        //        if (user != null)
        //        {
        //            user.IsNewEmployee = false; // Update the user's status
        //            userRepository.UpdateUser(user); // Update user status in the repository
        //        }

        //        // Redirect to a success page or the same page with a success message
        //        return RedirectToAction("AssignTaskToNewEmployee"); // Redirect back to the view
        //    }

        //    // If we got this far, something failed, redisplay the form
        //    return View();
        //}

        //public ActionResult AssignTask(int id)
        //{
        //    var task = taskRepository.GetTaskById(id);
        //    var users = userRepository.GetAllUsers();

        //    var model = new AssignTaskViewModel
        //    {
        //        Task = task,
        //        Users = users
        //    };

        //    return View(model);
        //}

        //// POST: Admin/AssignTask
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AssignTask(AssignTaskViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        taskRepository.AssignTaskToUser(model.TaskId, model.SelectedUserId);
        //        return RedirectToAction("Index"); // Redirect to your task list
        //    }

        //    // If validation fails, retrieve users again for the view
        //    model.Users = userRepository.GetAllUsers();
        //    return View(model);
        //}
        //public ActionResult AssignTask(int id)
        //{
        //    var task = taskRepository.GetTaskById(id);
        //    if (task == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var viewModel = new AssignTaskViewModel
        //    {
        //        Task = task,
        //        Users = userRepository.GetAllUsers() // Assuming you have a method to get users
        //    };

        //    return View(viewModel);
        //}
        //public ActionResult AssignTask(int taskId)
        //{
        //    // Fetch users to assign the task to
        //    var users = userRepository.GetAllUsers(); // Replace with your actual data fetching logic
        //    ViewBag.Users = users ?? new List<User>();

        //    // Fetch the task details for the view model
        //    var task = taskRepository.GetTaskById(taskId);
        //    if (task == null)
        //    {
        //        return HttpNotFound(); // Handle the case where the task is not found
        //    }

        //    var viewModel = new AssignTaskViewModel
        //    {
        //        TaskId = task.Id,
        //        Task = task,
        //        Users = ViewBag.Users.ToList() // Populate users
        //    };

        //    return View(viewModel);
        //}

        // POST: Admin/AssignTask
        //[HttpPost]
        //public ActionResult AssignTask(AssignTaskViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var task = taskRepository.GetTaskById(model.TaskId);
        //        if (task != null)
        //        {
        //            task.AssignedUserId = model.SelectedUserId; // Assign the selected user
        //            taskRepository.UpdateTask(task); // Save changes to the database
        //            return RedirectToAction("Index"); // Redirect to your desired action
        //        }
        //        ModelState.AddModelError("", "Task not found.");
        //    }

        //    // If we get here, something went wrong, re-populate the users
        //    model.Users = userRepository.GetAllUsers().ToList(); // Re-fetch users for the view
        //    return View(model);
        //}
        //[HttpGet]
        //public ActionResult AssignUserTask(int? taskId)
        //{
        //    if (!taskId.HasValue)
        //    {
        //        return HttpNotFound(); // Handle the case where taskId is not provided
        //    }

        //    var users = userRepository.GetAllUsers();
        //    ViewBag.Users = users ?? new List<User>();

        //    var task = taskRepository.GetTaskById(taskId.Value); // Use .Value to access the int
        //    if (task == null)
        //    {
        //        return HttpNotFound(); // Handle the case where the task is not found
        //    }

        //    var viewModel = new AssignTaskViewModel
        //    {
        //        Id = task.Id,
        //        TaskId = task.Id,
        //        Task = task,
        //        Users = users.ToList()
        //    };

        //    return View(viewModel);
        //}
        //// POST: Admin/AssignTask
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AssignUserTask(AssignTaskViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        taskRepository.AssignTaskToUser(model.TaskId, model.SelectedUserId);
        //        return RedirectToAction("Index"); // Redirect to the task list or assigned tasks view
        //    }

        //    // If validation fails, repopulate the users for the view
        //    model.Users = userRepository.GetAllUsers().ToList();
        //    return View(model);
        //}

        //[HttpGet]
        //public ActionResult AssignUserTask(int? taskId)
        //{
        //    if (!taskId.HasValue)
        //    {
        //        return HttpNotFound(); // Handle the case where taskId is not provided
        //    }

        //    var users = userRepository.GetAllUsers();
        //    ViewBag.Users = users ?? new List<User>();

        //    var task = taskRepository.GetTaskById(taskId.Value);
        //    if (task == null)
        //    {
        //        return HttpNotFound(); // Handle the case where the task is not found
        //    }

        //    var viewModel = new AssignTaskViewModel
        //    {
        //        Id = task.Id,
        //        TaskId = task.Id,
        //        Task = task,
        //        Users = users.ToList()
        //    };

        //    return View(viewModel);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AssignUserTask(AssignTaskViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Assign the task to the user
        //        taskRepository.AssignTaskToUser(model.TaskId, model.SelectedUserId);

        //        // Optionally, you could redirect to the AssignedTasks view instead of Index
        //        return RedirectToAction("AssignedTasks", "Admin");
        //    }

        //    // If validation fails, repopulate the users for the view
        //    model.Users = userRepository.GetAllUsers().ToList();
        //    return View(model);
        //}   
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AssignTask(AssignTaskViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Assign the task to the selected user
        //        taskRepository.AssignTaskToUser(model.TaskId, model.SelectedUserId);

        //        // Get the task and check if it has any assignments
        //        Task taskToAssign = taskRepository.GetTaskById(model.TaskId);
        //        if (taskToAssign != null)
        //        {
        //            // Remove the user from new users
        //            User user = userRepository.GetUserById(model.SelectedUserId);
        //            if (user != null)
        //            {
        //                // Update tasks to set AssignedUserId to NULL before deleting user
        //                //taskToAssign.AssignedUserId = null; // or assign to another user
        //                //taskRepository.UpdateTask(taskToAssign); // Save changes to the task

        //                // Now you can safely delete the user
        //                userRepository.RemoveUser(user.Id);
        //            }
        //        }

        //        // Optionally remove the task from the unassigned tasks
        //        taskRepository.RemoveTask(model.TaskId); // Adjust this logic based on your needs

        //        // Redirect to an appropriate page after assigning the task
        //        return RedirectToAction("Index"); // Adjust as necessary
        //    }

        //    // Handle model state errors
        //    return View(model);
        //}



        //public ActionResult AssignTask(int taskId)
        //{
        //    var users = userRepository.GetAllUsers();
        //    var task = taskRepository.GetTaskById(taskId);

        //    if (task == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var viewModel = new AssignTaskViewModel
        //    {
        //        TaskId = task.Id,
        //        Task = task,
        //        Users = users.ToList()
        //    };

        //    return View(viewModel);
        //}

        // POST: Admin/AssignTask
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AssignUserTask(AssignTaskViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        taskRepository.AssignTaskToUser(model.TaskId, model.SelectedUserId);
        //        return RedirectToAction("Index"); // Redirect to the task list or assigned tasks view
        //    }

        //    // If validation fails, repopulate the users for the view
        //    model.Users = userRepository.GetAllUsers().ToList();
        //    return View(model);
        //}

        //[HttpGet]
        //public ActionResult AssignUserTask(int taskId)
        //{
        //    // Check if taskId is valid
        //    if (taskId <= 0)
        //    {
        //        return HttpNotFound();
        //    }

        //    var task = taskRepository.GetTaskById(taskId);
        //    if (task == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var users = userRepository.GetAllUsers().ToList();

        //    var viewModel = new AssignTaskViewModel
        //    {
        //        TaskId = task.Id,
        //        Task = task,
        //        Users = users
        //    };

        //    return View(viewModel);
        //}

        //[HttpPost]
        //public ActionResult AssignUserTask(AssignTaskViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var taskToAssign = taskRepository.GetTaskById(model.TaskId);

        //        if (taskToAssign != null)
        //        {
        //            taskToAssign.AssignedUserId = model.SelectedUserId;
        //            taskRepository.UpdateTask(taskToAssign);

        //            return RedirectToAction("AssignedTasks");
        //        }

        //        ModelState.AddModelError("", "Task not found.");
        //    }

        //    model.Users = userRepository.GetAllUsers().ToList();
        //    return View(model);
        //}
    }

}

