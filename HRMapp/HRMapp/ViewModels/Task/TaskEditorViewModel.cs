﻿using HRMapp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HRMapp.ViewModels
{
    public class TaskEditorViewModel : EditorViewModel
    {
        private List<Employee> availableEmployees = new List<Employee>();
        private List<Employee> qualifiedEmployees = new List<Employee>();

        public override string FormTitle
        {
            get
            {
                switch (EditorType)
                {
                    case EditorType.New:
                        return "Nieuwe taak toevoegen";
                    case EditorType.Edit:
                        return "Taak bewerken";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public int ProductId { get; set; }

        [DisplayName("Naam:")]
        public string Name { get; set; }
        [DisplayName("Omschrijving:")]
        public string Description { get; set; }
        [DisplayName("Aantal uren:")]
        public int DurationHours { get; set; }
        [DisplayName("Aantal minuten:")]
        public int DurationMinutes { get; set; }    // Afvangen minimale en maximale minuten

        public TimeSpan Duration
        {
            get => new TimeSpan(DurationHours, DurationMinutes, 0);
            private set
            {
                DurationHours = value.Hours;
                DurationMinutes = value.Minutes;
            }
        }

        public List<SelectListItem> AvailableSkillsetListItems
        {
            get
            {
                var list = new List<SelectListItem>();
                foreach (var employee in availableEmployees)
                {
                    if(qualifiedEmployees.All(s => s.Id != employee.Id))      // Not optimal n^2
                    {
                        list.Add(new SelectListItem() { Text = $"{employee.FirstName} {employee.LastName}", Value = employee.Id.ToString() });
                    }
                }
                return list;
            }
        }

        public List<SelectListItem> RequiredSkillsetListItems
        {
            get
            {
                var list = new List<SelectListItem>();
                foreach (var employee in qualifiedEmployees)
                {
                    list.Add(new SelectListItem() { Text = $"{employee.FirstName} {employee.LastName}", Value = employee.Id.ToString() });
                }
                return list;
            }
        }

        //public List<int> LboxAvailableSkillsets { get; set; }
        public List<int> LboxQualifiedEmployees { get; set; }

        /// <summary>
        /// Used by form to send back data
        /// </summary>
        public TaskEditorViewModel()
        {
            LboxQualifiedEmployees = new List<int>(); // is dit nodig?
        }

        /// <summary>
        /// Used by controller to create a new task
        /// </summary>
        /// <param name="availableEmployees"></param>
        public TaskEditorViewModel(List<Employee> availableEmployees, int productId)
        {
            this.availableEmployees = availableEmployees;
            EditorType = EditorType.New;
            ProductId = productId;
        }

        /// <summary>
        /// Used by controller to edit a task
        /// </summary>
        /// <param name="availableEmployees"></param>
        /// <param name="task"></param>
        public TaskEditorViewModel(List<Employee> availableEmployees, ProductionTask task)
        {
            this.availableEmployees = availableEmployees;
            EditorType = EditorType.Edit;

            Id = task.Id;
            ProductId = task.Product.Id;
            Name = task.Name;
            Description = task.Description;
            Duration = task.Duration;
            qualifiedEmployees = task.Employees;
        }

        /// <summary>
        /// Used by controller when an error occurred
        /// </summary>
        /// <param name="availableEmployees"></param>
        /// <param name="viewModel"></param>
        /// <param name="errorMessage"></param>
        public TaskEditorViewModel(List<Employee> availableEmployees, TaskEditorViewModel viewModel, EditorType editorType, string errorMessage)
        {
            this.availableEmployees = availableEmployees;
            EditorType = editorType;

            Id = viewModel.Id;
            ProductId = viewModel.ProductId;
            Name = viewModel.Name;
            Description = viewModel.Description;
            Duration = viewModel.Duration;

            var requiredSkillsets = new List<Employee>();
            foreach (var id in viewModel.LboxQualifiedEmployees)
            {
                Employee employee = availableEmployees.Single(s => s.Id == id);
                if (employee != null)
                {
                    requiredSkillsets.Add(employee);
                }
            }
            this.qualifiedEmployees = requiredSkillsets;

            ErrorMessage = errorMessage;
        }

        public ProductionTask ToTask(List<Employee> employees)
        {
            var qualifiedEmployees = new List<Employee>();
            foreach (var id in LboxQualifiedEmployees)
            {
                qualifiedEmployees.Add(employees.Single(skillset => skillset.Id == id));
            }
            return new ProductionTask(Id, new Product(ProductId), Name, Description, Duration, qualifiedEmployees); //TODO change product id -1 to something else
        }
    }
}
