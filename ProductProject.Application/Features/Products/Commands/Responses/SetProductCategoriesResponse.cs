namespace ProductProject.Application.Features.Products.Commands.Responses
{
    public class SetProductCategoriesResponse
    {
        public HashSet<int> AddedCategories { get; set; } = new();
        public HashSet<int> RemovedCategories { get; set; } = new();
    }
}