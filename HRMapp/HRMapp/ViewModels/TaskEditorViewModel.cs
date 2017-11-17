using HRMapp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HRMapp.ViewModels
{
    public class TaskEditorViewModel
    {
        private List<Skillset> availableSkillsets = new List<Skillset>();
        private List<Skillset> requiredSkillsets = new List<Skillset>();

        public string ErrorMessage { get; set; }
        public string FormAction { get; private set; }
        public string FormTitle { get; private set; }
        public int Id { get; set; }
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
                foreach (var skillset in availableSkillsets)
                {
                    if(!requiredSkillsets.Any(s => s.Id == skillset.Id))      // Not optimal n^2
                    {
                        list.Add(new SelectListItem() { Text = skillset.Name, Value = skillset.Id.ToString() });
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
                foreach (var skillset in requiredSkillsets)
                {
                    list.Add(new SelectListItem() { Text = skillset.Name, Value = skillset.Id.ToString() });
                }
                return list;
            }
        }

        //public List<int> LboxAvailableSkillsets { get; set; }
        public List<int> LboxRequiredSkillsets { get; set; }
        
        public TaskEditorViewModel()                // This constructor is for the taskEditor so it can return something, needs empty constructor
        {
            LboxRequiredSkillsets = new List<int>();             // Hoe weet ik straks waarom ik dit hier doe?
            Id = -1;    // Als hij een gevuld viewmodel meegeeft, roept ie deze constructor niet aan?
        }

        public TaskEditorViewModel(List<Skillset> availableSkillsets)   // Used for new
        {
            this.availableSkillsets = availableSkillsets;
            FormAction = "New";
            FormTitle = "Nieuwe taak toevoegen";
        }

        public TaskEditorViewModel(List<Skillset> availableSkillsets, ProductionTask task)  // Used for edit
        {
            this.availableSkillsets = availableSkillsets;
            FormAction = "Edit";
            FormTitle = "Taak bewerken";

            Id = task.Id;
            Name = task.Name;
            Description = task.Description;
            Duration = task.Duration;
            requiredSkillsets = task.RequiredSkillsets;
        }

        public TaskEditorViewModel(List<Skillset> availableSkillsets, TaskEditorViewModel viewModel,
            string errorMessage)
        {
            this.availableSkillsets = availableSkillsets;

            if (viewModel.Id > -1)
            {
                FormAction = "Edit";
                FormTitle = "Taak bewerken";
            }
            else
            {
                FormAction = "New";
                FormTitle = "Nieuwe taak toevoegen";
            }

            Id = viewModel.Id;
            Name = viewModel.Name;
            Description = viewModel.Description;
            Duration = viewModel.Duration;

            var requiredSkillsets = new List<Skillset>();
            foreach (var id in viewModel.LboxRequiredSkillsets)
            {
                Skillset skillset = availableSkillsets.Single(s => s.Id == id);
                if (skillset != null)
                {
                    requiredSkillsets.Add(skillset);
                }
            }
            this.requiredSkillsets = requiredSkillsets;

            ErrorMessage = errorMessage;
        }

        public ProductionTask ToTask(List<Skillset> skillsets)
        {
            var requiredSkillsets = new List<Skillset>();
            foreach (var id in LboxRequiredSkillsets)
            {
                requiredSkillsets.Add(skillsets.Single(skillset => skillset.Id == id));
            }
            return new ProductionTask(Id, Name, Description, Duration, requiredSkillsets);
        }
    }
}
