namespace ProductShop.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        public User()
        {
            this.ProductsSold = new List<Product>();
            this.ProductsBought = new List<Product>();
        }

        public int Id { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        public int? Age { get; set; }

        public ICollection<Product> ProductsSold { get; set; } = new HashSet<Product>();

        public ICollection<Product> ProductsBought { get; set; } = new HashSet<Product>();

        [NotMapped]
        [JsonProperty("seller")]
        public string FullName { get { return (this.FirstName + " " + this.LastName); } }


    }
}