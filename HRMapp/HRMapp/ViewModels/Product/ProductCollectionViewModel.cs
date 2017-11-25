using HRMapp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMapp.ViewModels
{
    public class ProductCollectionViewModel
    {
        public string InfoMessage { get; set; }
        private List<Product> products;
        private int selectedItemId;

        public List<SelectListItem> ListItems { get; private set; }
        public List<Product> Products
        {
            get { return products; }
            private set
            {
                products = value;

                ListItems = new List<SelectListItem>();
                foreach (var product in products)
                {
                    ListItems.Add(new SelectListItem() { Text = product.Name, Value = product.Id.ToString(), Selected = (product.Id == selectedItemId) });
                }
            }
        }

        public ProductCollectionViewModel(int selectedItemId, List<Product> products)
        {
            this.selectedItemId = selectedItemId;
            Products = products;
        }
    }
}
