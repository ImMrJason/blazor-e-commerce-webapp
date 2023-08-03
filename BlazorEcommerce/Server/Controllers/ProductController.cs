using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private static List<Product> products = new List<Product>()
        {
            new Product()
            {
                Id = 1,
                Title = "Product 1",
                Description = "This is the description for product 1",
                ImageUrl = "https://via.placeholder.com/250",
                Price = 9.99M
            },
            new Product()
            {
                Id = 2,
                Title = "Product 2",
                Description = "This is the description for product 2",
                ImageUrl = "https://via.placeholder.com/250",
                Price = 19.99M
            },
            new Product()
            {
                Id = 3,
                Title = "Product 3",
                Description = "This is the description for product 3",
                ImageUrl = "https://via.placeholder.com/250",
                Price = 29.99M
            }
        };

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(products);
        }
    }
}