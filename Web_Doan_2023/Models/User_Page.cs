namespace Web_Doan_2023.Models
{
    public class User_Page
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string PageId { get; set; }
        public string IdUsercreate { get; set; }
        public string IdUserupdate { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public List<User_Page> Pages { get; set; }

    }
}
