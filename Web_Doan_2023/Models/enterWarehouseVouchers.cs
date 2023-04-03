namespace Web_Doan_2023.Models
{
    public class enterWarehouseVouchers //chứng từ nhập kho
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public string codeEnterWarehouseVouchers { get; set; }
        public decimal priceEnterWarehouseVouchers { get; set; }
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
