using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace signedup.Models
{
    public class Task
    {
        public int TaskId { get; set; }             // Unique identifier for the task
        public string Title { get; set; }         // Corrected from TaskNmae to TaskName
        public string Description { get; set; }      // Description of the task
        public int? AssignedUserId { get; set; }     // Nullable to indicate no user assigned yet
        public DateTime? Deadline { get; set; }      // Uncomment if you want to include deadlines
        public bool IsCompleted { get; set; }        // Uncomment if you want to track completion status
        public string Status => IsCompleted ? "Completed" : "Pending";
    }



   
        public class AssignTaskViewModel
        {
            public Task Task { get; set; }
            public IEnumerable<SelectListItem> Users { get; set; } // Add this property
            public IEnumerable<SelectListItem> Designers { get; set; }
            public List<string> Roles { get; set; } // e.g., Frontend, Backend, Designer
            public string SelectedRole { get; set; } // The selected role from the dropdown
            public List<User> FilteredUsers { get; set; } // Users filtered by role
            public int SelectedUserId { get; set; } // The user selected to assign the task
            public DateTime Deadline { get; set; } // Deadline for the task
        }
    

    public class UserViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }

    public class TaskOverviewViewModel
    {
        public List<Task> UnassignedTasks { get; set; }
        public List<Task> AssignedTasks { get; set; }
    }

}
