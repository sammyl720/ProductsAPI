namespace ProductsAPI.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public int Count { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
