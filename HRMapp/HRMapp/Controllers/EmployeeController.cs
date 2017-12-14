using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMapp.Factory;
using HRMapp.Logic;
using HRMapp.Models;
using HRMapp.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using HRMapp.ViewModels;

namespace HRMapp.Controllers
{
    public class EmployeeController : Controller
    {
        private static CrossActionMessageHolder infoMessage = new CrossActionMessageHolder();
        private IEmployeeLogic employeeLogic = new EmployeeFactory().Manage();

        public IActionResult Index(int id)
        {
            var employees = employeeLogic.GetAll();
            if (id == 0 && employees.Count > 0)                 // 0 Is the default id, no parameter passed
            {
                id = employees[0].Id;
            }
            var model = new EmployeeCollectionViewModel(id, employees) { InfoMessage = infoMessage.Message };
            return View("Employee", model);
        }

        public IActionResult EmployeeView(int id)
        {
            var employee = employeeLogic.GetById(id);
            return PartialView("_EmployeeView", employee);
        }

        public IActionResult New()
        {
            return View("EmployeeEditor", new EmployeeEditorViewModel(employeeLogic.GetAllTeamLeaders()));
        }

        [HttpPost]
        public IActionResult New(EmployeeEditorViewModel model)
        {
            try
            {
                var addedEmployeeId = employeeLogic.Add(model.ToEmployee(employeeLogic));
                infoMessage.Message = $"'{model.FirstName} {model.LastName}' is toegevoegd aan het systeem.";
                return RedirectToAction("Index", new { id = addedEmployeeId });
            }
            catch (ArgumentException argEx)
            {
                return View("EmployeeEditor", new EmployeeEditorViewModel(employeeLogic.GetAllTeamLeaders(), model, EditorType.Edit, argEx.Message));
            }
            catch (DBException dbEx)
            {
                return View("EmployeeEditor", new EmployeeEditorViewModel(employeeLogic.GetAllTeamLeaders(), model, EditorType.Edit, dbEx.Message));
            }
        }

        public IActionResult Edit(int id)
        {
            var employee = employeeLogic.GetById(id);
            return View("EmployeeEditor", new EmployeeEditorViewModel(employeeLogic.GetAllTeamLeaders(), employee));
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditorViewModel model)
        {
            try
            {
                employeeLogic.Update(model.ToEmployee(employeeLogic));
                infoMessage.Message = $"'{model.FirstName} {model.LastName}' is bewerkt.";
                return RedirectToAction("Index", new { id = model.Id });
            }
            catch (ArgumentException argEx)
            {
                return View("EmployeeEditor", new EmployeeEditorViewModel(employeeLogic.GetAllTeamLeaders(), model, EditorType.Edit, argEx.Message));
            }
            catch (DBException dbEx)
            {
                return View("EmployeeEditor", new EmployeeEditorViewModel(employeeLogic.GetAllTeamLeaders(), model, EditorType.Edit, dbEx.Message));
            }

        }
    }
}