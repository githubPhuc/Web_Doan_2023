namespace Web_Doan_2023.Models
{
    public class ImportBillDepotDetail
    {
        public int Id { get; set; }
        public int BillId { get; set;}
        public int idProduct { get; set; }
        public int Quantity { get;set; }
        public decimal price { get; set; }
    }
}
