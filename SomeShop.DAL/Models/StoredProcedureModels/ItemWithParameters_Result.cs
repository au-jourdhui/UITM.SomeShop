namespace SomeShop.DAL.Models.StoredProcedureModels
{
    public class ItemWithParameters_Result
    {
        public int ItemId { get; set; }
        public double Price { get; set; }
        public double? OldPrice { get; set; }
        public string Parameters { get; set; }
    }
}
