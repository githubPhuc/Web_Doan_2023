namespace Web_Doan_2023.Models
{
    public class CartDetailProduct
    {
        public int Id { get; set; }
        public int IdCartProduct { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
