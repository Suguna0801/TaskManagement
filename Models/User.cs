//using System;
//using System.ComponentModel.DataAnnotations;
//using System.Text.RegularExpressions;

//namespace signedup.Models
//{
//    public class User
//    {
//        public int Id { get; set; }

//        [Required(ErrorMessage = "First name is required.")]
//        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
//        [MinLength(3, ErrorMessage = "First Name must be at least 3 characters long.")]
//        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters.")]
//        public string FirstName { get; set; }

//        [Required(ErrorMessage = "Last name is required.")]
//        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
//        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters.")]
//        public string LastName { get; set; }

//        [Required(ErrorMessage = "Date of Birth is required.")]
//        [DataType(DataType.Date)]
//        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
//        public DateTime DateOfBirth { get; set; }

//        [Required(ErrorMessage = "Email is required.")]
//        [EmailAddress(ErrorMessage = "Invalid email format.")]
//        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email must be a Gmail address.")]
//        public string Email { get; set; }

//        [Required]
//        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits.")]
//        public string PhoneNumber { get; set; }

//        [Required(ErrorMessage = "Please select your gender.")]
//        public string Gender { get; set; }


//        [Required(ErrorMessage = "Address is required.")]
//        [StringLength(255)]
//        public string Address { get; set; }

//        [Required(ErrorMessage = "State is required.")]
//        [StringLength(50)]
//        public string State { get; set; }

//        [Required(ErrorMessage = "City is required.")]
//        [StringLength(50)]
//        public string City { get; set; }

//        [Required(ErrorMessage = "Username is required.")]
//        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username should contain only letters and numbers.")]
//        public string Username { get; set; }

//        [Required(ErrorMessage = "Password is required.")]
//        [DataType(DataType.Password)]
//        [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 8)]
//        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
//        ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one number, and one special character.")]
//        public string Password { get; set; }

//        [Required(ErrorMessage = "Confirm Password is required.")]
//        [DataType(DataType.Password)]
//        [Compare("Password", ErrorMessage = "Passwords do not match.")]
//        public string ConfirmPassword { get; set; }





//    }
//}

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace signedup.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [MinLength(3, ErrorMessage = "First Name must be at least 3 characters long.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email must be a Gmail address.")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please select your gender.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please select your role.")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(255)]
        public string Address { get; set; }

        // Updated State property to reference StateId
        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; } // Foreign key

        // Navigation property for State
      

        // Updated City property to reference CityId
        [Required(ErrorMessage = "City is required.")]
        public string City{ get; set; } // Foreign key

        // Navigation property for City

        

        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username should contain only letters and numbers.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}


