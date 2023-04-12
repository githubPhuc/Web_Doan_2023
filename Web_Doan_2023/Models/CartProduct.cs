namespace Web_Doan_2023.Models
{
    public class CartProduct
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? userCreate { get; set; }
        public bool Status { get; set; }
        public string StatusMessage { get; set; }

    }
}
