using Microsoft.AspNetCore.Identity;

namespace Skaters.Models.CustomClass
{
    public class Applicationuser: IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
