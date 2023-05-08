namespace Web_Doan_2023.Models
{
    public class productDepot
    {
        public int Id { get; set; }
        public int idProduct { get; set; }
        public int idDepot { get; set; }
        public int QuantityProduct { get; set; }
        public List<Product>? products { get; set; }
        

    }
}
