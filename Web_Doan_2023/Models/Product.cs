namespace Web_Doan_2023.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? codeProduct { get; set; }// Mã sản phẩm
        public string? nameProduct { get; set; }
        public string? Description { get; set; } // sự miêu tả
        public int idCategory { get; set; }
        public int idProducer { get; set; }// nhà sản xuất
        public decimal? price { get; set; }
        public int RamProduct { get; set; }
        public int SSDProduct { get; set; }
        public int CPUProduct { get; set; }
        public string MainProduct { get; set; }
        public int DisplayProduct { get; set; }
        public int ColorProduct { get; set; }
        public string portConnection { get;set; }
        public int CardDisplay { get; set; }
        public string? AccessoriesIncluded { get; set; }// Phụ kiện đi kèm
        public bool Status { get; set; }
        public int? idSale { get; set; }
        public bool IsDelete { get; set; }
        public List<Images>? images { get; set; }

    }
}
