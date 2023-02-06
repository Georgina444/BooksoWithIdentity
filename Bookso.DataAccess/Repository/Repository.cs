﻿using Bookso.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bookso.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        // Implementing the ApplicationDbContext in repository just like in the controller
        // and we will use that db to perform the CRUD operations
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            // _db.Products.Include(u => u.Category).Include(u=>u.CoverType);
            this.dbSet = _db.Set<T>();   // sets the DbSet to the particular instance of the class that is calling our repository
        }

        public void Add(T entity)
        {

            dbSet.Add(entity);        // same as: // _db.Categories.Add(obj/entity);

        }

        // includProp - "Category,CoverType"
        public IEnumerable<T> GetAll(string? includeProperties)
        {

            IQueryable<T> query = dbSet;     // IQueriable because we have to filter our data
            if (includeProperties != null)
            {
                foreach(var includeProp in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {

            dbSet.Remove(entity);

        }

        public void RemoveRange(IEnumerable<T> entity)
        {

            dbSet.RemoveRange(entity);
        }

/*        public void Attach(T entity)
        {
            dbSet.Attach(entity);
        }*/
    }
}