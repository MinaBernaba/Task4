namespace ProductProjrect.Data.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public int? ParentCategoryId { get; set; }

        public virtual Category? ParentCategory { get; set; } // Self-referencing relationship to parent category
        public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>(); // Child categories
        public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    }
}
