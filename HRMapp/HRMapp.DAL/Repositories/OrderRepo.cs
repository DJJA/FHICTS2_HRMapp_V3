using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.DAL.Contexts;
using HRMapp.Models;

namespace HRMapp.DAL.Repositories
{//TODO Repo context voor generalisatie
    public class OrderRepo
    {
        private IOrderContext context = new MssqlOrderContext();

        public IEnumerable<Order> GetAll => context.GetAll();
        public Order GetById(int id) => context.GetById(id);
        public int Add(Order order) => context.Add(order);
        public bool Update(Order order) => context.Update(order);
    }
}
