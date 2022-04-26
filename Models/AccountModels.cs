using System.ComponentModel.DataAnnotations;

namespace DigitalProject.Models {

    public class LoginModel {
        [Required]
        public string? OrediggerId { get; set; }
        [Required]
        public string? Password { get; set; }
    }

    public class RegistrationModel {
        [Required]
        public string? FistName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        public string? OrediggerId { get; set; }
        public int OfficeId { get; set; }
        [Required]
        public string? Password { get; set; }
    }

    public class LoggedInModel {
        public string? Token { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OrediggerId { get; set; }
    }

}