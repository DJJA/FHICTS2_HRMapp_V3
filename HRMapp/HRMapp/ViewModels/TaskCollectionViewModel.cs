using HRMapp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMapp.ViewModels
{
    public class TaskCollectionViewModel
    {
        private List<ProductionTask> tasks;
        private int selectedItemId;

        public List<SelectListItem> ListItems { get; private set; }
        public List<ProductionTask> Tasks
        {
            get { return tasks; }
            private set
            {
                tasks = value;

                ListItems = new List<SelectListItem>();
                foreach (var task in tasks)
                {
                    ListItems.Add(new SelectListItem() { Text = task.Name, Value = task.Id.ToString(), Selected = (task.Id == selectedItemId) });
                }
            }
        }

        public TaskCollectionViewModel(int selectedItemId, List<ProductionTask> tasks)
        {
            this.selectedItemId = selectedItemId;
            Tasks = tasks;
        }
    }
}
