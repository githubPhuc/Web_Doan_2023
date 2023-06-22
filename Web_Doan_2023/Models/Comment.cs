namespace Web_Doan_2023.Models
{
    public class Comment
    {
        public int id { get; set; }
        public string? Content { get; set; }
        public int? TopLevel { get; set; }//1
        public int? BottomLevel { get; set; }//0
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set;}
    }
}
