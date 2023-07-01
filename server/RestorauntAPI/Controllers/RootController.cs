using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace RestorauntAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControllerRoot : ControllerBase
    {
        protected bool Validation(string? token, out int userID)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var key = Encoding.ASCII.GetBytes(AuthOptions.KEY);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                // Получаем идентификационные данные пользователя из токена
                if (int.TryParse(jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value, out userID))
                {
                    return true;
                }
                else
                {
                    userID = 0;
                    return false;
                }
            }
            catch
            {
                userID = 0;
                return false;
            }
        }

        protected int GetCurrentUserID()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (Validation(token, out int userID))
            {
                return userID;
            }
            else
            {
                // В случае, если токен недействителен или не содержит идентификационных данных пользователя
                // можете вернуть значение, которое подходит для вашего приложения, например, -1 или 0.
                return 0;
            }
        }
    }
}
