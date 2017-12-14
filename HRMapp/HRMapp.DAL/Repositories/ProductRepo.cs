using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.DAL.Contexts;
using HRMapp.Models;

namespace HRMapp.DAL.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private IProductContext context;

        public ProductRepo(ContextType contextType)
        {
            switch (contextType)
            {
                case ContextType.Mssql:
                    context = new MssqlProductContext();
                    break;
                default: throw new NotImplementedException();
            }
        }

        public IEnumerable<Product> GetAll() => context.GetAll();
        public Product GetById(int id) => context.GetById(id);
        public int Add(Product product) => context.Add(product);
        public void Update(Product product) => context.Update(product);
    }
}
