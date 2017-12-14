using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMapp.Models
{
    public class Order
    {
        private string customer;
        public int Id { get; private set; }
        public SalesManager SalesManager { get; set; }  
        public DateTime Deadline { get; private set; }
        public DateTime EntryDate { get; private set; }

        public string Customer
        {
            get => customer;
            private set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("De klantnaam moet ingevuld worden.");
                }

                customer = value;
            }
        }
        public List<OrderItem> Items { get; private set; }

        public Order(int id, SalesManager salesManager, DateTime deadline, DateTime entryDate, string customer, List<OrderItem> items)
        {
            Id = id;
            SalesManager = salesManager;
            Deadline = deadline;
            EntryDate = entryDate;
            Customer = customer;
            Items = items;
        }

        public Order(int id, DateTime deadline, string customer, List<OrderItem> items)
        {
            Id = id;
            Deadline = deadline;
            Customer = customer;
            Items = items;
        }

        public override string ToString()
        {
            return $"{Id} - {Customer} - {Deadline.ToString("ddd dd-MMM-yy")}";
        }
    }
}
