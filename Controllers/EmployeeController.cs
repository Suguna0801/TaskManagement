using System.Web.Mvc;
using signedup.Models; // Adjust the namespace based on your project structure
using System.Collections.Generic;
using signedup.Repositories;

namespace signedup.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly TaskRepository taskRepository; // Assuming you have a task repository

        public EmployeeController()
        {
            string connectionString = "Data Source=DESKTOP-ENRUGTD\\SQLEXPRESS;Initial Catalog=suguna;Integrated Security=True;";
            //userRepository = new UserRepository(connectionString);
            taskRepository = new TaskRepository(connectionString);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AssignedTasks()
        {
            string username = User.Identity.Name; // Get the username from the logged-in user
            var tasks = taskRepository.GetAssignedTasksForUser(username);
            return View(tasks); // Pass the list of tasks to the view
        }

        public ActionResult CompletedTasks()
        {
            string username = User.Identity.Name; // Get the username from the logged-in user
            var tasks = taskRepository.GetCompletedTasksForUser(username);
            return View(tasks); // Pass the list of tasks to the view
        }
    }
}
