using SomeShop.DAL.Models;
using SomeShop.Web.Areas.Admin.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace SomeShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : AdminController
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult _Order(int id)
        {
            var model = new OrderVM
            {
                OrderId = id
            };

            var order = UnitOfWork.Orders.FindById(id);
            model.UserId = order.UserId;
            model.Address = order.Address;
            model.City = order.City;
            model.Email = order.Email;
            model.FirstName = order.FirstName;
            model.LastName = order.LastName;
            model.OrderDeliveryTypeId = order.OrderDeliveryType;
            model.OrderPaymentTypeId = order.OrderPaymentType;
            model.OrderStatusId = order.OrderStatus;
            model.Phone = order.Phone;
            model.Region = order.Region;
            model.OrderDeliveryTypes = UnitOfWork.OrderDeliveryTypes.FindAll();
            model.OrderPaymentTypes = UnitOfWork.OrderPaymentTypes.FindAll();
            model.OrderStatuses = UnitOfWork.OrderStatuses.FindAll();
            model.OrderProducts = UnitOfWork.StoredProcedures.GetOrderProductsInfo(order.UserId, id);
            model.TotalSum = UnitOfWork.StoredProcedures.GetOrderSum(id);

            return PartialView(model);
        }
        [HttpGet]
        public IActionResult _OrderList(string search)
        {
            var orders = UnitOfWork.StoredProcedures.GetOrdersInfo_Admin(search);
            return PartialView(orders);
        }
        [HttpPost]
        public IActionResult ChangeOrderItemQuantity(int itemId, int orderId, int quantity)
        {
            bool success;
            string message = string.Empty;
            double itemTotal = 0;
            double total = 0;
            try
            {
                var orderItem = UnitOfWork.OrderProducts.FirstOrDefault(x => x.ItemId == itemId && x.OrderId == orderId);
                orderItem.Quantity = quantity;
                success = UnitOfWork.OrderProducts.Update(orderItem);
                itemTotal = quantity * orderItem.Price;
                total = UnitOfWork.StoredProcedures.GetOrderSum(orderId);
            }
            catch (Exception)
            {
                success = false;
                message = "An error occurred while updating!";
            }
            return Json(new { success, message, itemTotal, total });
        }
        [HttpPost]
        public IActionResult DeleteOrderItem(int itemId, int orderId)
        {
            bool success;
            string message = string.Empty;
            double total = 0;
            try
            {
                var orderItem = UnitOfWork.OrderProducts.FirstOrDefault(x => x.ItemId == itemId && x.OrderId == orderId);
                success = UnitOfWork.OrderProducts.Delete(orderItem);
                total = UnitOfWork.StoredProcedures.GetOrderSum(orderId);
            }
            catch (Exception)
            {
                success = false;
                message = "An error occurred while deleting!";
            }
            return Json(new { success, message, total });
        }
        [HttpPost]
        public IActionResult SaveOrderInfo(OrderVM orderVM)
        {
            bool success;
            string message = string.Empty;
            Order order = null;
            try
            {
                order = UnitOfWork.Orders.FindById(orderVM.OrderId);
                order.Address = orderVM.Address;
                order.City = orderVM.City;
                order.Email = orderVM.Email;
                order.FirstName = orderVM.FirstName;
                order.LastName = orderVM.LastName;
                order.OrderDeliveryType = orderVM.OrderDeliveryTypeId;
                order.OrderPaymentType = orderVM.OrderPaymentTypeId;
                order.OrderStatus = orderVM.OrderStatusId;
                order.Phone = orderVM.Phone;
                order.Region = orderVM.Region;
                success = UnitOfWork.Orders.Update(order);
            }
            catch (Exception)
            {
                success = false;
                message = "An error occurred while deleting!";
            }
            return Json(new { success, message, order });
        }
        [HttpPost]
        public IActionResult DeleteOrRestoreOrder(int id)
        {
            bool success;
            bool? isDeleted = null;
            try
            {
                var order = UnitOfWork.Orders.FindById(id);
                success = UnitOfWork.Orders.DeleteOrRestore(order);
                isDeleted = order.IsDeleted;
            }
            catch (Exception)
            {
                success = false;
            }

            return Json(new { success, isDeleted });
        }
    }
}