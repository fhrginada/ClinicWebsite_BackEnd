using ClinicalBackend.Models;

namespace ClinicalBackend.Services
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<Prescription>> GetAllAsync();
        Task<Prescription?> GetByIdAsync(int id);
        Task<Prescription> CreateAsync(Prescription prescription);
        // يمكنك إضافة UpdateAsync و DeleteAsync لاحقاً
    }
}