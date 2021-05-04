namespace SomeShop.DAL.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }
}
