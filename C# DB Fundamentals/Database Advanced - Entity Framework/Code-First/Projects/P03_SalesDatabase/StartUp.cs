using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data;
using P03_SalesDatabase.Data.Models;
using System;

namespace P03_SalesDatabase
{
    class StartUp
    {
        static void Main(string[] args)
        {
            DbContextOptionsBuilder<SalesContext> optionsBuilder = ConfiguringTheOptions();

            using (SalesContext context = new SalesContext(optionsBuilder.Options))
            {
                ResetDatabase(context);
            }
        }

        private static void ResetDatabase(SalesContext context)
        {
            context.Database.EnsureDeleted();

            context.Database.Migrate();

            Seed(context);
        }

        private static void Seed(SalesContext context)
        {
            var stores = new[] 
            {
                new Store{Name = "Kaufland" },
                new Store{Name = "Lidl"},
                new Store{Name = "Homemax"}
            };
            context.Stores.AddRange(stores);
            context.SaveChanges();

            var customers = new[]
            {
                new Customer{Name = "Stoian", Email = "stoyan_shopov@abv.bg",CreditCardNumber = "6546754674234"},
                new Customer{Name = "Petyr", Email = "petko32@gmail.com"}
            };
            context.Customers.AddRange(customers);
            context.SaveChanges();

            var products = new[]
            {
                new Product {Name = "Bread",Quantity = 0.500m, Price = 0.90m, Description = "White bread"},
                new Product {Name = "Steak",Quantity = 0.900m, Price = 6.80m, Description = "Pork steak"},
                new Product {Name = "Crowbar",Quantity = 4.900m, Price = 12.50m, Description = "Crowbar for the garden"},
                new Product {Name = "Cake",Quantity = 1.000m, Price = 8.20m, Description = "Cake for a birthday"},
                new Product {Name = "Screwdriver",Quantity = 0.300m, Price = 9.99m, Description = "Screwdriver for small appliances"}
            };
            context.Products.AddRange(products);
            context.SaveChanges();

            var sales = new[]
            {
                new Sale {Date = DateTime.Now, Customer = customers[0], Product = products[1], Store = stores[0]},
                new Sale {Date = DateTime.Today.AddDays(-2), Customer = customers[0], Product = products[3], Store = stores[1]},
                new Sale {Date = DateTime.Today.AddDays(-6), Customer = customers[1], Product = products[2], Store = stores[2]},
                new Sale {Date = DateTime.Now, Customer = customers[1], Product = products[4], Store = stores[2]},
                new Sale {Date = DateTime.Now.AddDays(-3), Customer = customers[0], Product = products[0], Store = stores[0]},
                new Sale {Date = DateTime.Now, Customer = customers[1], Product = products[1], Store = stores[1]},
            };
            context.Sales.AddRange(sales);
            context.SaveChanges();
        }


        private static DbContextOptionsBuilder<SalesContext> ConfiguringTheOptions()
        {
            DbContextOptionsBuilder<SalesContext> optionsBuilder = new DbContextOptionsBuilder<SalesContext>();
            optionsBuilder.UseSqlServer(Configuration.SqlConnectionStr);
            return optionsBuilder;
        }
    }
}
