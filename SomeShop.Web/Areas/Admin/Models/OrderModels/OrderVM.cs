using SomeShop.DAL.Models;
using SomeShop.DAL.Models.StoredProcedureModels;
using System.Collections.Generic;

namespace SomeShop.Web.Areas.Admin.Models.OrderModels
{
    public class OrderVM
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

        public int OrderDeliveryTypeId { get; set; }
        public int OrderPaymentTypeId { get; set; }
        public int OrderStatusId { get; set; }

        public double TotalSum { get; set; }

        public IEnumerable<OrderDeliveryType> OrderDeliveryTypes { get; set; }
        public IEnumerable<OrderPaymentType> OrderPaymentTypes { get; set; }
        public IEnumerable<OrderStatus> OrderStatuses { get; set; }

        public IEnumerable<OrderProductsInfo_Result> OrderProducts { get; set; }

        public OrderVM()
        {
            OrderDeliveryTypes = new List<OrderDeliveryType>();
            OrderPaymentTypes = new List<OrderPaymentType>();
            OrderStatuses = new List<OrderStatus>();
            OrderProducts = new List<OrderProductsInfo_Result>();
        }
    }
}
