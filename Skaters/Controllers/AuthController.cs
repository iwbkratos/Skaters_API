using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Skaters.Models.CustomClass;
using Skaters.Models.DTO.AuthDTOs;
using Skaters.Repositories.AuthRepositories;

namespace Skaters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Applicationuser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<Applicationuser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
  
            var identityUser = new Applicationuser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                Console.WriteLine(1);
                if ( registerRequestDto.Roles.Any())
                {
                    Console.WriteLine(2);
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered !");
                    }
                }
            }
            return BadRequest(registerRequestDto);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            var userId = await userManager.GetUserIdAsync(user);    

            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var jwtToken =  tokenRepository.CreateToken(user, roles.ToList(),userId);

                        var response = new LoginResponseDto
                        {
                            jwtToken = jwtToken
                        };

                        return Ok(response);
                    }
                }
            }
            return BadRequest("Username or Password incorrect");
        }
    }
}
