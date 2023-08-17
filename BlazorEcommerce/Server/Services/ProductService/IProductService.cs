namespace BlazorEcommerce.Server.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetProducts();

        Task<ServiceResponse<Product>> GetProduct(int productId);

        Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl);

        Task<ServiceResponse<ProductSearchResult>> SearchProducts(string searchText, int page);

        Task<ServiceResponse<List<string>>> GetProductSuggestions(string searchText);

        Task<ServiceResponse<List<Product>>> GetFeaturedProducts();
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
                Data = await _context.Products.Include(p => p.Variants).ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var response = new ServiceResponse<Product>();
            var product = await _context.Products
                .Include(p => p.Variants)
                .ThenInclude(v => v.ProductType)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                response.Success = false;
                response.Message = "Product not found.";
                return response;
            }

            response.Data = product;
            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                .Where(p => p.Category.Url.ToLower() == categoryUrl.ToLower())
                .Include(p => p.Variants)
                .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<ProductSearchResult>> SearchProducts(string searchText, int page)
        {
            var productsPerPage = 2f;
            var pageCount = Math.Ceiling((await FindProductsBySearchText(searchText)).Count / productsPerPage);
            var products = await _context.Products
                            .Where(p => p.Title.ToLower().Contains(searchText) || p.Description.ToLower().Contains(searchText))
                            .Include(p => p.Variants)
                            .Skip((page - 1) * (int)productsPerPage)
                            .Take((int)productsPerPage)
                            .ToListAsync();

            // find products where the title or description contains the search text
            var response = new ServiceResponse<ProductSearchResult>
            {
                Data = new ProductSearchResult
                {
                    Products = products,
                    CurrentPage = page,
                    PageCount = (int)pageCount
                }
            };

            return response;
        }

        // Note that our implementation here is based on the assumption that all product names and descriptions are in a Latin-based language
        // that uses the Latin alphabet.
        public async Task<ServiceResponse<List<string>>> GetProductSuggestions(string searchText)
        {
            var products = await FindProductsBySearchText(searchText);

            List<string> result = new List<string>();

            foreach (var product in products)
            {
                // only add the prduct title, if the title has the search text in it.
                if (product.Title.ToLower().Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(product.Title);
                }

                // if we have a description, create a list of words from the description.
                // for each word, if the word contains the search text, add it to the result.
                if (!string.IsNullOrEmpty(product.Description))
                {
                    var punctuation = product.Description.ToLower().Where(char.IsPunctuation).Distinct().ToArray();
                    // Split() means split on whitespace. the default is to split on whitespace and punctuation.
                    var words = product.Description.ToLower().Split().Select(w => w.Trim(punctuation));

                    foreach (var word in words)
                    {
                        if (word.ToLower().Contains(searchText, StringComparison.OrdinalIgnoreCase) && !result.Contains(word))
                        {
                            result.Add(word);
                        }
                    }
                }
            }

            return new ServiceResponse<List<string>> { Data = result };
        }

        private async Task<List<Product>> FindProductsBySearchText(string searchText)
        {
            return await _context.Products
                            .Where(p => p.Title.ToLower().Contains(searchText) || p.Description.ToLower().Contains(searchText))
                            .Include(p => p.Variants)
                            .ToListAsync();
        }

        public async Task<ServiceResponse<List<Product>>> GetFeaturedProducts()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                .Where(p => p.Featured)
                .Include(p => p.Variants)
                .ToListAsync()
            };

            return response;
        }
    }
}