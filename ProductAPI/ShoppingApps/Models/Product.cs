namespace ShoppingApps.Models
{
   
    public class Product
    {
        public int id { get; set; }
        public string productname { get; set; }
        public string productdesc { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public string image { get; set; }

    }
}
