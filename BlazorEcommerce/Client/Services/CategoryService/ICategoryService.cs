namespace BlazorEcommerce.Client.Services.CategoryService
{
    public interface ICategoryService
    {
        List<Category> Categories { get; set; }

        Task GetCategories();
    }

    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _http;

        public CategoryService(HttpClient http)
        {
            _http = http;
        }

        public List<Category> Categories { get; set; } = new List<Category>();

        public async Task GetCategories()
        {
            // use the http object to call our endpoint api/category to get a list of categories
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/Category");

            if (result != null && result.Data != null)
            {
                Categories = result.Data;
            }

        }
    }
}
