using HRMapp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace HRMapp.ViewModels
{
    public class OrderEditorViewModel : EditorViewModel
    {
        private List<Product> products = new List<Product>();
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

        public string OrderedProducts { get; set; }

        public Dictionary<int, int> OrderedProductsV2
        {
            get { return JsonConvert.DeserializeObject<Dictionary<int, int>>(OrderedProducts); }
        }

        [DisplayName("Deadline:")]
        public DateTime Deadline { get; set; }
        [DisplayName("Klant:")]
        public string Customer { get; set; }

        /// <summary>
        /// Used for giving back a viewmodel
        /// </summary>
        public OrderEditorViewModel()
        {
            EditorType = EditorType.New;
        }

        /// <summary>
        /// Used for giving back a viewmodel and used for new skillset todo change summary
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
