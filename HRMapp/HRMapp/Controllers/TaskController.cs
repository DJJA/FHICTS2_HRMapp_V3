using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMapp.DAL.Repositories;
using HRMapp.Logic;
using HRMapp.Models;
using HRMapp.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using HRMapp.ViewModels;

namespace HRMapp.Controllers
{
    public class TaskController : Controller
    {
        private TaskLogic taskLogic = Factory.TaskFactory.ManageTasks();
        private EmployeeLogic employeeLogic = new EmployeeLogic(new EmployeeRepo(ContextType.Mssql));EmployeeRepo rrrr = new EmployeeRepo(ContextType.Mssql);

        public IActionResult Index(int id)
        {
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
                taskLogic.Add(model.ToTask(employeeLogic.GetAllTeamLeadersAndProductionWorkers));
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
                taskLogic.Update(model.ToTask(employeeLogic.GetAllTeamLeadersAndProductionWorkers));
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

        [HttpPost]
        public IActionResult Delete(int taskId, int productId)
        {
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