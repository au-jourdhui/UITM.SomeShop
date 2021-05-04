namespace SomeShop.DAL.Models
{
    public class Cart : BaseEntity
    {
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
