namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Cinema.Data.Models;
    using Cinema.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var json = string.Empty;

            var movies = context.Movies
                .Where(m => m.Rating >= rating && m.Projections
                .Any(p => p.Tickets.Count >= 1))
                .Select(m => new
                {
                    MovieName = m.Title,
                    Rating = $"{m.Rating:f2}",
                    TotalIncomes
                    = $"{m.Projections.SelectMany(p => p.Tickets).Sum(t => t.Price):f2}",
                    Customers = m.Projections.SelectMany(p => p.Tickets).Select(t => new
                    {
                        FirstName = t.Customer.FirstName,
                        LastName = t.Customer.LastName,
                        Balance = $"{t.Customer.Balance:f2}"
                    })
                    .OrderByDescending(c => c.Balance)
                    .ThenBy(c => c.FirstName)
                    .ThenBy(c => c.LastName).ToArray()
                })
                .OrderByDescending(m => double.Parse(m.Rating))
                .ThenByDescending(m => decimal.Parse(m.TotalIncomes))
                .Take(10)
                .ToArray();

            json = JsonConvert.SerializeObject(movies, Newtonsoft.Json.Formatting.Indented);

            return json;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var serialzer = new XmlSerializer(typeof(List<ExportCustomerDto>)
                , new XmlRootAttribute("Customers"));

            var exportCustomerDtos = new List<ExportCustomerDto>();
            var customers = context.Customers.Where(c => c.Age >= age).ToList();
            
            foreach (var customer in customers)
            {
                string spendTime = FormatingAndSummingTheWholeDuration(customer);

                var custDto = new ExportCustomerDto()
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    SpentTime = spendTime,
                    SpentMoney = $"{customer.Tickets.Sum(t => t.Price):f2}"
                };

                exportCustomerDtos.Add(custDto);
               
            }

            exportCustomerDtos = exportCustomerDtos.OrderByDescending(c => decimal.Parse(c.SpentMoney)).Take(10).ToList();
            
            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });


            serialzer.Serialize(new StringWriter(sb), exportCustomerDtos,namespaces);

            return sb.ToString().TrimEnd();
        }

        private static string FormatingAndSummingTheWholeDuration(Customer customer)
        {
            var hours = 0;
            var minutes = 0;
            var seconds = 0;

            var customerTime = customer.Tickets
                .Select(t => t.Projection.Movie.Duration)
                .Select(d => new
            {
                Hours = d.Hours,
                Mins = d.Minutes,
                Secs = d.Seconds
            }).ToArray();

            hours = customerTime.Sum(h => h.Hours);

            foreach (var mins in customerTime)
            {
                minutes += mins.Mins;
                
                if (minutes >= 60)
                {
                    minutes -= 60;
                    hours += 1;
                }
                
            }

            foreach (var secs in customerTime)
            {
                seconds += secs.Secs;

                if (seconds >= 60)
                {
                    seconds -= 60;
                    minutes += 1;
                }
            }

            return $"{hours:d2}:{minutes:d2}:{seconds:d2}";
        }
    }


    

}
