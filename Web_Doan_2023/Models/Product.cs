namespace Web_Doan_2023.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string codeProduct { get; set; }// Mã sản phẩm
        public string nameProduct { get; set; }
        public string Description { get; set; } // sự miêu tả
        public int idCategory { get; set; }
        public string idProducer { get; set; }// nhà sản xuất
        public decimal price { get; set; }
        public bool Status { get; set; }
        public bool IsDelete { get; set; }

    }
}
