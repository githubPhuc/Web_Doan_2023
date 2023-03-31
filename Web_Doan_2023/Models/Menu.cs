namespace Web_Doan_2023.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public string Menu_Name { get; set; }
        public int ParentId { get; set; }
        public int PageId { get; set; }//
        public int oder_no { get; set; }
        public int Icon { get; set; }
        public string IdUsercreate { get; set; }
        public string IdUserupdate { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public List<Page> Pages { get; set; }
    }
}
