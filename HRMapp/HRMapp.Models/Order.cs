using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.Models
{
    public class Order
    {
        public int Id { get; private set; }
        public SalesManager SalesManager { get; private set; }  // TODO Wanneer sla je een Id op en wanneer een heel object?
        public DateTime Deadline { get; private set; }
        public DateTime EntryDate { get; private set; }
        public string Customer { get; private set; }

        public Order(int id, SalesManager salesManager, DateTime deadline, DateTime entryDate, string customer)
        {
            Id = id;
            SalesManager = salesManager;
            Deadline = deadline;
            EntryDate = entryDate;
            Customer = customer;
        }

        public Order(int id, DateTime deadline, string customer)
        {
            Id = id;
            Deadline = deadline;
            Customer = customer;
        }

        public override string ToString()
        {
            return $"{Id} - {Customer} - {Deadline.ToString("ddd dd-MMM-yy")}";
        }
    }
}
