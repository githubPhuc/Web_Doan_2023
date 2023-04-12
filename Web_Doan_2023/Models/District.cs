namespace Web_Doan_2023.Models
{
    public class District
    {
        public int Id { get; set; }
        public int? IdDistrict { get; set; }
        public string? NameDistrict { get; set; }
        public bool Status { get; set; }
        public List<Wards> _Wards { get; set; }
    }
}
