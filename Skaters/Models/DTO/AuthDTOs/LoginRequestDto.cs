using System.ComponentModel.DataAnnotations;

namespace Skaters.Models.DTO.AuthDTOs
{
    public class LoginRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
