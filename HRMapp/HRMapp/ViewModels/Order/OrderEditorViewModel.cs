using HRMapp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
                    if(Items.All(i => i.Product.Id != product.Id))
                        list.Add(new SelectListItem() { Text = product.Name, Value = product.Id.ToString() });
                }
                return list;
            }
        }

        public string OrderedProducts { get; set; }

        private struct OrderItemHelper
        {
            public int ProductId, Amount;
        }

        private List<OrderItem> OrderItems
        {
            get
            {
                var deserializedObjects = JsonConvert.DeserializeObject<List<OrderItemHelper>>(OrderedProducts);
                var orderItems = new List<OrderItem>();
                orderItems.AddRange(from OrderItemHelper helper in deserializedObjects select new OrderItem(){Product = new Product(helper.ProductId), Amount = helper.Amount});
                return orderItems;
            }
        }

        [DisplayName("Deadline:")]
        public DateTime Deadline { get; set; }
        [DisplayName("Klant:")]
        public string Customer { get; set; }

        public List<OrderItem> Items { get; private set; }

        /// <summary>
        /// Used for giving back a viewmodel
        /// </summary>
        public OrderEditorViewModel()
        {
            EditorType = EditorType.New;
            Items = new List<OrderItem>();
        }

        /// <summary>
        /// Used for giving back a viewmodel and used for new skillset todo change summary
        /// </summary>
        public OrderEditorViewModel(List<Product> products)
        {
            EditorType = EditorType.New;
            this.products = products;
            Items = new List<OrderItem>();
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
            Deadline = order.Deadline;
            Customer = order.Customer;
            Items = order.Items;
        }

        public Order ToOrder()
        {
            return new Order(
                id: Id,
                deadline: Deadline,
                customer: Customer,
                items: OrderItems
                );
        }
    }
}
