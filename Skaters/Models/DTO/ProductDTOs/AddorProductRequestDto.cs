using System.ComponentModel.DataAnnotations;

namespace Skaters.Models.DTO.ProductDTOs
{
    public class AddorProductRequestDto
    {
        public Guid StoreId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public string? Description { get; set; }
        [Required]
        public required string Category { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
