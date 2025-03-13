namespace ProductProject.Application.Features.Categories.Queries.Responses
{
    public class GetCategoryDetailedInfoResponse
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CategoryHierarchy { get; set; } = null!;
    }
}