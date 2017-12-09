using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.DAL.Contexts;
using HRMapp.Models;

namespace HRMapp.DAL.Repositories
{
    public class ProductRepo
    {
        IProductContext context = new MssqlProductContext();

        public IEnumerable<Product> GetAll => context.GetAll();
        public Product GetById(int id) => context.GetById(id);
        public int Add(Product product) => context.Add(product);
        public void Update(Product product) => context.Update(product);
    }
}
