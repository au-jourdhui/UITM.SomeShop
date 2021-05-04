using SomeShop.DAL.Dao;
using SomeShop.DAL.Models;
using SomeShop.DAL.Models.StoredProcedureModels;
using Dapper;
using System.Collections.Generic;
using System.Data;

namespace SomeShop.DAL
{
    public class StoredProcedures : DataAccessObject
    {
        public StoredProcedures(IDbConnection connection) : base(connection) { }

        public IEnumerable<ProductItemInfo_Result> GetItemsInfo(int? count, bool? newItems = null, bool? saleItems = null, int? userId = null)
        {
            var obj = new
            {
                @count = count,
                @newItems = newItems,
                @saleItems = saleItems,
                @userId = userId
            };
            var result = Connection.Query<ProductItemInfo_Result>("dbo.GetItemsInfo", obj, commandType: CommandType.StoredProcedure);
            return result;
        }
        public IEnumerable<Category> CategoriesForProduct(int? productId = null)
        {
            var obj = new
            {
                @productId = productId
            };
            var result = Connection.Query<Category>("dbo.CategoriesForProduct", obj, commandType: CommandType.StoredProcedure);
            return result;
        }
        public IEnumerable<ProductImage> GetProductImages(int? productId = null)
        {
            var obj = new
            {
                @productId = productId
            };
            var result = Connection.Query<ProductImage>("dbo.GetProductImages", obj, commandType: CommandType.StoredProcedure);
            return result;
        }
        public IEnumerable<ItemWithParameters_Result> GetItemWithParameters(int productId)
        {
            var obj = new
            {
                @productId = productId
            };
            var result = Connection.Query<ItemWithParameters_Result>("dbo.GetItemWithParameters", obj, commandType: CommandType.StoredProcedure);
            return result;
        }
        public string MD5HashPassword(string password)
        {
            var result = Connection.ExecuteScalar<string>($"select dbo.MD5HashPassword('{password}')");
            return result;
        }
        public int GetUserRoleId(string role)
        {
            var result = Connection.ExecuteScalar<int>($"select dbo.GetUserRoleId(N'{role}')");
            return result;
        }
        public IEnumerable<GetCategoryHierarchy_Result> GetCategoryHierarchy(int? toLevelId = null)
        {
            var obj = new
            {
                @toLevelId = toLevelId
            };
            var result = Connection.Query<GetCategoryHierarchy_Result>("dbo.GetCategoryHierarchy", obj, commandType: CommandType.StoredProcedure);
            return result;
        }
        public IEnumerable<ProductItemInfo_Result> GetFavouriteProductInfo(int userId)
        {
            var obj = new
            {
                @userId = userId
            };
            var result = Connection.Query<ProductItemInfo_Result>("dbo.GetFavouriteProductInfo", obj, commandType: CommandType.StoredProcedure);
            return result;
        }
        public IEnumerable<ProductItemInfo_Result> SearchItemInfo(int? userId, int? categoryId = null, int? brandId = null, string search = null, double? minPrice = null, double? maxPrice = null)
        {
            var obj = new
            {
                @userId = userId,
                @categoryId = categoryId,
                @brandId = brandId,
                @search = search,
                @minPrice = minPrice,
                @maxPrice = maxPrice
            };
            var result = Connection.Query<ProductItemInfo_Result>("dbo.SearchItemInfo", obj, commandType: CommandType.StoredProcedure);
            return result;
        }
        public int GetProductNumericValueByOption(string option)
        {
            var result = Connection.ExecuteScalar<int>($"select dbo.GetProductNumericValueByOption('{option}')");
            return result;
        }
        public CartItemsInfo_Result MergeItemToCart(int userId, int itemId, int? quantity = null, bool isDeleted = false)
        {
            var obj = new
            {
                @userId = userId,
                @itemId = itemId,
                @quantity = quantity,
                @isDeleted = isDeleted
            };
            return Connection.QueryFirstOrDefault<CartItemsInfo_Result>("dbo.MergeItemToCart", obj, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<CartItemsInfo_Result> GetCartItemsInfo(int userId)
        {
            var obj = new
            {
                @userId = userId
            };
            return Connection.Query<CartItemsInfo_Result>("dbo.GetCartItemsInfo", obj, commandType: CommandType.StoredProcedure);
        }
        public bool CreateOrder(int userId, int deliveryTypeId, int paymentTypeId, string email, string firstName, string lastName, string phone, string address, string city, string region)
        {
            bool success;
            try
            {
                var obj = new
                {
                    @userId = userId,
                    @deliveryTypeId = deliveryTypeId,
                    @paymentTypeId = paymentTypeId,
                    @email = email,
                    @firstName = firstName,
                    @lastName = lastName,
                    @phone = phone,
                    @address = address,
                    @city = city,
                    @region = region
                };
                Connection.Execute("dbo.CreateOrder", obj, commandType: CommandType.StoredProcedure);
                success = true;
            }
            catch (System.Exception ex)
            {
                success = false;
            }
            return success;
        }
        public IEnumerable<OrderDetails_Result> GetOrderDetails(int userId)
        {
            var obj = new
            {
                @userId = userId
            };
            return Connection.Query<OrderDetails_Result>("dbo.GetOrderDetails", obj, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<OrderProductsInfo_Result> GetOrderProductsInfo(int userId, int orderId)
        {
            var obj = new
            {
                @userId = userId,
                @orderId = orderId
            };
            return Connection.Query<OrderProductsInfo_Result>("dbo.GetOrderProductsInfo", obj, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<ProductsInfo_Admin_Result> GetProductsInfo_Admin(string search = null)
        {
            var obj = new
            {
                @search = search
            };
            return Connection.Query<ProductsInfo_Admin_Result>("dbo.GetProductsInfo_Admin", obj, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<OptionalParameter> GetOptionalParametersForProduct_Admin(int? productId)
        {
            var obj = new
            {
                @productId = productId
            };
            return Connection.Query<OptionalParameter>("dbo.GetOptionalParametersForProduct_Admin", obj, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<GetOptionalParametersForItem_Admin_Result> GetOptionalParametersForItem_Admin(int? itemId)
        {
            var obj = new
            {
                @itemId = itemId
            };
            return Connection.Query<GetOptionalParametersForItem_Admin_Result>("dbo.GetOptionalParametersForItem_Admin", obj, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<GetOrdersInfo_Admin_Result> GetOrdersInfo_Admin(string search)
        {
            var obj = new
            {
                @search = search
            };
            return Connection.Query<GetOrdersInfo_Admin_Result>("dbo.GetOrdersInfo_Admin", obj, commandType: CommandType.StoredProcedure);
        }
        public double GetCartSum(int userId)
        {
            return Connection.ExecuteScalar<double>($"select dbo.GetCartSum({userId})");
        }
        public double GetOrderSum(int orderId)
        {
            var obj = new
            {
                @orderId = orderId
            };
            return Connection.ExecuteScalar<double>($"select dbo.GetOrderSum({orderId})");
        }

    }
}
