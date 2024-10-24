using Skaters.Domain.Model;
using Skaters.Models.DTO.ProductDTOs;

namespace Skaters.Models.DTO.CartDTOs
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public required string UserId { get; set; }
        public string Status { get; set; }
        public double Total { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<Product> Product { get; set; }

    }
}
