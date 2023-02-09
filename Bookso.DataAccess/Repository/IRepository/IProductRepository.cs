using Bookso.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookso.DataAccess.Repository.IRepository
{
    public interface IProductRepositroy : IRepository<Product>
    {
        //void Attach(Product obj);
        void Update(Product obj);

    }
}
