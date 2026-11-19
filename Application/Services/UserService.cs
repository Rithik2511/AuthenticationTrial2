using Application.Contracts.Repository;
using Application.Contracts.Service;
using Application.DTO.User;
using Domain.Entities.Login;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task<UserResponse> AddUserAsync(CreateUserRequest userRequest, CancellationToken token = default)
        {
            string ErrorMessage = "Account Created Successfully";
            if (!await this.CheckIfUserExistsAsync(userRequest))
            {
                int rowAffected = await _repository.AddUserAsync(new User()
                {
                    Username = userRequest.UserName,
                    Password = userRequest.Password
                }, token);
                if (rowAffected == 0)
                {
                    ErrorMessage = "User Saving Failed";
                }
            }
            else
                ErrorMessage = "This User Aleady Exists";

            return new UserResponse
            {
                UserName = userRequest.UserName,
                ErrorMessage = ErrorMessage
            };
        }

        public Task<bool> CheckIfUserExistsAsync(CreateUserRequest userRequest, CancellationToken token = default)
        {
            return _repository.CheckIfUserExistsAsync(new User()
            {
                Username = userRequest.UserName,
                Password = userRequest.Password
            }, token);
        }

        public Task<List<UserResponse>> GetAllUserAsync(CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<UserResponse> GetUserAsync(CreateUserRequest userRequest, CancellationToken token = default)
        {
            UserResponse response = new UserResponse()
            {
                UserName = userRequest.UserName,
                ErrorMessage = "Login Success"
            };
            User user = await _repository.GetUserAsync(new User()
            {
                Username = userRequest.UserName,
                Password = userRequest.Password
            }, token);
            if (user == null)
                response.ErrorMessage = "Username or Password is Incorrect";
            return response;

        }
    }
}
