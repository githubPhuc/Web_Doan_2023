using Microsoft.AspNetCore.Identity;

namespace Web_Doan_2023.Models
{
    public class User: IdentityUser
    {
        public string? Fullname { get; set; }
        public string? AccoutType { get; set; }
        public int ? City { get; set; }//thanh pho
        public int? Wards { get; set; }//phuong xa
        public int? District { get; set; }

        public string? ShippingAddress { get; set; }
        public int idDepartment { get; set; }
        public bool? IsLocked { get; set; }
    }
}
