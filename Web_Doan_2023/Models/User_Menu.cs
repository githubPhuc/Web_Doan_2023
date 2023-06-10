namespace Web_Doan_2023.Models
{
    public class User_Menu
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int MenuId { get; set; }
        public string? IdUsercreate { get; set; }
        public string? IdUserupdate { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }

    }
}
