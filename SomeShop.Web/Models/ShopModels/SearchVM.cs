using System.Collections.Generic;

namespace SomeShop.Web.Models.ShopModels
{
    public class SearchVM
    {
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public IEnumerable<int> Categories { get; set; }

        public SearchVM()
        {
            Categories = new List<int>();
        }
    }
}
