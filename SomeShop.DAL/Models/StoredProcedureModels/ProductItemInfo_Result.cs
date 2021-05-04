using System;

namespace SomeShop.DAL.Models.StoredProcedureModels
{
    public class ProductItemInfo_Result
    {
        public int ProductId { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double? OldPrice { get; set; }
        public string Image { get; set; }
        public bool Favourite { get; set; }

        public int? Discount => OldPrice.HasValue ? (int?)Math.Floor(Price / OldPrice.Value * 100 - 100) : null;
    }
}
