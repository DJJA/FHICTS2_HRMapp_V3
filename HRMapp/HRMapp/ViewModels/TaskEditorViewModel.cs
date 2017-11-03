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
        [DisplayName("Titel:")]
        public string Title { get; set; }
        [DisplayName("Omschrijving:")]
        public string Description { get; set; }
        [DisplayName("Aantal uren:")]
        public int DurationHours { get; set; }
        [DisplayName("Aantal minuten:")]
        public int DurationMinutes { get; set; }    // Afvangen minimale en maximale minuten

        public TimeSpan Duration
        {
            get
            {
                return new TimeSpan(DurationHours, DurationMinutes, 0);
            }
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
            //availableSkillsets = new List<Skillset>();
            //requiredSkillsets = new List<Skillset>();
            LboxRequiredSkillsets = new List<int>();             // Hoe weet ik straks waarom ik dit hier doe?
        }

        public TaskEditorViewModel(List<Skillset> availableSkillsets)   // Used for new
        {
            this.availableSkillsets = availableSkillsets;
            FormAction = "NewTask";
            FormTitle = "Nieuwe taak toevoegen";
        }

        public TaskEditorViewModel(List<Skillset> availableSkillsets, ProductionTask task)  // Used for edit
        {
            this.availableSkillsets = availableSkillsets;
            FormAction = "EditTask";
            FormTitle = "Taak bewerken";

            Id = task.Id;
            Title = task.Name;
            Description = task.Description;
            Duration = task.Duration;
            requiredSkillsets = task.RequiredSkillsets;
        }

        public ProductionTask ToTask(List<Skillset> skillsets)
        {
            var requiredSkillsets = new List<Skillset>();
            foreach (var id in LboxRequiredSkillsets)
            {
                requiredSkillsets.Add(skillsets.Single(skillset => skillset.Id == id));
            }
            return new ProductionTask(Id, Title, Description, Duration, requiredSkillsets);
        }
    }
}
