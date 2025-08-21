namespace TaskManager.Models;

public class Struttura
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Indirizzo { get; set; }
    
    // Relazioni
    public ICollection<Door> Doors { get; set; }
}