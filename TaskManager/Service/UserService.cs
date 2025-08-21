using TaskManager.Data;
using TaskManager.Models;
using TaskManager.RequestModel;

namespace TaskManager.Service;

public class UserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public User GetById(int id)
    {
        return _db.Users.Find(id);
    }

    public User? ValidateUser(AuthModelRequest userDaValidare)
    {
        return _db.Users.FirstOrDefault(user => user.Username == userDaValidare.UserName && user.PasswordHash == userDaValidare.Password);
    }
}