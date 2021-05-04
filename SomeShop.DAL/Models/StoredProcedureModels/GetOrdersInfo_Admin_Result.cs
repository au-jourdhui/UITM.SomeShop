using System;

namespace SomeShop.DAL.Models.StoredProcedureModels
{
    public class GetOrdersInfo_Admin_Result
    {
        public int OrderId { get; set; }
        public string Customer { get; set; }
        public int ItemsCount { get; set; }
        public double TotalSum { get; set; }
        public string OrderCity { get; set; }
        public string Status { get; set; }
        public string OrderDeliveryType { get; set; }
        public string OrderPaymentType { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsDeleted { get; set; }
    }
}
