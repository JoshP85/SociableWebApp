namespace SociableWebApp.Models
{
    public class AppUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public DateTime AccCreatedDate { get; set; }
        public DateTime AccUpdatedDate { get; set; }

    }
}
