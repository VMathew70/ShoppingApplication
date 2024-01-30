namespace ProductAPI.Models
{
    public class OrderHeader
    {

        public int Id { get; set; }
        public string? customername { get; set; }
        public string? email { get; set; }
        public string? ordernumber { get; set; }

    }
    public class OrderDetail
    {

        public int Id { get; set; }
        public string? ordernumber { get; set; }

        public int productid { get; set; }
        public int orderquantity { get; set; }
        public decimal price { get; set; }  

    }

    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string? Image { get; set; }

        public string? customername { get; set; }
        public string? email { get; set; }
        public string? ordernumber { get; set; }


    }
}
