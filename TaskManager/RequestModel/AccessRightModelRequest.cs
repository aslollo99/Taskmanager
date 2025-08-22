namespace TaskManager.RequestModel;

public class AccessRightModelRequest
{
    public int QrCodeId { get; set; }
    public int DoorsId { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
}