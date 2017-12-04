using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMapp.Logic;
using HRMapp.Models;
using HRMapp.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using HRMapp.ViewModels;

namespace HRMapp.Controllers
{
    public class TaskController : Controller
    {
        //private static CrossActionMessageHolder infoMessage = new CrossActionMessageHolder();
        private TaskLogic taskLogic = new TaskLogic();

        private EmployeeLogic employeeLogic = new EmployeeLogic();

        public IActionResult Index(int id)
        {
            //var tasks = taskLogic.GetAll;
            //if (id == 0 && tasks.Count > 0)
            //{
            //    id = tasks[0].Id;
            //}
            //var model = new TaskCollectionViewModel(id, tasks.ToList()) { InfoMessage = infoMessage.Message };
            //return View("Task", model);
            return RedirectToAction("Index", "Product");
        }

        public IActionResult TaskView(int id)
        {
            var task = taskLogic.GetById(id);
            return PartialView("_TaskView", task);
        }

        public IActionResult New(int productId)
        {
            return View("TaskEditor", new TaskEditorViewModel(employeeLogic.GetAllTeamLeadersAndProductionWorkers, productId));
        }

        [HttpPost]
        public IActionResult New(TaskEditorViewModel model)
        {
            try
            {
                var addedTaskId = taskLogic.Add(model.ToTask(employeeLogic.GetAllTeamLeadersAndProductionWorkers));
                //infoMessage.Message = $"'{model.Name}' is toegevoegd aan het systeem.";
                return RedirectToAction("Edit", "Product", new {id = model.ProductId});
            }
            catch (ArgumentException argEx)
            {
                return View("TaskEditor", new TaskEditorViewModel(employeeLogic.GetAllTeamLeadersAndProductionWorkers, model, EditorType.New, argEx.Message));
            }
            catch (DBException dbEx)
            {
                return View("TaskEditor", new TaskEditorViewModel(employeeLogic.GetAllTeamLeadersAndProductionWorkers, model, EditorType.New, dbEx.Message));
            }
        }

        public IActionResult Edit(int id)
        {
            var task = taskLogic.GetById(id);
            return View("TaskEditor", new TaskEditorViewModel(employeeLogic.GetAllTeamLeadersAndProductionWorkers, task));
        }

        [HttpPost]
        public IActionResult Edit(TaskEditorViewModel model)
        {
            try
            {
                var success = taskLogic.Update(model.ToTask(employeeLogic.GetAllTeamLeadersAndProductionWorkers));
                //infoMessage.Message = $"'{model.Name}' is bewerkt.";
                return RedirectToAction("Edit", "Product", new {id = model.ProductId});
            }
            catch (ArgumentException argEx)
            {
                return View("TaskEditor", new TaskEditorViewModel(employeeLogic.GetAllTeamLeadersAndProductionWorkers, model, EditorType.Edit, argEx.Message));
            }
            catch (DBException dbEx)
            {
                return View("TaskEditor", new TaskEditorViewModel(employeeLogic.GetAllTeamLeadersAndProductionWorkers, model, EditorType.Edit, dbEx.Message));
            }
        }

        //Wat is nou het verschil tussen psot en get, wat zou ik hier gebruiken? Beide zullen werken...
        [HttpPost]
        public IActionResult Delete(int taskId, int productId)
        {
            //try
            //{
            //    taskLogic.Delete(new ProductionTask(taskId));
            //    return RedirectToAction("Edit", "Product", new {id = productId});
            //}
            //catch (ArgumentException argEx)
            //{
            //    return RedirectToAction("Edit", "Product", new {id = productId}); 
            //}
            //catch (DBException dbEx)
            //{
            //    return RedirectToAction("Edit", "Product", new {id = productId});
            //}

            try
            {
                taskLogic.Delete(new ProductionTask(taskId));
                return PartialView("~/Views/Product/_TaskContainerContent.cshtml", taskLogic.GetByProductId(productId));
            }
            catch (ArgumentException argEx)
            {
                return PartialView("~/Views/Product/_TaskContainerContent.cshtml", taskLogic.GetByProductId(productId));
                //TODO Do proper error handling
            }
            catch (DBException dbEx)
            {
                return PartialView("~/Views/Product/_TaskContainerContent.cshtml", taskLogic.GetByProductId(productId));
            }
        }
    }
}