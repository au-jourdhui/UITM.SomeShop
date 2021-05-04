namespace SomeShop.DAL.Models
{
    public class CharacteristicProduct : BaseEntity
    {
        public int ProductId { get; set; }
        public int CharacteristicId { get; set; }
        public string Value { get; set; }
    }
}
