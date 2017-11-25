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
    public class OrderController : Controller
    {
        private static CrossActionMessageHolder infoMessage = new CrossActionMessageHolder();
        private OrderLogic orderLogic = new OrderLogic();

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

        //public IActionResult New()
        //{
        //    return View("SkillsetEditor", new SkillsetEditorViewModel());
        //}

        //[HttpPost]
        //public IActionResult New(SkillsetEditorViewModel model)
        //{
        //    try
        //    {
        //        var addedSkillsetId = orderLogic.Add(model.ToSkillset());
        //        infoMessage.Message = $"'{model.Name}' is toegevoegd aan het systeem.";
        //        return RedirectToAction("Index", new { id = addedSkillsetId });
        //    }
        //    catch (ArgumentException argEx)
        //    {
        //        model.ErrorMessage = argEx.Message;
        //        model.EditorType = EditorType.New;
        //        return View("SkillsetEditor", model);
        //    }
        //    catch (DBException dbEx)
        //    {
        //        model.ErrorMessage = dbEx.Message;
        //        model.EditorType = EditorType.New;          // Dit hoeft niet, maar zou ik het er bij zetten zodat de code beter te begrijpen is? Wat werkt het beste voor mij?
        //        return View("SkillsetEditor", model);
        //    }
        //}

        //public IActionResult Edit(int id)
        //{
        //    var skillset = orderLogic.GetById(id);
        //    return View("SkillsetEditor", new SkillsetEditorViewModel(skillset));
        //}

        //[HttpPost]
        //public IActionResult Edit(SkillsetEditorViewModel model)
        //{
        //    try
        //    {
        //        orderLogic.Update(model.ToSkillset());
        //        infoMessage.Message = $"'{model.Name}' is bewerkt.";
        //        return RedirectToAction("Index", new { id = model.Id });
        //    }
        //    catch (ArgumentException argEx)
        //    {
        //        model.ErrorMessage = argEx.Message;
        //        model.EditorType = EditorType.Edit;
        //        return View("SkillsetEditor", model);
        //    }
        //    catch (DBException dbbEx)
        //    {
        //        model.ErrorMessage = dbbEx.Message;
        //        model.EditorType = EditorType.Edit;
        //        return View("SkillsetEditor", model);
        //    }
        //}
    }
}