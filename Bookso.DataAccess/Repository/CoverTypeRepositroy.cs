using Bookso.DataAccess.Repository.IRepository;
using Bookso.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookso.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        //public void Attach(Category obj)
        //{
        //    if(_db.Entry(obj).State == Microsoft.EntityFrameworkCore.EntityState.Detached)
        //    {
        //        _db.Categories.Attach(obj);
        //    }

        //    _db.Entry(obj).State = EntityState.Modified;
        //}

        public void Attach(Category obj)
        {
            _db.Attach(obj);
        }

        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
