using System.ComponentModel.DataAnnotations;

namespace Web_Doan_2023.Models
{
    public class BillOfSale
    {
        public int Id { get; set; }
        public string? code { get; set; }
        public string Address { get; set; }
        [Phone]
        public string phone { get; set; }
        public decimal? sumQuantity { get; set; }
        public decimal? sumPrice { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public DateTime? deleteDate { get; set; }
        public string UsernameCreate { get; set; }
        public string? UsernameUpdate { get; set;}
        public string? UsernameDelete { get; set; }
        public string? StatusBill { get; set; }// trạng thái đơn hàng( đang xác nhận, đang lấy hàng, đang đợi vận chuyển, đang chuyển hàng, chờ thanh toán, đả thanh toán, hoành thành đơn)
        public bool StatusCode { get; set; }//trạng thái hoàn thành, thất bại
        public bool IsDelete { get; set; }

    }
}
