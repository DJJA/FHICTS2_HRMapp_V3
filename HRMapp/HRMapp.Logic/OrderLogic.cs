using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRMapp.DAL.Repositories;
using HRMapp.Models;

namespace HRMapp.Logic
{
    public class OrderLogic : IOrderLogic
    {
        private OrderRepo repo;

        public OrderLogic(OrderRepo repo)
        {
            this.repo = repo;
        }

        public List<Order> GetAll() => repo.GetAll().ToList();
        public Order GetById(int id) => repo.GetById(id);
        public int Add(Order order)
        {
            order.SalesManager = new SalesManager(3);   // TODO Do this via logged in user, limit functionality with interfaces
            return repo.Add(order);
        }

        public void Update(Order order) => repo.Update(order);
    }
}
