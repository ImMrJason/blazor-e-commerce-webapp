namespace BlazorEcommerce.Server.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetProducts();

        Task<ServiceResponse<Product>> GetProduct(int productId);
    }

    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Product>>> GetProducts()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products.ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var response = new ServiceResponse<Product>();
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                response.Success = false;
                response.Message = "Product not found.";
                return response;
            }

            response.Data = product;
            return response;
        }
    }
}
