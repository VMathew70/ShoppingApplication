namespace ShoppingApps.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string Image { get; set; }

        public string customername { get; set; }
        public string email { get; set; }
        public string ordernumber { get; set; }


        public CartItem()
        {

        }
        public CartItem(Product product )
        {
            this.ProductId = product.id;
            this.ProductName = product.productname;
            this.Quantity = 1;
            this.Price = product.price;
            this.Image = product.image;

        }
    }
}
