using HRMapp.Models;
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
        private List<Skillset> availableSkillsets = new List<Skillset>();
        private List<Skillset> requiredSkillsets = new List<Skillset>();

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
                    if(requiredSkillsets.All(s => s.Id != skillset.Id))      // Not optimal n^2
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

        /// <summary>
        /// Used by form to send back data
        /// </summary>
        public TaskEditorViewModel()
        {
            LboxRequiredSkillsets = new List<int>();
        }

        /// <summary>
        /// Used by controller to create a new task
        /// </summary>
        /// <param name="availableSkillsets"></param>
        public TaskEditorViewModel(List<Skillset> availableSkillsets)
        {
            this.availableSkillsets = availableSkillsets;
            EditorType = EditorType.New;
        }

        /// <summary>
        /// Used by controller to edit a task
        /// </summary>
        /// <param name="availableSkillsets"></param>
        /// <param name="task"></param>
        public TaskEditorViewModel(List<Skillset> availableSkillsets, ProductionTask task)
        {
            this.availableSkillsets = availableSkillsets;
            EditorType = EditorType.Edit;

            Id = task.Id;
            Name = task.Name;
            Description = task.Description;
            Duration = task.Duration;
            requiredSkillsets = task.RequiredSkillsets;
        }

        /// <summary>
        /// Used by controller when an error occurred
        /// </summary>
        /// <param name="availableSkillsets"></param>
        /// <param name="viewModel"></param>
        /// <param name="errorMessage"></param>
        public TaskEditorViewModel(List<Skillset> availableSkillsets, TaskEditorViewModel viewModel, EditorType editorType, string errorMessage)
        {
            this.availableSkillsets = availableSkillsets;
            EditorType = editorType;

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
