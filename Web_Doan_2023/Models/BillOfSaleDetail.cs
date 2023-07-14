namespace Web_Doan_2023.Models
{
    public class BillOfSaleDetail
    {
        public int Id { get; set; }
        public int IdBill { get; set; }
        public int IdProduct { get; set; }
        public string ShipmentCode { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }
    }
}
