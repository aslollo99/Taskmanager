namespace TaskManager.Models;

public class AccessRight
{
    public int Id { get; set; }
    
    //FK verso QrCode
    public int QrCodeId { get; set; }
    public QrCode QrCode { get; set; }
    
    //FK verso Door
    public int DoorId { get; set; }
    public Door Door { get; set; }
    
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
}