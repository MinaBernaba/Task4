namespace ProductProject.Application.Features.Orders.Queries.Responses
{
    public class GetOrderInfoResponse
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public List<OrderDetailsInfoResponse> OrderDetailsInfos { get; set; } = new();

    }
    public class OrderDetailsInfoResponse
    {
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
