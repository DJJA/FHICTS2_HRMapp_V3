using HRMapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMapp.ViewModels
{
    public class SkillsetSelectorViewModel
    {
        public List<Skillset> availableSkillsets = new List<Skillset>();
        public List<Skillset> requiredSkillsets = new List<Skillset>();
        // TODO create constructor or something like it to set available and required skillsets

        public List<SelectListItem> AvailableSkillsetListItems
        {
            get
            {
                var list = new List<SelectListItem>();
                foreach (var skillset in availableSkillsets)
                {
                    if (requiredSkillsets.All(s => s.Id != skillset.Id))      // Not optimal n^2
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
        
        public List<int> LboxRequiredSkillsets { get; set; }
    }
}
