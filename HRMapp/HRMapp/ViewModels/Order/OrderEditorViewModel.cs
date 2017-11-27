using HRMapp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMapp.ViewModels
{
    public class OrderEditorViewModel : EditorViewModel
    {
        private List<Product> products;
        public override string FormTitle
        {
            get
            {
                switch (EditorType)
                {
                    case EditorType.New:
                        return "Nieuwe order toevoegen";
                    case EditorType.Edit:
                        return "Order bewerken";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public List<SelectListItem> Products
        {
            get
            {
                var list = new List<SelectListItem>();
                foreach (var product in products)
                {
                    list.Add(new SelectListItem() { Text = product.Name, Value = product.Id.ToString() });
                }
                return list;
            }
        }

        [DisplayName("Deadline:")]
        public DateTime Deadline { get; set; }
        [DisplayName("Klant:")]
        public string Customer { get; set; }

        /// <summary>
        /// Used for giving back a viewmodel and used for new skillset
        /// </summary>
        public OrderEditorViewModel(List<Product> products)
        {
            EditorType = EditorType.New;
            this.products = products;
        }

        /// <summary>
        /// Used for editing a skillset
        /// </summary>
        /// <param name="order"></param>
        public OrderEditorViewModel(Order order, List<Product> products)
        {
            EditorType = EditorType.Edit;
            this.products = products;

            Id = order.Id;
            Deadline = order.DeadLine;
            Customer = order.Customer;
        }

        public Order ToOrder()
        {
            return new Order(Id, Deadline, Customer);
        }
    }
}
