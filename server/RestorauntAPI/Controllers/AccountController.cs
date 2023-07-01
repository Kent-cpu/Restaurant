using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestorauntAPI.Data;
using RestorauntAPI.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RestorauntAPI.Controllers
{
    public class AccountController : Controller
    {
        private readonly RestorauntDBContext _context;
        public AccountController(RestorauntDBContext context)
        {
            _context = context;
        }

        [HttpPost("/register")]
        public async Task<ActionResult<User>> PostUser(UserDTO newUser)
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
                Role = "User"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsers", new { id = user.ID }, user);
        }

        [HttpPost("/login")]
        public IActionResult Token(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromSeconds(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name,
                role = identity.Claims.Where(c => c.Type == "Role").Select(c => c.Value).SingleOrDefault(),
                user_id = identity.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault()
            };
            return Json(response);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var person = _context.Users.Where(u => u.Username == username).FirstOrDefault();
            if (person != null && equalsPassword(password, person.Password) == true)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Username),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role),
                    new Claim("Role", person.Role),
                    new Claim("Id", person.ID.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
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
