using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using HRMapp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMapp.ViewModels
{
    public class EmployeeEditorViewModel
    {
        private Employee employee = null;

        public List<SelectListItem> EmployeeTypes
        {
            get
            {
                var list = new List<SelectListItem>();
                list.Add(new SelectListItem() { Value = "0", Text = "Productiemedewerk(st)er", Selected = employee == null || employee is ProductionWorker});
                list.Add(new SelectListItem() { Value = "1", Text = "Teamleider", Selected = employee is TeamLeader});
                list.Add(new SelectListItem() { Value = "2", Text = "HR Manager", Selected = employee is HRManager});
                list.Add(new SelectListItem() { Value = "3", Text = "Sales Manager", Selected = employee is SalesManager});
                return list;
            }
        }
        public int EmployeeType { get; set; }

        public string ErrorMessage { get; set; }
        public string FormAction { get; private set; }
        public string FormTitle { get; private set; }

        public int Id { get; set; }
        [DisplayName("Voornaam:")]
        public string FirstName { get; set; }
        [DisplayName("Achternaam:")]
        public string LastName { get; set; }
        [DisplayName("Telefoonnummer:")]
        public Int64 PhoneNumber { get; set; }
        [DisplayName("Emailadres:")]
        public string EmailAddress { get; set; }
        [DisplayName("Straat:")]
        public string Street { get; set; }
        [DisplayName("Huisnummer:")]
        public string HouseNumber { get; set; }
        [DisplayName("Postcode:")]
        public string ZipCode { get; set; }
        [DisplayName("Woonplaats:")]
        public string City { get; set; }

        public EmployeeEditorViewModel()
        {
            FormAction = "New";
            FormTitle = "Nieuwe werknemer toevoegen";
        }

        public EmployeeEditorViewModel(Employee employee)
        {
            FormAction = "Edit";
            FormTitle = "Werknemer bewerken";

            this.employee = employee;

            Id = employee.Id;
            FirstName = employee.FirstName;
            LastName = employee.LastName;
            PhoneNumber = employee.PhoneNumber;
            EmailAddress = employee.EmailAddress;
            Street = employee.Street;
            HouseNumber = employee.HouseNumber;
            ZipCode = employee.ZipCode;
            City = employee.City;
        }

        public Employee ToSkillset()
        {
            //return new Skillset(Id, Name, Description);
            return null;
        }
    }
}
