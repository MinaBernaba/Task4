namespace ProductProject.Application.Features.Categories.Queries.Responses
{
    public class GetAllCategoriesResponse
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}