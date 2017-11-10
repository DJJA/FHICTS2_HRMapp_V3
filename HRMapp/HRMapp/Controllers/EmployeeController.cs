using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMapp.Logic;
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
    }
}