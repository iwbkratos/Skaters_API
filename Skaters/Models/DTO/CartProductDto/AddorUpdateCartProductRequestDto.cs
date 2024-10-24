using System.ComponentModel.DataAnnotations;

namespace Skaters.Models.DTO.CartProductDto
{
    public class AddorUpdateCartProductRequestDto
    {
        [Required]
        public Guid CartId { get; set; }
        [Required]
        public  required Guid ProductId { get; set; }
        [Required]
        public required int Quantity { get; set; } = 1;
    }
}
