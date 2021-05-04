using SomeShop.DAL.Models;
using SomeShop.Web.Extensions;
using SomeShop.Web.Models.HomeModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace SomeShop.Web.Controllers
{
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            var model = new IndexVM
            {
                Brands = UnitOfWork.Brands.FindAll(),
                NewItems = UnitOfWork.StoredProcedures.GetItemsInfo(8, newItems: true, userId: SystemUser?.Id),
                SaleItems = UnitOfWork.StoredProcedures.GetItemsInfo(8, saleItems: true, userId: SystemUser?.Id)
            };

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult About()
        {
            var model = new AboutVM
            {
                Brands = UnitOfWork.Brands.Random(),
                Product = UnitOfWork.StoredProcedures.GetItemsInfo(1, saleItems: true).FirstOrDefault()
            };

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Contact()
        {
            var contactInfo = UnitOfWork.ContactInfo.FindAll();
            return View(contactInfo);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SendMessage(SendMessageVM model)
        {
            bool success;
            var message = string.Empty;
            try
            {
                if (!model.IsNotRobot)
                {
                    message = "Confirm that you are not a robot!";
                    success = false;
                }
                else if (ModelState.IsValid)
                {
                    var supportMessage = new SupportMessage
                    {
                        Email = model.Email,
                        Message = model.Message,
                        Name = model.Name
                    };
                    success = UnitOfWork.SupportMessages.Merge(supportMessage);
                }
                else
                {
                    message = ModelState.GetHtmlErrors();
                    success = false;
                }
            }
            catch (Exception ex)
            {
                success = false;
                message = "An error occurred while sending! Contact Support.";
            }
            return Json(new { success, message });
        }
    }
}
