namespace SomeShop.DAL.Models.StoredProcedureModels
{
    public class CartItemsInfo_Result
    {
        public int ProductId { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
        public string Image { get; set; }
        public double Sum { get; set; }
    }
}
