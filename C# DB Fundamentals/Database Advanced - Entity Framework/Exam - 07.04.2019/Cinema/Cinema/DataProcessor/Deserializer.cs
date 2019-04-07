namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Cinema.Data.Models;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat 
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var movieDtos = JsonConvert
                .DeserializeObject<ImportMovieDto[]>(jsonString);

            List<Movie> movies = new List<Movie>();

            foreach (var dto in movieDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var movie = movies.FirstOrDefault(m => m.Title == dto.Title);

                if (movie == null)
                {
                    movie = new Movie()
                    {
                        Director = dto.Director,
                        Duration = dto.Duration,
                        Genre = dto.Genre,
                        Rating = dto.Rating,
                        Title = dto.Title
                    };

                    movies.Add(movie);
                    sb.AppendLine($"Successfully imported {movie.Title} with genre {movie.Genre.ToString()} and rating {movie.Rating:f2}!");
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            context.AddRange(movies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var hallDtos = JsonConvert.DeserializeObject<ImportHallsSeatDto[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            var halls = new List<Hall>();

            foreach (var dto in hallDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var hall = new Hall()
                {
                    Is3D = dto.Is3D,
                    Is4Dx = dto.Is4Dx,
                    Name = dto.Name
                };

                for (int i = 0; i < dto.Seats; i++)
                {
                    hall.Seats.Add(new Seat()
                    {
                        Hall = hall
                    });
                }

                halls.Add(hall);

                var projectionType = string.Empty;

                if (hall.Is3D && hall.Is4Dx)
                {
                    projectionType = "4Dx/3D";
                }
                else if (hall.Is3D)
                {
                    projectionType = "3D";
                }
                else if (hall.Is4Dx)
                {
                    projectionType = "4Dx";
                }
                else
                {
                    projectionType = "Normal";
                }
                sb.AppendLine($"Successfully imported {hall.Name}({projectionType}) with {dto.Seats} seats!");
            }
            context.AddRange(halls);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(ImportProjectDto[])
                , new XmlRootAttribute("Projections"));

            var projectionsDto = (ImportProjectDto[])serializer.Deserialize(new StringReader(xmlString));

            var projects = new List<Projection>();

            var dbHallIds = context.Halls.Select(h => h.Id).ToArray();
            var dbMovies = context.Movies.Select(m => new
            {
                m.Id,
                m.Title
            }).ToArray();

            foreach (var dto in projectionsDto)
            {
                if (!dbHallIds.Any(h=>h == dto.HallId) 
                    ||!dbMovies.Any(m => m.Id == dto.MovieId)
                    ||!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

               // string validDateTime = dto.DateTime.Replace('-', '/');

                var project = new Projection()
                {
                    DateTime =DateTime.ParseExact(dto.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                    HallId = dto.HallId,
                    MovieId = dto.MovieId
                };

                projects.Add(project);
                var currentMovie = dbMovies.FirstOrDefault(m => m.Id == project.MovieId);

                sb.AppendLine($"Successfully imported projection {currentMovie.Title} on {project.DateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}!");
            }

            context.Projections.AddRange(projects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportCustomerDto[])
                , new XmlRootAttribute("Customers"));

            var sb = new StringBuilder();

            var customerDtos = (ImportCustomerDto[])serializer.Deserialize(new StringReader(xmlString));

            var customers = new List<Customer>();

            foreach (var dto in customerDtos)
            {
                if (!IsValid(dto) || !dto.Tickets.All(IsValid))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var customer = new Customer()
                {
                    Age = dto.Age,
                    Balance = dto.Balance,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Tickets = dto.Tickets.Select(t => new Ticket
                    {
                        Price = t.Price,
                        ProjectionId = t.ProjectionId
                    }).ToArray()
                };

                customers.Add(customer);
                sb.AppendLine($"Successfully imported customer {customer.FirstName} {customer.LastName} with bought tickets: {customer.Tickets.Count}!");
            };

            context.AddRange(customers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            var isValid 
                = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            return isValid;
        }
    }
}