using SomeShop.DAL.Models;
using SomeShop.DAL.Models.StoredProcedureModels;
using System.Collections.Generic;

namespace SomeShop.Web.Models.HomeModels
{
    public class AboutVM
    {
        public IEnumerable<Brand> Brands { get; set; }
        public ProductItemInfo_Result Product { get; set; }
    }
}
