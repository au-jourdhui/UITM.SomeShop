using SomeShop.DAL.Models.StoredProcedureModels;
using System.Collections.Generic;

namespace SomeShop.Web.Models.ShopModels
{
    public class IndexVM
    {
        public IEnumerable<IEnumerable<ProductItemInfo_Result>> Products { get; set; }
        public IEnumerable<GetCategoryHierarchy_Result> CategoryHierarchy { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public double? SelectedMinPrice { get; set; }
        public double? SelectedMaxPrice { get; set; }
        public IEnumerable<int> Categories { get; set; }
        public string Search { get; set; }

        public IndexVM()
        {
            Products = new List<List<ProductItemInfo_Result>>();
            CategoryHierarchy = new List<GetCategoryHierarchy_Result>();
        }
    }
}
