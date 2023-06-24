using AutoMapper.Configuration.Conventions;

namespace Web_Doan_2023.Models
{
    public class Producer// nhà sản xuất
    {
        public int Id { get; set; }
        public string? codeProduce { get; set; }
        public string? nameProduce { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int? idWards { get; set; }
        public int? idCity { get; set; }
        public int? idDistrict { get; set; }
        public string? Location { get; set; }
        public bool Status { get; set; }
        public string? logoProducer { get; set; }
        public List<Product>? Products { get; set; }



    }
}
