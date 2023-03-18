namespace Web_Doan_2023.Models.Address
{
    public class City
    {
        public int Id { get; set; }
        public string NameCity { get; set; }
        public bool Status { get; set; }
        public List<District> _Districts { get; set; }
        public List<Wards> _Wards { get; set; }
    }
}
