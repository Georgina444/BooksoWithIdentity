using Bookso.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bookso.DataAccess.Repository.IRepository
{
    // This is a generic repository where T is a class
    public interface IRepository<T> where T : class
    {
        // T- Category
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true);
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null, string? includeProperties = null);
  
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);   // in case you want to remove more than one entity
        void Attach(T entity);
        // we are receiving those entities as IEnumerable which is a collection of entities


    }
}
