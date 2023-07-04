using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RestorauntAPI.Data;
using RestorauntAPI.Data.DTO;
using RestorauntAPI.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RestorauntAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly RestorauntDBContext _context;
        public AccountController(RestorauntDBContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> PostUser([FromBody] UserDTO newUser)
        {
            cryptPassword(newUser.Password, out string hashedPassword);
            if (_context.Users.Any(u => u.Username == newUser.Username))
            {
                return StatusCode((int)HttpStatusCode.Conflict); ;
            }

            var user = new User
            {
                Username = newUser.Username,
                Password = hashedPassword,
                Role = newUser.Role,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var claims = new List<Claim>
            {
                new Claim("id", user.ID.ToString()), // Добавление идентификатора пользователя
                new Claim("role", user.Role) // Добавление роли пользователя
            };

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claims,
                    expires: now.Add(TimeSpan.FromSeconds(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Json(new { token = encodedJwt });
        }

        [HttpPost("login")]
        public IActionResult Token([FromBody] LoginDTO newUser)
        {
            var claims = GetIdentity(newUser.Username, newUser.Password);
            if (claims == null)
            {
                var error = new { EmailOrPassword = "Неправильное имя пользователя или пароль" };
                return BadRequest(error);
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claims,
                    expires: now.Add(TimeSpan.FromSeconds(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Json(new { token = encodedJwt });
        }


        private List<Claim> GetIdentity(string username, string password)
        {
            var person = _context.Users.Where(u => u.Username == username).FirstOrDefault();
            if (person != null && equalsPassword(password, person.Password) == true)
            {
                var claims = new List<Claim>
                {
                     new Claim("id", person.ID.ToString()), // Добавление идентификатора пользователя
                     new Claim("role", person.Role)
                };
                
                return claims;
            }
            return null;
        }

        private void cryptPassword(string password, out string passwordHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                passwordHash = Convert.ToBase64String(hashedBytes);
            }
        }

        private bool equalsPassword(string password, string passwordHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hashedPassword = Convert.ToBase64String(hashedBytes);
                return hashedPassword == passwordHash;
            }
        }
    }
}
