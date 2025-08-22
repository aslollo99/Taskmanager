using TaskManager.Data;
using TaskManager.Models;
using TaskManager.RequestModel;

namespace TaskManager.Service;

public class DoorService
{
    private readonly AppDbContext _db;

    public DoorService(AppDbContext db)
    {
        _db = db;
    }

    public List<Door> GetDoors()
    {
        return _db.Doors.ToList();
    }

    public Door? GetDoor(int id)
    {
        return _db.Doors.Find(id);
    }
    
    public Door AddDoor(DoorModelRequest doorRequest)
    {
        var door = new Door
        {
            Name = doorRequest.Name,
            StrutturaId = doorRequest.StrutturaId,
            Location = doorRequest.Location,
            IsActive = doorRequest.IsActive,
        };
        _db.Doors.Add(door);
        _db.SaveChanges();
        return door;
    }

    public void UpdateDoor(DoorModelRequest doorRequest, int idRequest)
    {
        var door = new Door
        {
            Id = idRequest,
            Name = doorRequest.Name,
            StrutturaId = doorRequest.StrutturaId,
            Location = doorRequest.Location,
            IsActive = doorRequest.IsActive,
        };
        _db.Doors.Update(door);
        _db.SaveChanges();
    }
    
    public void DeleteDoor(int id)
    {
        var door = _db.Doors.Find(id);
        if (door == null)
        {
            throw new Exception("Porta non trovata");
        }
        _db.Doors.Remove(door);
        _db.SaveChanges();
    }
}