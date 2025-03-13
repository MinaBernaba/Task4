namespace ProductProjrect.Data.Entities
{
    public class ProductCategory
    {
        public int ProductCategoryId { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
    }
}
