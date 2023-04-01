namespace Web_Doan_2023.Models
{
    public class saleProduct
    {
        public int Id { get; set; }
        public string nameSale { get; set;  }
        public string descriptionSale { get; set; }//miêu tả
        public float marth { get; set; }//tính toán giảm giá
        public bool Status { get; set; }

    }
}
