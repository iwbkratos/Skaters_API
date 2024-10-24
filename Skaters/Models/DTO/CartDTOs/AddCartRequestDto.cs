using System.ComponentModel.DataAnnotations;

namespace Skaters.Models.DTO.CartDTOs
{
    public class AddCartRequestDto
    {

        [Required]
        public string Status { get; set; }
        [Required]
        public double Total { get; set; }
    }
}
