using SomeShop.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace SomeShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactInfoController : AdminController
    {
        [HttpGet]
        public IActionResult Index()
        {
            var contactInfo = UnitOfWork.ContactInfo.FindAll(true);
            return View(contactInfo);
        }

        [HttpGet]
        public IActionResult GetContactInfo(int contactId)
        {
            var contactInfo = UnitOfWork.ContactInfo.FindById(contactId);
            return Json(contactInfo);
        }

        [HttpPost]
        public IActionResult UpdateContactInfo(ContactInfo contact)
        {
            bool success;
            try
            {
                success = UnitOfWork.ContactInfo.Merge(contact);
            }
            catch (Exception)
            {
                success = false;
            }
            return Json(new { success });
        }

        [HttpPost]
        public IActionResult DeleteOrRestoreContact(int id)
        {
            bool success;
            bool? isDeleted = null;
            try
            {
                var contact = UnitOfWork.ContactInfo.FindById(id);
                success = UnitOfWork.ContactInfo.DeleteOrRestore(contact);
                isDeleted = contact.IsDeleted;
            }
            catch (Exception)
            {
                success = false;
            }

            return Json(new { success, isDeleted });
        }
    }
}