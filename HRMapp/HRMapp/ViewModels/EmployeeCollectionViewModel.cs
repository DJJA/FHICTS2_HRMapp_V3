using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMapp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMapp.ViewModels
{
    public class EmployeeCollectionViewModel
    {
        public string InfoMessage { get; set; }
        private List<Employee> employees;
        private int selectedItemId;

        public List<SelectListItem> ListItems { get; private set; }
        public List<Employee> Employees
        {
            get { return employees; }
            private set
            {
                employees = value;

                ListItems = new List<SelectListItem>();
                foreach (var employee in employees)
                {
                    ListItems.Add(new SelectListItem() { Text = $"{employee.FirstName} {employee.LastName}", Value = employee.Id.ToString(), Selected = (employee.Id == selectedItemId) });
                }
            }
        }

        public EmployeeCollectionViewModel(int selectedItemId, List<Employee> employees)
        {
            this.selectedItemId = selectedItemId;
            Employees = employees;
        }
    }
}
