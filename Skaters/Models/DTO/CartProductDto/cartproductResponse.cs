namespace Skaters.Models.DTO.CartProductDto
{
    public class CartproductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string? Description { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public string storename { get; set; }
        public int quantity { get; set; } = 1;
    }
}
