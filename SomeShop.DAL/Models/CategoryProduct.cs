namespace SomeShop.DAL.Models
{
    public class CategoryProduct : BaseEntity
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
    }
}
