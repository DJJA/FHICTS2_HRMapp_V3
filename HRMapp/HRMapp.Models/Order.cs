using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.Models
{
    public class Order
    {
        public int Id { get; private set; }
        public int SalesManagerId { get; private set; }  // TODO Wanneer sla je een Id op en wanneer een heel object?
        public DateTime DeadLine { get; private set; }
        public DateTime EntryDate { get; private set; }
        public string Customer { get; private set; }

        public Order(int id, int salesManagerId, DateTime deadLine, DateTime entryDate, string customer)
        {
            Id = id;
            SalesManagerId = salesManagerId;
            DeadLine = deadLine;
            EntryDate = entryDate;
            Customer = customer;
        }

        public Order(int id, DateTime deadLine, string customer)
        {
            Id = id;
            DeadLine = deadLine;
            Customer = customer;
        }

        public override string ToString()
        {
            return $"{Id} - {Customer} - {DeadLine.ToString("ddd dd-MMM-yy")}";
        }
    }
}
