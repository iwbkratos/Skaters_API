namespace Skaters.Models.DTO.ProductDTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string? Description { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid StoreId { get; set; }
        public required string UserId { get; set; }
    }
}
