using HRMapp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HRMapp.ViewModels
{
    public class SkillsetEditorViewModel
    {
        public string ErrorMessage { get; set; }
        public string FormAction { get; private set; }
        public string FormTitle { get; private set; }
        public int Id { get; set; }
        [DisplayName("Naam:")]
        public string Name { get; set; }
        [DisplayName("Omschrijving:")]
        public string Description { get; set; }

        public SkillsetEditorViewModel()
        {
            FormAction = "New";
            FormTitle = "Nieuwe skillset toevoegen";
        }

        public SkillsetEditorViewModel(Skillset skillset)
        {
            FormAction = "Edit";
            FormTitle = "Skillset bewerken";

            Id = skillset.Id;
            Name = skillset.Name;
            Description = skillset.Description;
        }

        public Skillset ToSkillset()
        {
            return new Skillset(Id, Name, Description);
        }
    }
}
