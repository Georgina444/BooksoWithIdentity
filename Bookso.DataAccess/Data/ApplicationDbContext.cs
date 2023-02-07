using Bookso.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bookso.DataAccess;

public class ApplicationDbContext : IdentityDbContext
{
    // General syntax needed to establish the conneciton with EF (configures our DbContext)
    // (parameters) -> passing them to the base class
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Category> Categories { get; set; }   // Creates table Categories
    public DbSet<CoverType> CoverTypes { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ShoppingCart> ShopppingCarts { get; set; }

}
// We are using EF Core through ApplicationDbContext
// We can access the db object cs in here we have the tables




// ApplicationDbContext is a custom class used to interact with the database(it is a subclass of DbContext meaning it is derived from the DbContext)


// Custom class - user defined class that is created to meet specific needs. It extands the functionality of existing classes or libraries
// by adding custom properties, methods(it will enherit the properties, method and behaviours of the )