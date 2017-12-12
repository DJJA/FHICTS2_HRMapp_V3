using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMapp.Factory;
using HRMapp.Logic;
using HRMapp.Models.Exceptions;
using HRMapp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HRMapp.Controllers
{
    public class OrderController : Controller
    {
        private static CrossActionMessageHolder infoMessage = new CrossActionMessageHolder();
        private OrderLogic orderLogic = OrderFactory.ManageOrders();
        private ProductLogic productLogic = new ProductLogic();

        public IActionResult Index(int id)
        {
            var orders = orderLogic.GetAll.ToList();
            if (id == 0 && orders.Count > 0)
            {
                id = orders[0].Id;
            }
            var model = new OrderCollectionViewModel(id, orders) { InfoMessage = infoMessage.Message };
            return View("Order", model);
        }

        public IActionResult OrderView(int id)
        {
            var order = orderLogic.GetById(id);
            return PartialView("_OrderView", order);
        }

        public IActionResult New()
        {
            return View("OrderEditor", new OrderEditorViewModel(productLogic.GetAll));
        }

        [HttpPost]
        public IActionResult New(OrderEditorViewModel model)
        {
            try
            {
                var addedId = orderLogic.Add(model.ToOrder());
                infoMessage.Message = $"'{model.Id} {model.Customer} {model.Deadline}' is toegevoegd aan het systeem."; //todo Moet ik eigenlijk uit de model halen
                return RedirectToAction("Index", new { id = addedId });
            }
            catch (ArgumentException argEx)
            {
                model.ErrorMessage = argEx.Message;
                model.EditorType = EditorType.New;
                return View("OrderEditor", model);
            }
            catch (DBException dbEx)
            {
                model.ErrorMessage = dbEx.Message;
                model.EditorType = EditorType.New;          // Dit hoeft niet, maar zou ik het er bij zetten zodat de code beter te begrijpen is? Wat werkt het beste voor mij?
                return View("OrderEditor", model);
            }
        }

        public IActionResult Edit(int id)
        {
            var order = orderLogic.GetById(id);
            return View("OrderEditor", new OrderEditorViewModel(order, productLogic.GetAll));
        }

        [HttpPost]
        public IActionResult Edit(OrderEditorViewModel model)
        {
            try
            {
                orderLogic.Update(model.ToOrder()); // todo This function needs a generic name, and needs to be in editor interface
                infoMessage.Message = $"'{model.Id} {model.Customer} {model.Deadline}' is bewerkt.";
                return RedirectToAction("Index", new { id = model.Id });
            }
            catch (ArgumentException argEx)
            {
                model.ErrorMessage = argEx.Message;
                model.EditorType = EditorType.Edit;
                return View("OrderEditor", model);
            }
            catch (DBException dbbEx)
            {
                model.ErrorMessage = dbbEx.Message;
                model.EditorType = EditorType.Edit;
                return View("OrderEditor", model);
            }
        }
    }
}