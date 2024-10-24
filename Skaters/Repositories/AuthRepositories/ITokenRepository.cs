using Microsoft.AspNetCore.Identity;
using Skaters.Models.CustomClass;

namespace Skaters.Repositories.AuthRepositories
{
    public interface ITokenRepository
    {
        string CreateToken(Applicationuser user, List<string> roles, string userId);
    }
}
