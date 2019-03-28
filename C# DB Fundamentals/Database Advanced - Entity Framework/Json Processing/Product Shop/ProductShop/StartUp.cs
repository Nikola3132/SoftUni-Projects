using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (ProductShopContext context = new ProductShopContext())
            {
                //Console.WriteLine(ImportUsers(context, File
                //.ReadAllText(@"C:\Users\nikolaviktor3132\Desktop\Shop\ProductShop\Datasets\users.json")));

                //Console.WriteLine(ImportProducts(context, File
                //.ReadAllText(@"C:\Users\nikolaviktor3132\Desktop\Shop\ProductShop\Datasets\products.json")));

                //Console.WriteLine(ImportCategories(context, File
                //.ReadAllText(@"C:\Users\nikolaviktor3132\Desktop\Shop\ProductShop\Datasets\categories.json")));

                //Console.WriteLine(ImportCategoryProducts(context, File
                //.ReadAllText(@"C:\Users\nikolaviktor3132\Desktop\Shop\ProductShop\Datasets\categories-products.json")));

                //Console.WriteLine(GetProductsInRange(context));

                //Console.WriteLine(GetSoldProducts(context));

               // Console.WriteLine(GetCategoriesByProductsCount(context));

               // Console.WriteLine(GetUsersWithProducts(context));
            }


        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IEnumerable<User> users = JsonConvert.DeserializeObject<IEnumerable<User>>(inputJson);

            context.Users.AddRange(users);

            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IEnumerable<Product> products = JsonConvert.DeserializeObject<IEnumerable<Product>>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();
            return $"Successfully imported {products.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IEnumerable<Category> categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(inputJson).Where(e=>e.Name != null);

            context.Categories.AddRange(categories);
            context.SaveChanges();
            return $"Successfully imported {categories.Count()}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            IEnumerable<CategoryProduct> categoriesProducts = JsonConvert.DeserializeObject<IEnumerable<CategoryProduct>>(inputJson);

            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();
            return $"Successfully imported {categoriesProducts.Count()}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {

            var products = context.Products.Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p=> new
            {
                name = p.Name,
                price = p.Price,
                seller = p.Seller.FullName
            })
            .OrderBy(p => p.price)
            .ToList();

            var json = JsonConvert.SerializeObject(products, Formatting.Indented);
           
            return json;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p=>p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold.Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        buyerFirstName = p.Buyer.FirstName,
                        buyerLastName = p.Buyer.LastName
                    })
                })
                .ToList();

            var json = JsonConvert.SerializeObject(users, Formatting.Indented);

            return json;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {

            var categories = context.Categories
               .OrderByDescending(c => c.CategoryProducts.Count)
               .Select(x => new
               {
                   Category = x.Name,
                   ProductsCount = x.CategoryProducts.Count,
                   AveragePrice = $"{x.CategoryProducts.Average(c => c.Product.Price):F2}",
                   TotalRevenue = $"{x.CategoryProducts.Sum(c => c.Product.Price)}"
               })
               .ToList();

            string json = JsonConvert.SerializeObject(categories,
                new JsonSerializerSettings()
                {
                    ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new CamelCaseNamingStrategy(),
                    },

                    Formatting = Formatting.Indented
                }
            );

            return json;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users.Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderByDescending(u => u.ProductsSold.Count(p => p.Buyer != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold
                                .Count(p => p.Buyer != null),
                        products = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Select(p => new
                            {
                                name = p.Name,
                                price = p.Price,
                            })
                }
                });

            var json = JsonConvert.SerializeObject(new { usersCount = users.Count(), users }
            , new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
               ,Formatting = Formatting.Indented
            });

            return json;
        }
    }
}