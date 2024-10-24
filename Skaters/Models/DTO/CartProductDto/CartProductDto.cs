using Skaters.Domain.Model;

namespace Skaters.Models.DTO.CartProductDto
{
    public class CartProductDto
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public required int Quantity { get; set; } 
        public Product Product { get; set; }
    }
}
