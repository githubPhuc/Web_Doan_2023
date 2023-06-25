namespace Web_Doan_2023.Models
{
    public class EditAccountModel
    {
        public string Id { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public string? Fullname { get; set; }
        public string? images { get; set; }
        public int Cyti { get; set; }//thanh pho
        public int Wards { get; set; }//phuong xa
        public int District { get; set; }
        public string? ShippingAddress { get; set; }
        public int idDepartment { get; set; }
        public bool? IsLocked { get; set; }
    }
}
