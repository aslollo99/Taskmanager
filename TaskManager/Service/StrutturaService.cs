using TaskManager.Data;
using TaskManager.Models;
using TaskManager.RequestModel;

namespace TaskManager.Service;

public class StrutturaService
{
    private readonly AppDbContext _dbContext;

    public StrutturaService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Struttura GetById(int id)
    {
        return _dbContext.Strutture.Find(id);
    }

    public List<Struttura> GetList()
    {
        return _dbContext.Strutture.ToList();
    }

    public Struttura Create(StrutturaModelRequest request)
    {
        var struttura = new Struttura
        {
            Name = request.Name,
            Indirizzo = request.Indirizzo,
            // altre proprietà se servono
        };
        _dbContext.Strutture.Add(struttura);
        _dbContext.SaveChanges();
        return struttura;
    }

    public void Update(StrutturaModelRequest request, int id)
    {
        var struttura = new Struttura
        {
            Id = id,
            Name = request.Name,
            Indirizzo = request.Indirizzo,
            // altre proprietà se servono
        };
        _dbContext.Strutture.Update(struttura);
        _dbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        var struttura = _dbContext.Strutture.Find(id);
        if (struttura == null)
        {
            throw new Exception("Struttura non trovata");
        }
        _dbContext.Strutture.Remove(struttura);
        _dbContext.SaveChanges();
    }
}