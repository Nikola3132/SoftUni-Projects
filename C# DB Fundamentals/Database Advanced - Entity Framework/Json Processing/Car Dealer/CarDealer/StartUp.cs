using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                //string suppliesJson = File
                //    .ReadAllText(@"C:\Users\nikolaviktor3132\Desktop\CarDealer\CarDealer\Datasets\suppliers.json");

                //string partsJson = File
                //    .ReadAllText(@"C:\Users\nikolaviktor3132\Desktop\CarDealer\CarDealer\Datasets\parts.json");

                string carsJson = File
                    .ReadAllText(@"C:\Users\nikolaviktor3132\Desktop\CarDealer\CarDealer\Datasets\cars.json");

                //string customersJson = File
                //    .ReadAllText(@"C:\Users\nikolaviktor3132\Desktop\CarDealer\CarDealer\Datasets\customers.json");

                //string salesJson = File
                //   .ReadAllText(@"C:\Users\nikolaviktor3132\Desktop\CarDealer\CarDealer\Datasets\sales.json");

                //Console.WriteLine(ImportSuppliers(context, suppliesJson));
                //Console.WriteLine(ImportParts(context,partsJson));
                //Console.WriteLine(ImportCars(context,carsJson));
                //Console.WriteLine(ImportCustomers(context,customersJson));
                //Console.WriteLine(ImportSales(context,salesJson));
                //Console.WriteLine(GetOrderedCustomers(context));
                //Console.WriteLine(GetCarsFromMakeToyota(context));
                //Console.WriteLine(GetLocalSuppliers(context));
                 Console.WriteLine(GetCarsWithTheirListOfParts(context));
                //Console.WriteLine(GetTotalSalesByCustomer( context));
                //Console.WriteLine();
            }
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var supplies = JsonConvert.DeserializeObject<IEnumerable<Supplier>>(inputJson);

            context.Suppliers.AddRange(supplies);
            context.SaveChanges();

            return $"Successfully imported {supplies.Count()}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<IEnumerable<Part>>(inputJson
                , new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            int added = 0;
            foreach (var part in parts)
            {

                if (!context.Suppliers.Any(s => s.Id == part.SupplierId))
                {
                    continue;
                }
                context.Parts.Add(part);
                added++;

            }
            context.SaveChanges();

            return $"Successfully imported {added}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var cars = JsonConvert.DeserializeObject<CarInsertDto[]>(inputJson);
            var mappedCars = new List<Car>();

            foreach (var car in cars)
            {
                Car vehicle = Mapper.Map<CarInsertDto, Car>(car);
                mappedCars.Add(vehicle);

                var partIds = car
                .PartsId
                .Distinct()
                .ToList();

                if (partIds == null)
                    continue;

                partIds.ForEach(pid =>
                {
                    var currentPair = new PartCar()
                    {
                        Car = vehicle,
                        PartId = pid
                    };

                    vehicle.PartCars.Add(currentPair);
                }
                );

            }

            context.Cars.AddRange(mappedCars);

            context.SaveChanges();
            int affectedRows = context.Cars.Count();

            return $"Successfully imported {affectedRows}.";
        }


        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<IEnumerable<Customer>>(inputJson);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {context.Customers.Count()}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<IEnumerable<Sale>>(inputJson);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {context.Sales.Count()}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers.OrderBy(c => c.BirthDate).ThenBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture),
                    IsYoungDriver = c.IsYoungDriver
                });

            string json = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return json;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new
                {
                    c.Id,
                    c.Make,
                    c.Model,
                    c.TravelledDistance
                })
                .ToList();

            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return json;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var supplies = context.Suppliers.Where(s => s.IsImporter == false)
            .Select(s => new
            {
                s.Id,
                s.Name,
                PartsCount = s.Parts.Count()
            })
            .ToList();

            var json = JsonConvert.SerializeObject(supplies, Formatting.Indented);

            return json;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
               .Include(c => c.PartCars)
               .ThenInclude(c => c.Part)
               .Select(c => new
               {
                   car = new
                   {
                       Make = c.Make,
                       Model = c.Model,
                       TravelledDistance = c.TravelledDistance
                   },

                   parts = c.PartCars
                   .Select(p => new
                   {
                       Name = p.Part.Name,
                       Price = $"{p.Part.Price:F2}"
                   })
                   .ToList()
               })
               .ToList();

            var json = JsonConvert.SerializeObject(cars, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            });

            return json;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
              .Include(c => c.Sales)
              .ThenInclude(s => s.Car)
              .ThenInclude(c => c.PartCars)
              .ThenInclude(pc => pc.Part)
              .Where(c => c.Sales.Count >= 1)
              .Select(x => new
              {
                  FullName = x.Name,
                  BoughtCars = x.Sales.Count,
                  SpentMoney = x.Sales.Sum(y => y.Car.PartCars.Sum(z => z.Part.Price))
              })
              .ToList()
              .OrderByDescending(a => a.SpentMoney)
              .ThenBy(a => a.BoughtCars)
              .ToList();


            var json = JsonConvert.SerializeObject(customers, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            });

            return json;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .Select(x => new
                {
                    car = new
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TravelledDistance = x.Car.TravelledDistance
                    },

                    customerName = x.Customer.Name,
                    Discount = $"{x.Discount:F2}",
                    price = $"{x.Car.PartCars.Sum(y => y.Part.Price):F2}",
                    priceWithDiscount = $"{x.Car.PartCars.Sum(y => y.Part.Price) - (x.Car.PartCars.Sum(y => y.Part.Price) * (x.Discount / 100)):F2}",
                })
                .ToList();

            var json = JsonConvert.SerializeObject(sales, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                //ContractResolver = new DefaultContractResolver()
                //{
                //    NamingStrategy = new CamelCaseNamingStrategy()
                //}
            });

            return json;
        }
    }
}