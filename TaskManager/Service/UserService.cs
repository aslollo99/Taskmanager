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

    public List<User> GetAll()
    {
        return _db.Users.ToList();
    }

    public User Add(UserModelRequest userRequest)
    {
        var user = new User
        {
            Email = userRequest.Email,
            fullname = userRequest.FullName,
            PasswordHash = userRequest.Password,
            Role = userRequest.Role,
            Username = userRequest.Username
        };
        _db.Users.Add(user);
        _db.SaveChanges();
        return user;
    }

    public void Update(int id, UserModelRequest userRequest)
    {
        var user = GetById(id);
        user.Email = userRequest.Email;
        user.fullname = userRequest.FullName;
        if (userRequest.Password != null)
        {
            user.PasswordHash = userRequest.Password;
        }
        user.Role = userRequest.Role;
        user.Username = userRequest.Username;
        _db.Users.Update(user);
        _db.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = GetById(id);
        if (user == null)
        {
            throw new Exception("Utente non trovato");
        }
        _db.Users.Remove(user);
        _db.SaveChanges();
    }

    public User? ValidateUser(AuthModelRequest userDaValidare)
    {
        return _db.Users.FirstOrDefault(user => user.Username == userDaValidare.UserName && user.PasswordHash == userDaValidare.Password);
    }
}