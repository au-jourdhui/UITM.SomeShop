namespace SomeShop.DAL.Models
{
    public class OrderProduct : BaseEntity
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
