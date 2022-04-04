using Microsoft.EntityFrameworkCore;

namespace ProductsAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Precision(6)]
        public decimal Price { get; set; } = decimal.Zero;

        public string ImageUrl { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
