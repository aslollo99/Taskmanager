namespace TaskManager.Models;

public class Door
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public bool IsActive { get; set; }
    
    public int StrutturaId { get; set; }
    public Struttura struttura { get; set; }
    
    // Relazioni
    public ICollection<AccessRight> AccessRights { get; set; }
}