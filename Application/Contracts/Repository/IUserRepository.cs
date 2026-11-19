using Application.DTO.User;
using Domain.Entities.Login;

namespace Application.Contracts.Repository
{
    public interface IUserRepository
    {
        Task<bool> CheckIfUserExistsAsync(User userRequest, CancellationToken token = default);
        Task<User> GetUserAsync(User userRequest, CancellationToken token = default);
        Task<int> AddUserAsync(User userRequest, CancellationToken token = default);
        Task<List<User>> GetAllUserAsync(CancellationToken token = default);
    }
}
