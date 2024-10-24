using System.ComponentModel.DataAnnotations;

namespace Skaters.Models.DTO.CartDTOs
{
    public class UpdateCartRequestDto
    {
        [Required]
        public string Status { get; set; }
        [Required]
        public double  Total { get; set; }

    }
}
