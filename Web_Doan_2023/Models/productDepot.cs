namespace Web_Doan_2023.Models
{
    public class productDepot
    {
        public int Id { get; set; }

        public int idProduct { get; set; } // Mã tt sản phẩm
        public int idDepot { get; set; }// Mã kho
        public int QuantityProduct { get; set; }// số lượng tồn kho
        public string ShipmentCode { get; set; }// mã lô hàng
        public decimal price { get; set; }// giá nhập
        public decimal priceSale { get; set; } //giá bán
        public bool status { get; set; }// Trạng thái 
        public DateTime DateCreate { get; set; }

    }
}
