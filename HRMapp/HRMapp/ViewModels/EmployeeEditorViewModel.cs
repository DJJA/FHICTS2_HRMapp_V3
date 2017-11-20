using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Resources;
using System.Threading.Tasks;
using HRMapp.Logic;
using HRMapp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMapp.ViewModels
{
    public enum EmployeeFunction
    {
        ProductionWorker,
        TeamLeader,
        HRManager,
        SalesManager,
        None
    }

    public class EmployeeEditorViewModel : EditorViewModel
    {
        //private Employee employee = null;
        //private EmployeeFunction employeeFunction = EmployeeFunction.None;

        public List<SelectListItem> EmployeeTypes
        {
            get
            {
                var list = new List<SelectListItem>();
                //list.Add(new SelectListItem() { Value = "0", Text = "Productiemedewerk(st)er", Selected = employee == null || employee is ProductionWorker});
                //list.Add(new SelectListItem() { Value = "1", Text = "Teamleider", Selected = employee is TeamLeader});
                //list.Add(new SelectListItem() { Value = "2", Text = "HR Manager", Selected = employee is HRManager});
                //list.Add(new SelectListItem() { Value = "3", Text = "Sales Manager", Selected = employee is SalesManager});
                list.Add(new SelectListItem() { Value = ((int)EmployeeFunction.ProductionWorker).ToString(), Text = EmployeeFunction.ProductionWorker.ToString(), Selected = EmployeeType == EmployeeFunction.None || EmployeeType == EmployeeFunction.ProductionWorker });
                list.Add(new SelectListItem() { Value = ((int)EmployeeFunction.TeamLeader).ToString(), Text = EmployeeFunction.TeamLeader.ToString(), Selected = EmployeeType == EmployeeFunction.TeamLeader });
                list.Add(new SelectListItem() { Value = ((int)EmployeeFunction.HRManager).ToString(), Text = EmployeeFunction.HRManager.ToString(), Selected = EmployeeType == EmployeeFunction.HRManager });
                list.Add(new SelectListItem() { Value = ((int)EmployeeFunction.SalesManager).ToString(), Text = EmployeeFunction.SalesManager.ToString(), Selected = EmployeeType == EmployeeFunction.SalesManager });
                return list;
            }
        }
        public EmployeeFunction EmployeeType { get; set; }

        public override string FormTitle
        {
            get
            {
                switch (EditorType)
                {
                    case EditorType.New:
                        return "Nieuwe werknemer toevoegen";
                    case EditorType.Edit:
                        return "Werknemer bewerken";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        [DisplayName("Voornaam:")]
        public string FirstName { get; set; }
        [DisplayName("Achternaam:")]
        public string LastName { get; set; }
        [DisplayName("Telefoonnummer:")]
        public string PhoneNumber { get; set; }
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

        /// <summary>
        /// Used by form to return data to controller
        /// Used by controller to create new employee
        /// </summary>
        public EmployeeEditorViewModel()
        {
            //FormAction = "New";
            //FormTitle = "Nieuwe werknemer toevoegen";
            EditorType = EditorType.New;                // Remove this when skillsets are added
            //employeeFunction = (EmployeeFunction)EmployeeType;
        }

        /// <summary>
        /// Used by controller to edit employee
        /// </summary>
        /// <param name="employee"></param>
        public EmployeeEditorViewModel(Employee employee)
        {
            //FormAction = "Edit";
            //FormTitle = "Werknemer bewerken";
            EditorType = EditorType.Edit;

            //this.employee = employee;

            //employeeFunction = GetEmployeeFunction(employee);
            EmployeeType = GetEmployeeFunction(employee);

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

        private EmployeeFunction GetEmployeeFunction(Employee employee)
        {
            switch (employee)
            {
                case ProductionWorker _:
                    return EmployeeFunction.ProductionWorker;
                case TeamLeader _:
                    return EmployeeFunction.TeamLeader;
                case HRManager _:
                    return EmployeeFunction.HRManager;
                case SalesManager _:
                    return EmployeeFunction.SalesManager;
                default:
                    return EmployeeFunction.None;
            }
        }

        public Employee ToEmployee(EmployeeLogic logic)
        {
            //if (EmployeeType == (int)EmployeeFunction.ProductionWorker)
            //{
            //    return new ProductionWorker(Id, FirstName, LastName, PhoneNumber, EmailAddress, Street, HouseNumber, ZipCode, City, null, null);
            //}
            //if (EmployeeType == (int)EmployeeFunction.TeamLeader)
            //{
            //    return new TeamLeader(Id, FirstName, LastName, PhoneNumber, EmailAddress, Street, HouseNumber, ZipCode, City, null, null);
            //}
            //if (EmployeeType == (int)EmployeeFunction.HRManager)
            //{
            //    return new HRManager(Id, FirstName, LastName, PhoneNumber, EmailAddress, Street, HouseNumber, ZipCode, City);
            //}
            //if (EmployeeType == (int)EmployeeFunction.SalesManager)
            //{
            //    return new SalesManager(Id, FirstName, LastName, PhoneNumber, EmailAddress, Street, HouseNumber, ZipCode, City);
            //}
            if (EmployeeType == EmployeeFunction.ProductionWorker)
            {
                return new ProductionWorker(Id, FirstName, LastName, PhoneNumber, EmailAddress, Street, HouseNumber, ZipCode, City, null, null);
            }
            if (EmployeeType == EmployeeFunction.TeamLeader)
            {
                return new TeamLeader(Id, FirstName, LastName, PhoneNumber, EmailAddress, Street, HouseNumber, ZipCode, City, null, null);
            }
            if (EmployeeType == EmployeeFunction.HRManager)
            {
                return new HRManager(Id, FirstName, LastName, PhoneNumber, EmailAddress, Street, HouseNumber, ZipCode, City);
            }
            if (EmployeeType == EmployeeFunction.SalesManager)
            {
                return new SalesManager(Id, FirstName, LastName, PhoneNumber, EmailAddress, Street, HouseNumber, ZipCode, City);
            }
            return null;
        }
    }
}

