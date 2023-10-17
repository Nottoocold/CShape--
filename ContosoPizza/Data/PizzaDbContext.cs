using Microsoft.EntityFrameworkCore;
using ContosoPizza.Models;

namespace ContosoPizza.Data
{
    public class PizzaDbContext : DbContext
    {
        public PizzaDbContext(DbContextOptions<PizzaDbContext> options) : base(options)
        {
        }

        // Add the DbSets for the Pizza, Topping, and Sauce models
        // The DbSet properties are used to query and save instances of Pizza, Topping, and Sauce
        // The name of the DbSet property is used as the table name in the database
        // The name of the DbSet property can be used as the model name in API endpoints
        public DbSet<Pizza> Pizzas => Set<Pizza>();
        public DbSet<Topping> Toppings => Set<Topping>();
        public DbSet<Sauce> Sauces => Set<Sauce>();
    }
}