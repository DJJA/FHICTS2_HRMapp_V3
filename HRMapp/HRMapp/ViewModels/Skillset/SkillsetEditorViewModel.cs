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

        /// <summary>
        /// Used for giving back a viewmodel and used for new skillset
        /// </summary>
        public SkillsetEditorViewModel()
        {
            EditorType = EditorType.New;
        }

        /// <summary>
        /// Used for editing a skillset
        /// </summary>
        /// <param name="skillset"></param>
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
