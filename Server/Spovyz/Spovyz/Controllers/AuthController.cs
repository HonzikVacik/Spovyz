using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Spovyz.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Spovyz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthController(ApplicationDbContext context, JwtSettings jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings;
        }

        [HttpPost("token")]
        public IActionResult GenerateToken(string user, string password)
        {
            //Kontrola, jestli není řetězec prázdný
            if (string.IsNullOrEmpty(user))
                return Unauthorized("Neplatné uživatelské jméno");
            if (string.IsNullOrEmpty(password))
                return Unauthorized("Neplatné uživatelské heslo");


            //Kontrola databáze

            //Kontrola existence user
            Employee[] employees = _context.Employees.Where(e => e.Username == user).ToArray();
            if(employees.Length == 0)
                return Unauthorized("e1");

            //Kontrola správného hesla
            var employee = employees[0];
            byte[] salt = employee.Salt;
            string hashedNow = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

            if (hashedNow != employee.Password)
                return Unauthorized("e1");

            //Vrácení tokenu

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "testuser"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, employee.Username), //user), //"mujuzivatel"),
                new Claim(ClaimTypes.Role, employee.Account_type.ToString())//employee.Account_type.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: creds
            );

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
