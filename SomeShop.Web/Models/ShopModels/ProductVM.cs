using SomeShop.DAL.Models;
using SomeShop.DAL.Models.StoredProcedureModels;
using System.Collections.Generic;

namespace SomeShop.Web.Models.ShopModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<ProductItemInfo_Result> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<ProductImage> ProductImages { get; set; }
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<ItemWithParameters_Result> ItemsWithParameters { get; set; }
        public bool Favourite { get; set; }

        public ProductVM()
        {
            Products = new List<ProductItemInfo_Result>();
            Categories = new List<Category>();
            ProductImages = new List<ProductImage>();
            ItemsWithParameters = new List<ItemWithParameters_Result>();
        }
    }
}
