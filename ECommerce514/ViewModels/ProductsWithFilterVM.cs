namespace ECommerce514.ViewModels
{
    public class ProductsWithFilterVM
    {
        public string? ProductName { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public int CategoryId { get; set; }
    }
}
