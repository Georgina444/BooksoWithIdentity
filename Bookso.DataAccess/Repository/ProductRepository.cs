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
    public class ProductRepository : Repository<Product>, IProductRepositroy
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


/*        public override void Attach(Product obj)
        {
            _db.Attach(obj);
        }
*/
        public void Update(Product obj)
        {
            // _db.Products.Update(obj);   updates all the other properties even if only one is being edited

            var objFromDb = _db.Products.FirstOrDefault(u => u.ProductId == obj.ProductId);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Description = obj.Description;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Price = obj.Price;
                objFromDb.Price2 = obj.Price2;
                objFromDb.Author = obj.Author;
                objFromDb.CoverTypeId = obj.CoverTypeId;
                if(obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;    // cs if you don't update the image in that session, and you already had an image - it will be gone
                }
            }
        }
    }
}
