namespace SomeShop.DAL.Models
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }
        public int OrderStatus { get; set; }
        public int OrderDeliveryType { get; set; }
        public int OrderPaymentType { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
    }
}
