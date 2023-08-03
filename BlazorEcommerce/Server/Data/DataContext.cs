namespace BlazorEcommerce.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed some product data to the database
            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    Id = 1,
                    Title = "Product 1",
                    // give me a interesting description with 1000 characters
                    Description = "This is the description for product 1. Make the description interesting with at least 1000 characters. and make sure that the description is long enough to wrap to the next line.",
                    ImageUrl = "https://via.placeholder.com/250",
                    Price = 9.99M
                },
                new Product()
                {
                    Id = 2,
                    Title = "Product 2",
                    Description = "This is the description for product 2. Make the description interesting with at least 1000 characters. and make sure that the description is long enough to wrap to the next line.",
                    ImageUrl = "https://via.placeholder.com/250",
                    Price = 19.99M
                },
                new Product()
                {
                    Id = 3,
                    Title = "Product 3",
                    Description = "This is the description for product 3. Make the description interesting with at least 1000 characters. and make sure that the description is long enough to wrap to the next line.",
                    ImageUrl = "https://via.placeholder.com/250",
                    Price = 29.99M
                }
            );
        }

        public DbSet<Product> Products { get; set; }
    }
}