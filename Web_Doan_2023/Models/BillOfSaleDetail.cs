namespace Web_Doan_2023.Models
{
    public class BillOfSaleDetail
    {
        public int Id { get; set; }
        public int IdBill { get; set; }
        public int Idproduct { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }
    }
}
