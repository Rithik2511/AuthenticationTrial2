using Application.Contracts.Service;
using Application.DTO.User;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationTrial2.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _service;
        public LoginController(IUserService service)
        {
            _service = service;
        }
        public IActionResult SignIn()
        {
            return View();
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
            }
            else
            {
                response = await _service.GetUserAsync(request, token);
            }
            return Json(response.ErrorMessage);
        }
    }
}
