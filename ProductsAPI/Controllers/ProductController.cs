using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProductsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ProductController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            return Ok(await GetProducts());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Product>>> GetSingleProduct(int id)
        {
            var product = await _dataContext.Products.Include(p => p.Category).SingleOrDefaultAsync(x => x.Id == id);

            if(product == null)
            {
                return NotFound("No Product");
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<List<Product>>> PostProduct(Product product)
        {
            if (product == null)
            {
                return BadRequest("Please provide product");
            }

            _dataContext.Products.Add(product);
            await _dataContext.SaveChangesAsync();
            return Ok(await GetProducts());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Product>>> UpdateProduct(Product product, int id)
        {
            var dbProduct = await _dataContext.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (dbProduct == null)
            {
                return NotFound("Product not found");
            }

            dbProduct.Title = product.Title;
            dbProduct.Description = product.Description;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Price = product.Price;
            dbProduct.ImageUrl = product.ImageUrl;

            await _dataContext.SaveChangesAsync();
            return await GetProducts();
        }

        [HttpGet("category")]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            return Ok(await _dataContext.Categories.ToListAsync());
        }



        [HttpPost("category")]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _dataContext.Categories.Add(category);
            await _dataContext.SaveChangesAsync();
            return category;
        }

        [HttpPut("category/{id}")]
        public async Task<ActionResult<List<Category>>> UpdateCategory(Category category, int id)
        {
            var dbCategory = await _dataContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (dbCategory == null)
            {
                return NotFound("Category Not Found");
            }

            dbCategory.Name = category.Name;
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.Categories.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Product>>> DeleteProduct(int id)
        {
            var product = await _dataContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound("Product Not Found");
            }

            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
            return Ok(await GetProducts());
        }

        [HttpGet("inventory")]
        public async Task<ActionResult<List<Inventory>>> GetInventory()
        {
            return Ok(await GetFullInventory());
        }

        [HttpPost("inventory")]
        public async Task<ActionResult<List<Inventory>>> CreateInventory(Inventory inventory)
        {
            var dbInventory = await _dataContext.Inventory.Include(i => i.Product).FirstOrDefaultAsync(i => i.Product.Id == inventory.ProductId);
            if (dbInventory == null)
            {
                dbInventory = inventory;
                _dataContext.Inventory.Add(dbInventory);
            } else
            {
                dbInventory.Count = inventory.Count;
            }

            await _dataContext.SaveChangesAsync();
            return Ok(await GetFullInventory());
        }

        private async Task<List<Product>> GetProducts()
        {
            return await _dataContext.Products.Include(p => p.Category).ToListAsync();
        }


        private async Task<List<Inventory>> GetFullInventory()
        {
            return await _dataContext.Inventory.Include(i => i.Product).Include(i => i.Product.Category).ToListAsync();
        }

    }
}
