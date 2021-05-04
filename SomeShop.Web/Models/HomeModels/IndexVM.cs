using SomeShop.DAL.Models;
using SomeShop.DAL.Models.StoredProcedureModels;
using SomeShop.Web.Extensions;
using System.Collections.Generic;

namespace SomeShop.Web.Models.HomeModels
{
    public class IndexVM
    {
        public IEnumerable<Brand> Brands { get; set; }
        public IEnumerable<Brand> RandomBrands => Brands?.Random(2);
        public IEnumerable<ProductItemInfo_Result> NewItems { get; set; }
        public IEnumerable<ProductItemInfo_Result> SaleItems { get; set; }

        public IndexVM()
        {
            Brands = new List<Brand>();
            NewItems = new List<ProductItemInfo_Result>();
            SaleItems = new List<ProductItemInfo_Result>();
        }
    }
}
