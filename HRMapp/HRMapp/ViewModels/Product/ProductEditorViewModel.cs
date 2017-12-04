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
        public List<ProductionTask> Tasks { get; private set; }

        /// <summary>
        /// Used for giving back a viewmodel and used for new product
        /// </summary>
        public ProductEditorViewModel()
        {
            EditorType = EditorType.New;
            Tasks = new List<ProductionTask>();
        }

        /// <summary>
        /// Used for editing a product
        /// </summary>
        /// <param name="product"></param>
        public ProductEditorViewModel(Product product)
        {
            EditorType = EditorType.Edit;

            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Tasks = product.Tasks;
        }

        public Product ToProduct()
        {
            return new Product(Id, Name, Description);
        }
    }
}
