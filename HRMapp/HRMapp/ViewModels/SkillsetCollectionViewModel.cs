using HRMapp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMapp.ViewModels
{
    public class SkillsetCollectionViewModel
    {
        private List<Skillset> skillsets;
        private int selectedItemId;

        public List<SelectListItem> ListItems { get; private set; }
        public List<Skillset> Skillsets
        {
            get { return skillsets; }
            private set
            {
                skillsets = value;

                ListItems = new List<SelectListItem>();
                foreach (var skillset in skillsets)
                {
                    ListItems.Add(new SelectListItem() { Text = skillset.Name, Value = skillset.Id.ToString(), Selected = (skillset.Id == selectedItemId) });
                }
            }
        }

        public SkillsetCollectionViewModel(int selectedItemId, List<Skillset> skillsets)
        {
            this.selectedItemId = selectedItemId;
            Skillsets = skillsets;
        }
    }
}
