using Infrastructure.Persistance.Connectrion.DBContext;
using Application.Contracts.Repository;
using Domain.Entities.Login;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext _context;
        public UserRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<int> AddUserAsync(User userRequest, CancellationToken token = default)
        {
            await _context.Users.AddAsync(userRequest,token);
            return await _context.SaveChangesAsync();
        }

        public Task<bool> CheckIfUserExistsAsync(User userRequest, CancellationToken token = default)
        {
            return _context.Users.AnyAsync(user => user.Username == userRequest.Username,token);
        }

        public Task<List<User>> GetAllUserAsync(CancellationToken token = default)
        {
            return _context.Users.ToListAsync(token);
        }

        public Task<User> GetUserAsync(User userRequest, CancellationToken token = default)
        {
            return _context.Users.Where(user => user.Username == userRequest.Username && user.Password == userRequest.Password).FirstOrDefaultAsync(token);
        }

    }
}
