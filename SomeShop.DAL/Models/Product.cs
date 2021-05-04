namespace SomeShop.DAL.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int BrandId{ get; set; }
    }
}
