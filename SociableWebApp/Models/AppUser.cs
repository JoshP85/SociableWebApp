using System.ComponentModel.DataAnnotations;

namespace SociableWebApp.Models
{
    public class AppUser
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        public string PhoneNumber { get; set; }

        public string? Location { get; set; }

        public DateTime AccCreatedDate { get; set; }

        public DateTime AccUpdatedDate { get; set; }

    }
}
