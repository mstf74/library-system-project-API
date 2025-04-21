using BusinessLayer.Dto;
using BusinessLayer.ManagersInterfaces;
using DataAcessLayer.Models;
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
        private readonly IRefreshTokenManager _refreshTokenManager;
        private readonly IConfiguration _configurationManager;
        public AccountController(IAccountManager accountManager, IConfiguration configurationManager, IRefreshTokenManager refreshTokenManager)
        {
            _accountManager = accountManager;
            _configurationManager = configurationManager;
            _refreshTokenManager = refreshTokenManager;
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
                        TokenResultDto token = await GenerateToken(loginResult);
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
                        TokenResultDto token = await GenerateToken(loginResult);
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
                TokenResultDto loginResult = await GenerateToken(result);
                return Ok(loginResult);
            }
            else
                return BadRequest(result.ErrorMessage);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, AccountDto user)
        {
            var result = await _accountManager.UpdateUser(id, user);
            if (!result.Succeeded)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.ErrorMessage);
        }
        [HttpPost]
        public async Task<IActionResult> RefreshToken(RequestTokenDto requestToken)
        {
            TokenResultDto newToken;
            try
            {
                newToken = await ValidateAndRefreshToken(requestToken);
            }
            catch
            {
                return BadRequest("invalid Jwttoken or refresh toke");
            }
            if (newToken is null)
                return BadRequest("invalid JwtToken or RefreshToken");
            return Ok(newToken);
        }
        private async Task<TokenResultDto> GenerateToken(LoginResult user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationManager["jwt:key"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.email),
                new Claim(ClaimTypes.Role,user.role),
            };
            var token = new JwtSecurityToken(
                issuer: _configurationManager["jwt:issuer"],
                audience: _configurationManager["jwt:audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            );
            var acesstoken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = new RefreshTokens()
            {
                tokenId = token.Id,
                expiryDate = DateTime.UtcNow.AddDays(7),
                userId = user.Id
            };
            await _refreshTokenManager.AddRefreshToken(refreshToken);
            return new TokenResultDto()
            {
                token = acesstoken,
                refreshToken = refreshToken.token,
                id = user.Id,
                role = user.role
            };
        }
        private async Task<TokenResultDto> ValidateAndRefreshToken(RequestTokenDto requestToken)
        {
            var key = Encoding.ASCII.GetBytes(_configurationManager["jwt:key"]);
            var jwtHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidAudience = _configurationManager["jwt:audience"],
                ValidateAudience = true,
                ValidIssuer = _configurationManager["jwt:issuer"],
                ValidateIssuer = true,
                ValidateLifetime = true,
            };
            jwtHandler.ValidateToken(requestToken.jwtToken, validationParameters, out var validatedToken);
            if (validatedToken is not JwtSecurityToken jwtToken ||
                !jwtToken.SignatureAlgorithm.Equals(SecurityAlgorithms.HmacSha256))
                return null;
            var tokenJti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var storedRefreshToken = await _refreshTokenManager.GetRefreshToken(requestToken.RefreshToken);
            if (storedRefreshToken is null || storedRefreshToken.expiryDate < DateTime.UtcNow ||
                storedRefreshToken.used || storedRefreshToken.revoked)
                return null;
            if (storedRefreshToken.tokenId != tokenJti)
                return null;
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
            var userEmail = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email).Value;
            var userRole = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            if (userRole is null || userEmail is null || userRole is null)
                return null;
            storedRefreshToken.used = true;
            await _refreshTokenManager.SaveChanges();
            var newTokens = await GenerateToken(new LoginResult()
            {
                Id = userId,
                email = userEmail,
                role = userRole,
            });
            return newTokens;
        }
    }
}
