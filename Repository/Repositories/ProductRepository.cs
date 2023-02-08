using Entities;
using PanacealogicsSales.Contracts;
using PanacealogicsSales.Entities.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanacealogicsSales.Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
