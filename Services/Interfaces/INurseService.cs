using PatientApi.Models.ViewModels.NurseVM;

namespace PatientApi.Services.Interfaces
{
    public interface INurseService
    {
        Task<NurseResponse> CreateAsync(CreateNurseRequest request);
        Task<IEnumerable<NurseResponse>> GetAllAsync();
        Task<NurseResponse?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateNurseRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
