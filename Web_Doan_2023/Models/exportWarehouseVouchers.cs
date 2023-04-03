namespace Web_Doan_2023.Models
{
    public class exportWarehouseVouchers
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public string codeExportWarehouseVouchers { get; set; }
        public decimal priceExportWarehouseVouchers { get; set; }// tổng tiền
        public DateTime dateCreate { get; set; }
        public DateTime dateUpdate { get; set; }
        public DateTime dateDelete { get; set; }
        public string userCreate { get; set; }
        public string userUpdate { get; set; }
        public string userDelete { get; set; }
        public bool agree { get; set; }// phê duyệt
        public int Status { get; set; }
    }
}
