namespace Web_Doan_2023.Models
{
    public class Page
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Controller { get; set; }
        public string? Action { get; set; }
        public string? IdUsercreate { get; set; }
        public string? IdUserupdate { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
