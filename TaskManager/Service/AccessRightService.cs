using TaskManager.Data;
using TaskManager.Models;
using TaskManager.RequestModel;


namespace TaskManager.Service;

public class AccessRightService
{
    private readonly AppDbContext _dbContext;

    public AccessRightService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<AccessRight> GetAccessRightsByQrCodeId(int QrCodeId)
    {
        return _dbContext.AccessRights.Where(x => x.QrCodeId == QrCodeId).ToList();
    }

    public void UpdateDoorByQrCodeId(int QrCodeId, List<AccessRightModelRequest> accessRightModel)
    {
        var existingAccessRights = _dbContext.AccessRights.Where(x => x.QrCodeId == QrCodeId).ToList();

        foreach (var existingAccessRight in accessRightModel)
        {
            var esiste = existingAccessRights.FirstOrDefault(r => r.DoorId == existingAccessRight.DoorsId);
            if (esiste != null)
            {
                esiste.ValidFrom = existingAccessRight.ValidFrom;
                esiste.ValidTo = existingAccessRight.ValidTo;
            }
            else
            {
                var newAccessRight = new AccessRight
                {
                    QrCodeId = QrCodeId,
                    DoorId = existingAccessRight.DoorsId,
                    ValidFrom = existingAccessRight.ValidFrom,
                    ValidTo = existingAccessRight.ValidTo,
                };
                _dbContext.AccessRights.Add(newAccessRight);
            }
        }
        
        var toRemove = existingAccessRights.Where(ar => !accessRightModel.Any(er => er.DoorsId == ar.DoorId)).ToList();
        _dbContext.AccessRights.RemoveRange(toRemove);
        
        _dbContext.SaveChanges();
    }
}