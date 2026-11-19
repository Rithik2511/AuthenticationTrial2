using Application.Contracts.Service;
using Application.DTO.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationTrial2.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _service;
        private readonly IConfiguration _config;
        public LoginController(IUserService service, IConfiguration config)
        {
            _service = service;
            _config = config;
        }
        public IActionResult SignIn()
        {
            return View();
        }
        public IActionResult SignOut()
        {
            var cookieName = _config["Jwt:CookieName"]!;

            // Delete the cookie using matching security options
            Response.Cookies.Delete(cookieName, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
            return Ok(new { message = "Logged out successfully." });
        }
        public IActionResult Register()
        {
            return View();
        }
        public async Task<IActionResult> CreateAccount([FromBody] CreateUserRequest request,CancellationToken token)
        {
            UserResponse response = new UserResponse() { UserName = request.UserName, ErrorMessage = "Account Created Successfully" };
            if (!ModelState.IsValid)
            {
                int counter = 0;
                response.ErrorMessage = string.Join("<br>", ModelState.Values
                    .Select(val => val.Errors.Select(error => $"{++counter}) {error.ErrorMessage}"))
                    .SelectMany(val => val));
            }
            else
            {
                response = await _service.AddUserAsync(request,token);
            }
            return Json(response.ErrorMessage);
        }
        public async Task<IActionResult> AccessAccount([FromBody] CreateUserRequest request, CancellationToken token)
        {
            UserResponse response = new UserResponse() { UserName = request.UserName, ErrorMessage = "Login Success" };
            if (!ModelState.IsValid)
            {
                int counter = 0;
                response.ErrorMessage = string.Join("<br>", ModelState.Values
                    .Select(val => val.Errors.Select(error => $"{++counter}) {error.ErrorMessage}"))
                    .SelectMany(val => val));
                return Json(response.ErrorMessage);
            }
            else
            {
                response = await _service.GetUserAsync(request, token);
            }
            // 2. Generate the JWT token
            var tokenString = GenerateJwtToken(request.UserName, "Admin");

            // 3. Configure secure cookie options
            var expireMinutes = double.Parse(_config["Jwt:ExpireMinutes"]!);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Prevents JavaScript (XSS) from reading the cookie
                Secure = true,   // Ensures cookie is only sent over HTTPS
                SameSite = SameSiteMode.Strict, // Protects against CSRF attacks
                Expires = DateTimeOffset.UtcNow.AddMinutes(expireMinutes)
            };

            // 4. Attach the cookie to the HTTP response
            var cookieName = _config["Jwt:CookieName"]!;
            Response.Cookies.Append(cookieName, tokenString, cookieOptions);
            return Json(response.ErrorMessage);
        }
        private string GenerateJwtToken(string username, string role)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Define claims embedded inside the token payload
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var expireMinutes = double.Parse(jwtSettings["ExpireMinutes"]!);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
