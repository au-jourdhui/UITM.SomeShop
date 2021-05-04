namespace SomeShop.DAL.Models.StoredProcedureModels
{
    public class GetOptionalParametersForItem_Admin_Result
    {
        public int ItemId { get; set; }
        public int OptionalParameterId { get; set; }
        public int OptionalParameterProductId { get; set; }
        public string OptionalParameterName { get; set; }
        public string OptionalParameterValue { get; set; }
        public bool IsDeletedOptionalParameter { get; set; }
        public bool IsDeletedOptionalParameterProduct { get; set; }
    }
}
