using SomeShop.DAL.Models;
using SomeShop.DAL.Models.StoredProcedureModels;
using System.Collections.Generic;

namespace SomeShop.Web.Models.AccountModels
{
    public class IndexVM
    {
        public User User { get; set; }
        public IEnumerable<ProductItemInfo_Result> Products { get; set; }
        public IEnumerable<OrderDetails_Result> OrderDetails { get; set; }
        public AccountTab Tab { get; set; }

        public string Error { get; set; }
        public bool HasError => !string.IsNullOrWhiteSpace(Error);
        public bool UpdatedUserInfo { get; set; }

        public IndexVM()
        {
            Products = new List<ProductItemInfo_Result>();
            OrderDetails = new List<OrderDetails_Result>();
        }
    }
}
