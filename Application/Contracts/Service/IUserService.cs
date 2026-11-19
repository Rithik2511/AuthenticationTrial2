using Application.DTO.User;
using Domain.Entities.Login;

namespace Application.Contracts.Service
{
    public interface IUserService
    {
        Task<bool> CheckIfUserExistsAsync(CreateUserRequest userRequest, CancellationToken token = default);
        Task<UserResponse> GetUserAsync(CreateUserRequest userRequest, CancellationToken token = default);
        Task<UserResponse> AddUserAsync(CreateUserRequest userRequest, CancellationToken token = default);

        Task<List<UserResponse>> GetAllUserAsync(CancellationToken token = default);
    }
}
