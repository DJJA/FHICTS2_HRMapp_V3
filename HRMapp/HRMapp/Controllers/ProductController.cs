using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMapp.Logic;
using HRMapp.Models.Exceptions;
using HRMapp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HRMapp.Controllers
{
    public class ProductController : Controller
    {
        private static CrossActionMessageHolder infoMessage = new CrossActionMessageHolder();
        private ProductLogic productLogic = new ProductLogic();

        public IActionResult Index(int id)
        {
            var products = productLogic.GetAll;
            if (id == 0 && products.Count > 0)
            {
                id = products[0].Id;
            }
            var model = new ProductCollectionViewModel(id, products) { InfoMessage = infoMessage.Message };
            return View("Product", model);
        }

        public IActionResult ProductView(int id)
        {
            var product = productLogic.GetById(id);
            return PartialView("_ProductView", product);
        }

        public IActionResult New()
        {
            return View("ProductEditor", new ProductEditorViewModel());
        }

        [HttpPost]
        public IActionResult New(ProductEditorViewModel model)
        {
            try
            {
                var addedProductId = productLogic.Add(model.ToProduct());
                infoMessage.Message = $"'{model.Name}' is toegevoegd aan het systeem.";
                return RedirectToAction("Index", new { id = addedProductId });
            }
            catch (ArgumentException argEx)
            {
                model.ErrorMessage = argEx.Message;
                model.EditorType = EditorType.New;
                return View("ProductEditor", model);
            }
            catch (DBException dbEx)
            {
                model.ErrorMessage = dbEx.Message;
                model.EditorType = EditorType.New;
                return View("ProductEditor", model);
            }
        }

        public IActionResult Edit(int id)
        {
            var product = productLogic.GetById(id);
            return View("ProductEditor", new ProductEditorViewModel(product));
        }

        [HttpPost]
        public IActionResult Edit(ProductEditorViewModel model)
        {
            try
            {
                productLogic.Update(model.ToProduct());
                infoMessage.Message = $"'{model.Name}' is bewerkt.";
                return RedirectToAction("Index", new { id = model.Id });
            }
            catch (ArgumentException argEx)
            {
                model.ErrorMessage = argEx.Message;
                model.EditorType = EditorType.Edit;
                return View("ProductEditor", model);
            }
            catch (DBException dbbEx)
            {
                model.ErrorMessage = dbbEx.Message;
                model.EditorType = EditorType.Edit;
                return View("ProductEditor", model);
            }
        }
    }
}