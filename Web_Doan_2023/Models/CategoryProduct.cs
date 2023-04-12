namespace Web_Doan_2023.Models
{
    public class CategoryProduct
    {
        public int Id { get; set; } 
        public string? nameCategory { get; set; }
        

        public bool? Status { get; set; }
        public List<Product> Products { get; set; } 

    }
}
