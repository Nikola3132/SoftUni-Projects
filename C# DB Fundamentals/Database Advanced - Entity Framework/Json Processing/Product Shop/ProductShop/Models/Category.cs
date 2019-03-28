namespace ProductShop.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;


    public class Category
    {
        public Category()
        {

        }

        public int Id { get; set; }
        
        public string Name { get; set; }

        public ICollection<CategoryProduct> CategoryProducts { get; set; } = new List<CategoryProduct>();
    }
}
