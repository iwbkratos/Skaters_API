using Skaters.Domain.Model;

namespace Skaters.Models.Domain
{
    public class CartProduct
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public required int Quantity { get; set; } = 1;
        public Product Product { get; set; }
    }
}
