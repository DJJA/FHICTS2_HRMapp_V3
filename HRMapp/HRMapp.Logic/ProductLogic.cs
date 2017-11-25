using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRMapp.DAL.Repositories;
using HRMapp.Models;

namespace HRMapp.Logic
{
    public class ProductLogic
    {
        ProductRepo repo = new ProductRepo();

        public List<Product> GetAll => repo.GetAll.ToList();
        public Product GetById(int id) => repo.GetById(id);
        public int Add(Product product) => repo.Add(product);
        public bool Update(Product product) => repo.Update(product);
    }
}
