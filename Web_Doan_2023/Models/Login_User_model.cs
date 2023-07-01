using System.ComponentModel.DataAnnotations;

namespace Web_Doan_2023.Models
{
    public class Login_User_model
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
