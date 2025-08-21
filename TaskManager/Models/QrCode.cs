namespace TaskManager.Models;

public class QrCode
{
    public int Id { get; set; }
    public string codeValue { get; set; }
    public bool isActive { get; set; }
    
    //FK verso User
    public int UserId { get; set; }
    public User user { get; set; }
    
    // Relazioni
    public ICollection<AccessRight> AccessRights { get; set; }
}