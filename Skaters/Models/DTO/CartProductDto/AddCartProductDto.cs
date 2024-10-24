using System.ComponentModel.DataAnnotations;

namespace Skaters.Models.DTO.CartProductDto
{
    public class AddCartProductDto
    {
        [Required]
        public required Guid ProductId { get; set; }
        [Required]
        public required int Quantity { get; set; } = 1;
    }
}
