using Skaters.Domain.Model;

namespace Skaters.Models.DTO.StoreDTOs
{
    public class StoreDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string UserId { get; set; }

        public List<Product> products { get; set; }
    }
}
