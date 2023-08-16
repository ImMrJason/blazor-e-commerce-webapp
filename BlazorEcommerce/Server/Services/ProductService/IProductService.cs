﻿namespace BlazorEcommerce.Server.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetProducts();

        Task<ServiceResponse<Product>> GetProduct(int productId);

        Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl);

        Task<ServiceResponse<List<Product>>> SearchProducts(string searchText);

        Task<ServiceResponse<List<string>>> GetProductSuggestions(string searchText);
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

        public async Task<ServiceResponse<List<Product>>> SearchProducts(string searchText)
        {
            // find products where the title or description contains the search text
            var response = new ServiceResponse<List<Product>>
            {
                Data = await FindProductsBySearchText(searchText)
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

    }
}
