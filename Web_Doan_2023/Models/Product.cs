namespace Web_Doan_2023.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? codeProduct { get; set; }// Mã sản phẩm
        public string? nameProduct { get; set; }
        public string? Description { get; set; } // sự miêu tả
        public int idCategory { get; set; }
        public string? nameCategory { get; set; }
        public int idProducer { get; set; }// nhà sản xuất
        public decimal? price { get; set; }
        public string? RamProduct { get; set; }
        public string? SSDProduct { get; set; }
        public string? CPUProduct { get; set; }
        public string? MainProduct { get; set; }
        public string? DisplayProduct { get; set; }
        public string? ColorProduct { get; set; }
        public string? AccessoriesIncluded { get; set; }
        public bool? Status { get; set; }
        public int idSale { get; set; }
        public bool? IsDelete { get; set; }
        public List<Images>? images { get; set; }

    }
}
