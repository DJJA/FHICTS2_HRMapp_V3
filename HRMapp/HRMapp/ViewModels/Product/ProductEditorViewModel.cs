using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HRMapp.Models;

namespace HRMapp.ViewModels
{
    public class ProductEditorViewModel : EditorViewModel
    {
        private List<ProductionTask> tasks = new List<ProductionTask>();

        public override string FormTitle
        {
            get
            {
                switch (EditorType)
                {
                    case EditorType.New:
                        return "Nieuw product toevoegen";
                    case EditorType.Edit:
                        return "Product bewerken";
                    default:
                        throw new ArgumentOutOfRangeException(); // TODO Zet hier een exception message in
                }
            }
        }

        [DisplayName("Naam:")]
        public string Name { get; set; }
        [DisplayName("Omschrijving:")]
        public string Description { get; set; }

        public List<TaskEditorViewModel> TaskEditorViewModels
        {
            get
            {
                var list = new List<TaskEditorViewModel>();

                list.AddRange(from ProductionTask task in tasks select new TaskEditorViewModel(new List<Skillset>(), task));

                return list;
            }
        }
        public List<ProductionTask> Tasks
        {
            get
            {
                var list = new List<ProductionTask>();

                list.AddRange(tasks);

                return list;
            }
        }

        /// <summary>
        /// Used for giving back a viewmodel and used for new product
        /// </summary>
        public ProductEditorViewModel()
        {
            EditorType = EditorType.New;
        }

        /// <summary>
        /// Used for editing a product
        /// </summary>
        /// <param name="product"></param>
        public ProductEditorViewModel(Product product, List<ProductionTask> tasks)
        {
            EditorType = EditorType.Edit;

            Id = product.Id;
            Name = product.Name;
            Description = product.Description;

            this.tasks = tasks;
        }

        public Product ToProduct()
        {
            return new Product(Id, Name, Description, new List<ProductionTask>());
        }
    }
}
