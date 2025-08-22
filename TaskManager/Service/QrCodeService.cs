using TaskManager.Data;
using TaskManager.Models;
using TaskManager.RequestModel;

namespace TaskManager.Service;

public class QrCodeService
{
    private readonly AppDbContext _db;

    public QrCodeService(AppDbContext db)
    {
        _db = db;
    }

    public List<QrCode> GetQrCodesByUserId(int idUser)
    {
        return _db.QrCodes.Where(q => q.UserId == idUser).ToList();
    }

    public QrCode AddQrCodeUser(int idUser, QrCodeModelRequest qrCodeModel)
    {
        var qrCode = new QrCode
        {
            UserId = idUser,
            codeValue = qrCodeModel.CodeValue,
            isActive = qrCodeModel.isActive
        };
        _db.QrCodes.Add(qrCode);
        _db.SaveChanges();
        return qrCode;
    }

    public void UpdateStatoQrCode(int id,  QrCodeModelRequest qrCodeModel)
    {
        var qrCode = _db.QrCodes.FirstOrDefault(q => q.Id == id);
        qrCode.isActive = qrCodeModel.isActive;
        _db.QrCodes.Update(qrCode);
        _db.SaveChanges();
    }

    public void Delete(int id)
    {
        var qrCode = _db.QrCodes.FirstOrDefault(q => q.Id == id);
        if (qrCode == null)
        {
            throw new Exception("QrCode not found");
        }
        _db.QrCodes.Remove(qrCode);
        _db.SaveChanges();
    }
}