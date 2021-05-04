namespace SomeShop.DAL.Models.StoredProcedureModels
{
    public class ProductsInfo_Admin_Result
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string Categories { get; set; }
        public bool IsDeleted { get; set; }
    }
}
