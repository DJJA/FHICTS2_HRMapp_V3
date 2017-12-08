using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.Models
{
    public struct OrderItem
    {
        public Product Product;
        public int Amount;
    }

    public class Order
    {
        public int Id { get; private set; }
        public SalesManager SalesManager { get; set; }  // TODO Wanneer sla je een Id op en wanneer een heel object?
        public DateTime Deadline { get; private set; }
        public DateTime EntryDate { get; private set; }
        public string Customer { get; private set; }
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
            Items = items;          //TODO Misschien lijsten niet in contructors instantieren, maar in de class body
        }

        public override string ToString()
        {
            return $"{Id} - {Customer} - {Deadline.ToString("ddd dd-MMM-yy")}";
        }
    }
}
