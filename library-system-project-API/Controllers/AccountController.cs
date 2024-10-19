using BusinessLayer.Dto;
using BusinessLayer.ManagersInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace library_system_project_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountManager _accountManager;
        private readonly IConfiguration _configurationManager;
        public AccountController(IAccountManager accountManager,IConfiguration configurationManager)
        {
            _accountManager = accountManager;
            _configurationManager = configurationManager;
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountManager.Register(user);
                if (result.Succeeded)
                    return Ok(user);
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BadRequest(errors);
                }
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto user)
        {

            var result = await _accountManager.Login(user);
            if (result.Succeeded)
            {
                TokenDto tokendto = GenerateToken(user);
                return Ok(tokendto.token);
            }
            else
                return BadRequest(result.ErrorMessage);
        }
        private TokenDto GenerateToken(LoginDto user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationManager["jwt:key"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {

                new Claim(JwtRegisteredClaimNames.Sub,user.email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                
            };
            var token = new JwtSecurityToken(
                issuer: _configurationManager["jwt:issuer"],
                audience: _configurationManager["jwt:audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            );
            return new TokenDto()
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpirDate = token.ValidTo
            };
        }

    }
}
