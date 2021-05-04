namespace SomeShop.DAL.Models
{
    public class OptionalParameterProduct : BaseEntity
    {
        public int ItemId { get; set; }
        public int OptionalParameterId { get; set; }
        public string Value { get; set; }
    }
}
