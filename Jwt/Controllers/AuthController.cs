using Jwt.Dto;
using Jwt.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWTconfig _jwtconfig;

        public AuthController(UserManager<ApplicationUser> userManager, IOptions<JWTconfig> jwtconfig)
        {
            _userManager = userManager;
            _jwtconfig = jwtconfig.Value;
        }


        [HttpPost]
        [Route("Register")]

        public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
        {
            if(ModelState.IsValid)
            {
                var user_exist = await _userManager.FindByEmailAsync(userRequest.Email);

                if(user_exist != null) 
                {
                    return BadRequest(new AuthResponse()
                    {
                        Result = false,
                        Message = "Email already exist"
                    });
                }

                var user = new ApplicationUser()
                {
                    Email = userRequest.Email,
                    UserName = userRequest.Name,
                    
                };

                var is_created = await _userManager.CreateAsync(user, userRequest.Password);

                if(is_created.Succeeded)
                {
                    var token = GenerateJwtToken(user);
                    return Ok(new AuthResponse()
                    {
                        Token = token,
                        Result = true
                    });
                }
            }

            
                return BadRequest(ModelState);
            
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userLogin.Email);
                if(user != null && await _userManager.CheckPasswordAsync(user, userLogin.Password))
                {
                    var token = GenerateJwtToken(user);
                    return Ok(new AuthResponse()
                    {
                        Token = token,
                        Result = true
                    });
                }
                return Unauthorized(new AuthResponse()
                {
                    Result = false,
                    Message = "Email or password incorrect"
                });
            }
            return BadRequest(ModelState);
        }




        private string GenerateJwtToken(ApplicationUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_jwtconfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(type:"Id", value: user.Id.ToString()),
                    new Claim(type:JwtRegisteredClaimNames.Sub, value: user.Email),
                    new Claim(type:JwtRegisteredClaimNames.Email, value: user.Email),
                    new Claim(type:JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
                    new Claim(type:JwtRegisteredClaimNames.Iat, value: DateTime.Now.ToUniversalTime().ToString()),
                }),

                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
    }
    }

   
}
