namespace ProductShop.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class Product
    {
        public Product()
        {
        }

        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        public int SellerId { get; set; }
        public User Seller { get; set; }

        public int? BuyerId { get; set; }
        public User Buyer { get; set; }

        public ICollection<CategoryProduct> CategoryProducts { get; set; } = new HashSet<CategoryProduct>();
    }
}