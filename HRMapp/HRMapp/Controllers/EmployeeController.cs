using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMapp.Logic;
using HRMapp.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using HRMapp.ViewModels;

namespace HRMapp.Controllers
{
    public class EmployeeController : Controller
    {
        private static CrossActionMessageHolder errorMessage = new CrossActionMessageHolder();
        private static CrossActionMessageHolder infoMessage = new CrossActionMessageHolder();
        private EmployeeLogic employeeLogic = new EmployeeLogic();

        public IActionResult Index(int id)
        {
            var employees = employeeLogic.GetAll().ToList();    // Where do I use a List and where an IEnumerable? Where do I convert?
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
            return View("EmployeeEditor", new EmployeeEditorViewModel() { ErrorMessage = errorMessage.Message });
        }

        [HttpPost]
        public IActionResult New(EmployeeEditorViewModel model)
        {
            try
            {
                var addedEmployeeId = employeeLogic.Add(model.ToEmployee(employeeLogic));
                //var addedEmployeeId = 1;
                infoMessage.Message = $"'{model.FirstName} {model.LastName}' is toegevoegd aan het systeem.";
                return RedirectToAction("Index", new { id = addedEmployeeId });
            }
            catch (ArgumentException argEx)
            {
                errorMessage.Message = argEx.Message;
                return RedirectToAction("New");
            }
            catch (DBException dbEx)
            {
                errorMessage.Message = dbEx.Message;
                return RedirectToAction("New");
            }
        }

        public IActionResult Edit(int id)
        {
            var employee = employeeLogic.GetById(id);
            return View("EmployeeEditor", new EmployeeEditorViewModel(employee) { ErrorMessage = errorMessage.Message });
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditorViewModel model)
        {
            try
            {
                var success = employeeLogic.Update(model.ToEmployee(employeeLogic));
                infoMessage.Message = $"'{model.FirstName} {model.LastName}' is bewerkt.";
                return RedirectToAction("Index", new { id = model.Id });
            }
            catch (ArgumentException argEx)
            {
                errorMessage.Message = argEx.Message;
                return RedirectToAction("Edit", new { id = model.Id });
            }

        }
    }
}