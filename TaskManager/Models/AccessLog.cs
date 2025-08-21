namespace TaskManager.Models;

public class AccessLog
{
    public int Id { get; set; }
    
    public int QrCodeId { get; set; }
    public QrCode QrCode { get; set; }
    
    public int DoorId { get; set; }
    public Door Door { get; set; }
    
    public DateTime Orario  { get; set; }
    public string Result {get;set;} //permesso //negato
}