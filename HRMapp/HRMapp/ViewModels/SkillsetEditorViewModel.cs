using HRMapp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HRMapp.ViewModels
{
    public class SkillsetEditorViewModel : EditorViewModel
    {
        public override string FormAction
        {
            get
            {
                switch (EditorType)
                {
                    case EditorType.New:
                        return "New";
                    case EditorType.Edit:
                        return "Edit";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override string FormTitle
        {
            get
            {
                switch (EditorType)
                {
                    case EditorType.New:
                        return "Nieuwe skillset toevoegen";
                    case EditorType.Edit:
                        return "Skillset bewerken";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        [DisplayName("Naam:")]
        public string Name { get; set; }
        [DisplayName("Omschrijving:")]
        public string Description { get; set; }

        public SkillsetEditorViewModel()
        {
            EditorType = EditorType.New;
        }

        public SkillsetEditorViewModel(Skillset skillset)
        {
            EditorType = EditorType.Edit;

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
