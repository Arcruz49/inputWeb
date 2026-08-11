using InputWeb.Domain.Entities;

namespace InputWeb.Domain.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetUsers(string search = "");
    Task<User> GetUserById(Guid id);
}