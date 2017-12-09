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
using Microsoft.WindowsAzure.Storage.Blob.Protocol;

namespace HRMapp.ViewModels
{
    public class EmployeeEditorViewModel : EditorViewModel
    {
        private List<TeamLeader> teamLeaders = new List<TeamLeader>();

        public List<SelectListItem> EmployeeTypes
        {
            get
            {
                var list = new List<SelectListItem>();
                list.Add(new SelectListItem() { Value = ((int)EmployeeFunction.ProductionWorker).ToString(), Text = EmployeeFunction.ProductionWorker.ToString(), Selected = EmployeeType == EmployeeFunction.None || EmployeeType == EmployeeFunction.ProductionWorker });
                list.Add(new SelectListItem() { Value = ((int)EmployeeFunction.TeamLeader).ToString(), Text = EmployeeFunction.TeamLeader.ToString(), Selected = EmployeeType == EmployeeFunction.TeamLeader });
                list.Add(new SelectListItem() { Value = ((int)EmployeeFunction.HRManager).ToString(), Text = EmployeeFunction.HRManager.ToString(), Selected = EmployeeType == EmployeeFunction.HRManager });
                list.Add(new SelectListItem() { Value = ((int)EmployeeFunction.SalesManager).ToString(), Text = EmployeeFunction.SalesManager.ToString(), Selected = EmployeeType == EmployeeFunction.SalesManager });
                return list;
            }
        }
        public EmployeeFunction EmployeeType { get; set; }

        public List<SelectListItem> TeamLeaders
        {
            get
            {
                var list = new List<SelectListItem>();
                list.Add(new SelectListItem() { Value = "-1", Text = "- - -" });
                list.AddRange(from TeamLeader teamLeader in teamLeaders select new SelectListItem() { Value = teamLeader.Id.ToString(), Text = $"{teamLeader.FirstName} {teamLeader.LastName}", Selected = teamLeader.Id == TeamLeaderId });
                return list;
            }
        }
        public int TeamLeaderId { get; set; }

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


        public List<int> LboxRequiredSkillsets { get; set; } = new List<int>();


        /// <summary>
        /// Used by form to return data to controller
        /// </summary>
        public EmployeeEditorViewModel()
        {
        }

        /// <summary>
        /// Used by controller to create new employee
        /// </summary>
        /// <param name="teamLeaders"></param>
        public EmployeeEditorViewModel(List<TeamLeader> teamLeaders)
        {
            EditorType = EditorType.New;
            this.teamLeaders = teamLeaders;
        }

        /// <summary>
        /// Used by controller to edit
        /// </summary>
        /// <param name="teamLeaders"></param>
        /// <param name="employee"></param>
        public EmployeeEditorViewModel(List<TeamLeader> teamLeaders, Employee employee)
        {
            EditorType = EditorType.Edit;

            this.teamLeaders = teamLeaders;

            if (employee is ProductionWorker worker)
            {
                if (worker.TeamLeader != null)
                {
                    TeamLeaderId = worker.TeamLeader.Id;
                }
                else
                {
                    TeamLeaderId = -1;
                }
            }

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

        /// <summary>
        /// Used by controller when error occurs
        /// </summary>
        /// <param name="teamLeaders"></param>
        /// <param name="viewModel"></param>
        /// <param name="editorType"></param>
        /// <param name="errorMessage"></param>
        public EmployeeEditorViewModel(List<TeamLeader> teamLeaders, EmployeeEditorViewModel viewModel, EditorType editorType, string errorMessage)
        {
            EditorType = editorType;

            this.teamLeaders = teamLeaders;

            EmployeeType = viewModel.EmployeeType;

            TeamLeaderId = viewModel.TeamLeaderId;

            Id = viewModel.Id;
            FirstName = viewModel.FirstName;
            LastName = viewModel.LastName;
            PhoneNumber = viewModel.PhoneNumber;
            EmailAddress = viewModel.EmailAddress;
            Street = viewModel.Street;
            HouseNumber = viewModel.HouseNumber;
            ZipCode = viewModel.ZipCode;
            City = viewModel.City;

            ErrorMessage = errorMessage;
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

        public Employee ToEmployee(EmployeeLogic logic) // TODO Should I even pass the logic in here, do I even want to get all the properties of teamleader?
        {
            if (EmployeeType == EmployeeFunction.ProductionWorker)
            {
                TeamLeader teamLeader = null;
                if (TeamLeaderId > -1)
                {
                    teamLeader = logic.GetTeamLeaderById(TeamLeaderId);
                }
                return new ProductionWorker(Id, FirstName, LastName, PhoneNumber, EmailAddress, Street, HouseNumber, ZipCode, City, teamLeader);
            }
            if (EmployeeType == EmployeeFunction.TeamLeader)
            {
                return new TeamLeader(Id, FirstName, LastName, PhoneNumber, EmailAddress, Street, HouseNumber, ZipCode, City);
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

