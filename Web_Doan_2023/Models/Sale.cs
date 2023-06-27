namespace Web_Doan_2023.Models
{
    public class Sale
    {
            public int Id { get; set; }
            public string? nameSale { get; set;  }
            public string? descriptionSale { get; set; }//miêu tả
            public float marth { get; set; }//tính toán giảm giá
            public string Unit { get; set; }//Đơn vị tính
            public bool Status { get; set; }

    }
}
