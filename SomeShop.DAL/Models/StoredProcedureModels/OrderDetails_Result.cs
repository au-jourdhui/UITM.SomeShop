using System;

namespace SomeShop.DAL.Models.StoredProcedureModels
{
    public class OrderDetails_Result
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string DeliveryType { get; set; }
        public string PaymentType { get; set; }
        public double Total { get; set; }
    }
}
