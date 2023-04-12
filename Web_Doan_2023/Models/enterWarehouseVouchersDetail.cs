using System;

namespace Web_Doan_2023.Models
{
    public class enterWarehouseVouchersDetail // chi tiết chwusng từ nhập
    {
        public int Id { get; set; }
        public string? codeProduct { get; set; }
        public decimal? price { get; set; }
        public int? quantity { get; set; }

    }
}
