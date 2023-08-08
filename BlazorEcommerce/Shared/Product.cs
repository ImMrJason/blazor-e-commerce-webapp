using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorEcommerce.Shared
{
    public class Product
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        // The decimal(18,2) means that the database will store the value as a decimal with 18 digits and 2 of them will be after the decimal point.
        public decimal Price { get; set; }

        public Category? Category { get; set; }

        public int CategoryId { get; set; }
    }
}