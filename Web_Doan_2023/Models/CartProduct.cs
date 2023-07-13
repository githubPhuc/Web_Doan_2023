namespace Web_Doan_2023.Models
{
    public class CartProduct
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int? saleID { get; set; }
        public decimal? salePrice { get; set; }
        public string ShipmentCode { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Username { get; set; }
        public bool Status { get; set; }
        public List<Product>? Products { get; set;}

    }
}
