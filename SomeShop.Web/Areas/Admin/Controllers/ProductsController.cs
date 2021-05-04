using SomeShop.DAL.Models;
using SomeShop.Web.Areas.Admin.Models.ProductsModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SomeShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : AdminController
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult _ProductList(string search)
        {
            var products = UnitOfWork.StoredProcedures.GetProductsInfo_Admin(search);
            return PartialView(products);
        }

        [HttpGet]
        public IActionResult _Product(int? id)
        {
            var model = new ProductVM()
            {
                ProductId = id.GetValueOrDefault()
            };

            var product = UnitOfWork.Products.FindById(id);
            if (product != null)
            {
                model.BrandId = product.BrandId;
                model.Name = product.Name;
                model.Description = product.Description;
            }

            model.Brands = UnitOfWork.Brands.FindAll();
            model.Categories = UnitOfWork.Categories.FindAll();
            model.Characteristics = UnitOfWork.Characteristics.FindAll();
            model.OptionalParameters = UnitOfWork.OptionalParameters.FindAll();
            model.ProductImages = UnitOfWork.ProductImages.Find(x => x.ProductId == id);

            model.SelectedCategories = UnitOfWork.CategoryProducts.Find(x => x.ProductId == id).Select(x => x.CategoryId);
            model.SelectedCharacteristics = UnitOfWork.CharacteristicProducts.Find(x => x.ProductId == id).Select(x => x.CharacteristicId);
            model.SelectedOptionalParameters = UnitOfWork.StoredProcedures.GetOptionalParametersForProduct_Admin(id).Select(x => x.Id);

            var items = UnitOfWork.Items.Find(x => x.ProductId == id);
            var list = new List<ItemVM>();
            foreach (var item in items)
            {
                var itemVM = new ItemVM
                {
                    ItemId = item.Id,
                    OldPrice = item.OldPrice,
                    Price = item.Price,
                    OptionalParametersForItems = UnitOfWork.StoredProcedures.GetOptionalParametersForItem_Admin(item.Id)
                };
                list.Add(itemVM);
            }
            model.Items = list;

            return PartialView(model);
        }

        [HttpPost]
        public IActionResult SaveProduct(ProductVM productVM)
        {
            bool success;
            int productId = 0;
            try
            {
                #region Product
                var product = new Product
                {
                    BrandId = productVM.BrandId,
                    Description = productVM.Description,
                    Name = productVM.Name,
                    Id = productVM.ProductId
                };
                UnitOfWork.Products.Merge(product);
                productId = product.Id;
                #endregion

                #region Categories
                var productCategories = UnitOfWork.CategoryProducts.Find(x => x.ProductId == product.Id, true);
                foreach (var categoryId in productVM.SelectedCategories)
                {
                    if (productCategories.FirstOrDefault(x => x.CategoryId == categoryId) is CategoryProduct categoryProduct)
                    {
                        if (categoryProduct.IsDeleted)
                        {
                            UnitOfWork.CategoryProducts.Restore(categoryProduct);
                        }
                        productCategories = productCategories.Where(x => x.CategoryId != categoryId);
                    }
                    else
                    {
                        categoryProduct = new CategoryProduct
                        {
                            CategoryId = categoryId,
                            ProductId = product.Id
                        };
                    }
                    UnitOfWork.CategoryProducts.Merge(categoryProduct);
                }
                foreach (var categoryProduct in productCategories)
                {
                    UnitOfWork.CategoryProducts.Delete(categoryProduct);
                }
                #endregion

                #region Items
                if (productVM.Items?.Count() > 0)
                {
                    foreach (var item in productVM.Items)
                    {
                        var oldItem = UnitOfWork.Items.FindById(item.ItemId);
                        oldItem.OldPrice = oldItem.Price > item.Price ? (double?)oldItem.Price : null;
                        oldItem.Price = item.Price;
                        UnitOfWork.Items.Update(oldItem);

                        foreach (var optionalParameter in item.OptionalParametersForItems)
                        {
                            var oldOptionalParameter = UnitOfWork.OptionalParameterProducts.FirstOrDefault(x => x.OptionalParameterId == optionalParameter.OptionalParameterId && x.ItemId == oldItem.Id);
                            oldOptionalParameter.Value = optionalParameter.OptionalParameterValue;
                            UnitOfWork.OptionalParameterProducts.Update(oldOptionalParameter);
                        }
                    }
                }
                #endregion

                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(new { success, productId });
        }

        [HttpPost]
        public async Task<IActionResult> UploadProductImages(IEnumerable<IFormFile> files, [FromQuery] int id, [FromQuery] string name)
        {
            bool success;
            try
            {
                #region Product Images
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var filename = $"{name}_{DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).Ticks}{Path.GetExtension(file.FileName)}";
                        var path = Path.Combine(GlobalVariables.ResourceDirectory, ImageFolderNames.ProductsPath, filename);
                        using (var fs = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(fs);
                        }
                        var productImage = new ProductImage
                        {
                            Image = filename,
                            ProductId = id
                        };
                        UnitOfWork.ProductImages.Insert(productImage);
                    }
                }
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }
            #endregion
            return Json(new { success });
        }

        [HttpPost]
        public IActionResult UpdateSpecifications(int productId, int optionalParameterId, bool toDelete = false)
        {
            bool success;
            try
            {
                var items = UnitOfWork.Items.Find(x => x.ProductId == productId, true);
                foreach (var item in items)
                {
                    if (UnitOfWork.OptionalParameterProducts.FirstOrDefault(x => x.ItemId == item.Id && x.OptionalParameterId == optionalParameterId, true) is OptionalParameterProduct optionalParameterProduct)
                    {
                        if (toDelete)
                        {
                            UnitOfWork.OptionalParameterProducts.Delete(optionalParameterProduct);
                        }
                        else
                        {
                            UnitOfWork.OptionalParameterProducts.Restore(optionalParameterProduct);
                        }
                    }
                    else
                    {
                        if (!toDelete)
                        {
                            optionalParameterProduct = new OptionalParameterProduct
                            {
                                ItemId = item.Id,
                                OptionalParameterId = optionalParameterId,
                                Value = string.Empty
                            };
                            UnitOfWork.OptionalParameterProducts.Insert(optionalParameterProduct);
                        }
                    }
                }

                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }
            return Json(new { success });
        }

        [HttpPost]
        public IActionResult AddNewItem(int productId, IEnumerable<int> optionalParameters)
        {
            bool success;
            try
            {
                var item = new Item
                {
                    ProductId = productId
                };
                UnitOfWork.Items.Insert(item);
                foreach (var parameter in optionalParameters)
                {
                    if (UnitOfWork.OptionalParameters.FindById(parameter) is OptionalParameter optionalParameter)
                    {
                        var optionalParameterProduct = new OptionalParameterProduct
                        {
                            ItemId = item.Id,
                            OptionalParameterId = optionalParameter.Id,
                            Value = string.Empty
                        };
                        UnitOfWork.OptionalParameterProducts.Insert(optionalParameterProduct);
                    }
                }
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }
            return Json(new { success });
        }

        [HttpPost]
        public IActionResult DeleteItem(int itemId)
        {
            bool success;
            try
            {
                success = UnitOfWork.Items.Delete(itemId);
            }
            catch (Exception ex)
            {
                success = false;
            }
            return Json(new { success });
        }

        [HttpPost]
        public IActionResult DeleteOrRestoreProduct(int id)
        {
            bool success;
            bool? isDeleted = null;
            try
            {
                var product = UnitOfWork.Products.FindById(id);
                success = UnitOfWork.Products.DeleteOrRestore(product);
                isDeleted = product.IsDeleted;
            }
            catch (Exception)
            {
                success = false;
            }

            return Json(new { success, isDeleted });
        }

        [HttpPost]
        public IActionResult RemoveProductImage(int id)
        {
            bool success;
            try
            {
                success = UnitOfWork.ProductImages.Delete(id);
            }
            catch (Exception)
            {
                success = false;
            }

            return Json(new { success });
        }



        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }
    }
}