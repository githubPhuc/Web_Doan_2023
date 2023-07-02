using System.ComponentModel.DataAnnotations;

namespace Web_Doan_2023.Models
{
    public class RegisterUserModel
    {
        public string FullName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Phone]
        [Required(ErrorMessage = "Phone number is required")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "City is required")]
        public int? Cyti { get; set; }
        [Required(ErrorMessage = "Wards is required")]
        public int? Wards { get; set; }
        [Required(ErrorMessage = "Wards is required")]
        public int? District { get; set; }
        [Required(ErrorMessage = "District is required")]
        public string? ShippingAddress { get; set; }
    }
}
