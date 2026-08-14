using InputWeb.Domain.Entities;
using InputWeb.Domain.Exceptions;
using InputWeb.Domain.Interfaces;
using InputWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InputWeb.Infrastructure.Repositories;

public class UserRepository(Context db) :IUserRepository
{
    public async Task<List<User>> GetUsers(string search = "")
    {
        return await db.Users.AsNoTracking().Where(a => (a.Name ?? "").Contains(search ?? "")).ToListAsync();
    }
    public async Task<User> GetUserById(Guid id)
    {
        return await db.Users.Where(a => a.Id == id).FirstOrDefaultAsync() ?? throw new NotFoundException("Usuário não encontrado");
    }
    public User UpdateUser(User user)
    {
        db.Users.Update(user);
        return user;
    }
    public User CreateUser(User user)
    {
        db.Users.Add(user);
        return user;
    }
    public async Task DeleteUser(Guid id)
    {
        var user = await GetUserById(id);
        db.Users.Remove(user);
    }
    public async Task<User?> GetUserByEmail(string email)
    {
        return await db.Users.AsNoTracking().Where(a => (a.Email ?? "") == email).FirstOrDefaultAsync();
    }


}