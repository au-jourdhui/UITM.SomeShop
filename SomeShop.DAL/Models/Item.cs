namespace SomeShop.DAL.Models
{
    public class Item : BaseEntity
    {
        public int ProductId { get; set; }
        public double Price { get; set; }
        public double? OldPrice { get; set; }
    }
}
