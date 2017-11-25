using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMapp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMapp.ViewModels
{
    public class OrderCollectionViewModel
    {
        public string InfoMessage { get; set; }
        private List<Order> orders;
        private int selectedItemId;

        public List<SelectListItem> ListItems { get; private set; }
        public List<Order> Orders
        {
            get => orders;
            private set
            {
                orders = value;

                ListItems = new List<SelectListItem>();
                foreach (var order in orders)
                {
                    ListItems.Add(new SelectListItem() { Text = order.ToString(), Value = order.Id.ToString(), Selected = (order.Id == selectedItemId) });
                }
            }
        }

        public OrderCollectionViewModel(int selectedItemId, List<Order> orders)
        {
            this.selectedItemId = selectedItemId;
            Orders = orders;
        }
    }
}
