using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRMapp.DAL.Repositories;
using HRMapp.Models;

namespace HRMapp.Logic
{
    public class ProductLogic : IProductLogic
    {
        private ProductRepo repo;

        public ProductLogic(ProductRepo repo)
        {
            this.repo = repo;
        }

        public List<Product> GetAll() => repo.GetAll().ToList();
        public Product GetById(int id) => repo.GetById(id);
        public int Add(Product product) => repo.Add(product);
        public void Update(Product product) => repo.Update(product);
    }
}
