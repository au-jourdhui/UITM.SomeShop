namespace SomeShop.DAL.Models
{
    public class ContactInfo : BaseEntity
    {
        public string City { get; set; }
        public string Address { get; set; }
        public string Phones { get; set; }
        public string TimeTable { get; set; }
        public string GoogleUrl { get; set; }
    }
}
