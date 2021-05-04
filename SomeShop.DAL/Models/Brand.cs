namespace SomeShop.DAL.Models
{
    public class Brand : BaseEntity
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string Logo { get; set; }
        public string BackgroundImage { get; set; }
    }
}
