namespace Web_Doan_2023.Models
{
    public class Department//Phòng ban
    {
        public int Id { get; set; }
        public string? codeDepartment { get; set; }
        public string? nameDepartment { get; set; }
        public bool Status { get; set; }
        public List<User>? Users { get; set; }

    }
}
