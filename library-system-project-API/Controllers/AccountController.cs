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
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _accountManager.GetById(id);
            if (user == null) 
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountManager.Register(user);
                if (result.Succeeded)
                {
                    var loginResult = await _accountManager.Login(new LoginDto()
                    {
                        email = user.email,
                        password = user.password,
                    });
                    if (loginResult.Succeeded) 
                    {
                        TokenDto token = GenerateToken(loginResult);
                        return Ok(token);
                    }
                    else
                    {
                        return BadRequest("register succeeded but login failed");
                    }
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BadRequest(errors);
                }
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(RegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountManager.RegisterAdmin(user);
                if (result.Succeeded)
                {
                    var loginResult = await _accountManager.Login(new LoginDto()
                    {
                        email = user.email,
                        password = user.password,
                    });
                    if (loginResult.Succeeded)
                    {
                        TokenDto token = GenerateToken(loginResult);
                        return Ok(token);
                    }
                    else
                    {
                        return BadRequest("register succeeded but login failed");
                    }
                }
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
                TokenDto tokendto = GenerateToken(result);
                return Ok(tokendto);
            }
            else
                return BadRequest(result.ErrorMessage);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id , AccountDto user)
        {
            var result = await _accountManager.UpdateUser(id, user);
            if (!result.Succeeded)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.ErrorMessage);
        }
        private TokenDto GenerateToken(LoginResult user)
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
                ExpirDate = token.ValidTo,
                UserId = user.Id,
                user_name = user.user_name,
                email = user.email,
                role = user.role,
            };
        }

    }
}
