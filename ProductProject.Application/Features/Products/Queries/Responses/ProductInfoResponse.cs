namespace ProductProject.Application.Features.Products.Queries.Responses
{
    public class ProductInfoResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public HashSet<string> Categories { get; set; } = new();
    }
}