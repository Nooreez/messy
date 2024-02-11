using Messenger.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Messenger.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class MessController : ControllerBase
    {
        private static readonly string secretKey = GetGeneratedSecretKey();

        private readonly MessContext _context;

        private readonly ILogger<MessController> _logger;

        private readonly IConfiguration _configuration;

        public MessController(ILogger<MessController> logger, MessContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }
        private static string GetGeneratedSecretKey()
        {
            byte[] secretKeyBytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(secretKeyBytes);
            }

            // Convert the byte array to a base64-encoded string
            string base64SecretKey = Convert.ToBase64String(secretKeyBytes);

            return base64SecretKey;
        }
        [HttpGet("search/{text}")]
        public async Task<ActionResult<IEnumerable<string>>> GetFiveUsers(string text)
        {
            var users = await _context.Users.Where(i => i.username.StartsWith(text)).ToListAsync();
            var usernames = new List<string>();
            foreach(var user in users)
            {
                usernames.Add(user.username);
            }
            return usernames;
        }

        [HttpGet("get/{username}")]
        public async Task<ActionResult<Users>> GetUser(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(i => i.username.Equals(username));
            if (user == null) return NotFound();
            return Ok(user);
        }
        [HttpGet("get/all/users")]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers()
        {
            var list = await _context.Users.ToListAsync();  
            return list;
        }

        [HttpGet("get/all/requests")]
        public async Task<ActionResult<IEnumerable<FriendsRequest>>> GetAllRequsts()
        {
            var list = await _context.FriendsRequest.ToListAsync();
            return list;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Users registrationData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the username or email is already in use
            if (await _context.Users.AnyAsync(u => u.username == registrationData.username))
            {
                return Conflict("Username is already in use.");
            }
            else if(await _context.Users.AnyAsync(u => u.email == registrationData.email))
            {
                return Conflict("Email is already in use");
            }
            if(registrationData.username == null)
            {
                return Conflict("username cannot be empty");
            }
            else if(registrationData.email == null)
            {
                return Conflict("email cannot be empty");
            }
            else if(registrationData.name == null)
            {
                return Conflict("name cannot be empty");
            }
            else if(registrationData.password == null)
            {
                return Conflict("password cannot be empty");
            }
            registrationData.password = HashPassword(registrationData.password);

            _context.Users.Add(registrationData);
            await _context.SaveChangesAsync();

            return Ok("Registration successful.");
        }
        
        private string HashPassword(string password)
        {
            // Replace with your password hashing logic
            return password;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var list = await _context.Users.FirstOrDefaultAsync(i => i.username.Equals(model.username) && i.password.Equals(model.password));
            if(list != null) {
                string token = GenerateJwtToken(model.username);
                return Ok(token);
            }
            return NotFound();
        }

        /*
        [HttpGet("isit")]
        private async Task<ActionResult<bool>> IsValidCredentials(string username, string password)
        {
        }
        */
        private string GenerateJwtToken(string user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]); 

            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name , user),
            };
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        /*
                private string GenerateJwtToken(Users user)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                        new Claim(ClaimTypes.Name, user.username),
                    }),
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    return tokenHandler.WriteToken(token);
                }
        */
        
    }
}