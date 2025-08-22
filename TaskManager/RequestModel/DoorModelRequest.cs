namespace TaskManager.RequestModel;

public class DoorModelRequest
{
    public string Name { get; set; }
    public int StrutturaId { get; set; }
    
    public string Location { get; set; }
    
    public bool IsActive { get; set; }
}