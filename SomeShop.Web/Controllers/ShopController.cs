using SomeShop.DAL.Models.StoredProcedureModels;
using SomeShop.Web.Extensions;
using SomeShop.Web.Models.ShopModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace SomeShop.Web.Controllers
{
    public class ShopController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(int? categoryId = null, int? brandId = null, string search = null, string categoriesStr = null, int? minPrice = null, int? maxPrice = null)
        {
            var categories = categoriesStr?.Split(',').Select(int.Parse).AsEnumerable();

            var products = UnitOfWork.StoredProcedures.SearchItemInfo(SystemUser?.Id, brandId: brandId, categoryId: categoryId, minPrice: minPrice, maxPrice: maxPrice, search: search);
            var hasProducts = products.Count() == 0;
            var _minPrice = minPrice.HasValue ? minPrice.Value : hasProducts ? 0 : products.Min(x => x.Price);
            var _maxPrice = maxPrice.HasValue ? maxPrice.Value : hasProducts ? 0 : products.Max(x => x.Price);

            if (categories?.Count() > 0)
            {
                products = products.Where(x => UnitOfWork.StoredProcedures.CategoriesForProduct(x.ProductId).Any(y => categories.Contains(y.Id)));
            }

            var model = new IndexVM
            {
                Products = products.ChunkBy(6),
                CategoryHierarchy = UnitOfWork.StoredProcedures.GetCategoryHierarchy(),
                MinPrice = UnitOfWork.StoredProcedures.GetProductNumericValueByOption(ProductOption.MinPrice.GetDescription()),
                MaxPrice = UnitOfWork.StoredProcedures.GetProductNumericValueByOption(ProductOption.MaxPrice.GetDescription()),
                Search = search,
                SelectedMinPrice = minPrice,
                SelectedMaxPrice = maxPrice,
                Categories = categories
            };
            if (categoryId.HasValue)
            {
                model.Categories = new int[] { categoryId.Value };
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Product(int id)
        {
            var model = new ProductVM
            {
                Product = UnitOfWork.Products.FindById(id),
                Products = UnitOfWork.StoredProcedures.GetItemsInfo(8, userId: SystemUser?.Id),
                Categories = UnitOfWork.StoredProcedures.CategoriesForProduct(id),
                ProductImages = UnitOfWork.StoredProcedures.GetProductImages(id),
                Items = UnitOfWork.Items.Find(x => x.ProductId == id),
                ItemsWithParameters = UnitOfWork.StoredProcedures.GetItemWithParameters(id),
                Favourite = UnitOfWork.FavouriteProducts.Find(x => x.UserId == SystemUser?.Id && x.ProductId == id).Any()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult _Cart()
        {
            var model = new CartVM
            {
                CartItems = UnitOfWork.StoredProcedures.GetCartItemsInfo(SystemUser.Id)
            };
            return PartialView(model);
        }

        [HttpGet]
        public IActionResult _OrderProductInfo(int orderId)
        {
            var model = UnitOfWork.StoredProcedures.GetOrderProductsInfo(SystemUser.Id, orderId);
            return PartialView(model);
        }

        [HttpGet]
        public IActionResult Cart()
        {
            var model = new CartVM
            {
                CartItems = UnitOfWork.StoredProcedures.GetCartItemsInfo(SystemUser.Id)
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var model = new CheckoutVM
            {
                CartItems = UnitOfWork.StoredProcedures.GetCartItemsInfo(SystemUser.Id),
                OrderDeliveryTypes = UnitOfWork.OrderDeliveryTypes.FindAll(),
                OrderPaymentTypes = UnitOfWork.OrderPaymentTypes.FindAll()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Order(OrderVM model)
        {
            bool success;
            var message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    success = UnitOfWork.StoredProcedures.CreateOrder(SystemUser.Id, model.DeliveryTypeId.Value, model.PaymentTypeId.Value,
                        model.Email, model.FirstName, model.LastName, model.Phone, model.Address, model.City, model.Region);
                }
                else
                {
                    success = false;
                    message = ModelState.GetHtmlErrors();
                    return Json(new { success, message });
                }
            }
            catch (System.Exception)
            {
                success = false;
            }
            if (success)
            {
                message = "Your order has been created. Our manager will contact you soon!";
            }
            else
            {
                message = "An error occurred while creating your order! Please contact support.";
            }

            return Json(new { success, message });
        }

        [HttpPost]
        public IActionResult AddToCart(int itemId, int? quantity = null)
        {
            bool success;
            var message = string.Empty;
            double cartTotal = 0;
            var item = (CartItemsInfo_Result)null;
            try
            {
                var isZero = quantity.HasValue && quantity.Value == 0;
                item = UnitOfWork.StoredProcedures.MergeItemToCart(SystemUser.Id, itemId, isZero ? null : quantity, isZero);
                cartTotal = UnitOfWork.StoredProcedures.GetCartSum(SystemUser.Id);
                success = true;
                message = "The item has been added to the cart!";
            }
            catch (System.Exception)
            {
                success = false;
                message = "An error occurred while adding a product! Please contact support.";
            }

            return Json(new { success, message, item, cartTotal });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int itemId)
        {
            bool success;
            var message = string.Empty;
            try
            {
                UnitOfWork.StoredProcedures.MergeItemToCart(SystemUser.Id, itemId, isDeleted: true);
                success = true;
                message = "The item has been removed from the cart!";
            }
            catch (System.Exception)
            {
                success = false;
                message = "An error occurred while adding a product! Please contact support.";
            }

            return Json(new { success, message });
        }
    }
}