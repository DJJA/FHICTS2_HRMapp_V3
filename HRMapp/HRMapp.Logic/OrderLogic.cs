﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRMapp.DAL.Repositories;
using HRMapp.Models;

namespace HRMapp.Logic
{
    public class OrderLogic
    {
        private OrderRepo repo = new OrderRepo();

        public List<Order> GetAll => repo.GetAll.ToList();
        public Order GetById(int id) => repo.GetById(id);
        public int Add(Order order) => repo.Add(order);
        public bool Update(Order order) => repo.Update(order);
    }
}