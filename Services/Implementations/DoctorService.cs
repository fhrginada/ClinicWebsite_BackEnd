using PatientApi.Models.Entities;
using PatientApi.Models.ViewModels.DoctorVM;
using PatientApi.Repositories.Interfaces;
using PatientApi.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientApi.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repo;

        public DoctorService(IDoctorRepository repo)
        {
            _repo = repo;
        }

        public async Task<DoctorResponse> CreateDoctorAsync(CreateDoctorRequest model)
        {
            var doctor = new Doctor
            {
                FullName = model.FullName,
                Specialty = model.Specialty,
                UserId = model.UserId
            };

            await _repo.AddAsync(doctor);

            return new DoctorResponse
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.FullName ?? string.Empty,
                Specialty = doctor.Specialty ?? string.Empty,
                UserId = doctor.UserId
            };
        }

        public async Task<IEnumerable<DoctorResponse>> GetAllDoctorsAsync()
        {
            var doctors = await _repo.GetAllAsync();

            return doctors.Select(d => new DoctorResponse
            {
                DoctorId = d.DoctorId,
                FullName = d.FullName ?? string.Empty,
                Specialty = d.Specialty ?? string.Empty,
                UserId = d.UserId
            });
        }

        public async Task<DoctorResponse?> GetDoctorByIdAsync(int id)
        {
            var doctor = await _repo.GetByIdAsync(id);
            if (doctor == null)
                return null;

            return new DoctorResponse
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.FullName ?? string.Empty,
                Specialty = doctor.Specialty ?? string.Empty,
                UserId = doctor.UserId
            };
        }

        public async Task<bool> UpdateDoctorAsync(int id, UpdateDoctorRequest model)
        {
            var doctor = await _repo.GetByIdAsync(id);
            if (doctor == null)
                return false;

            doctor.FullName = model.FullName;
            doctor.Specialty = model.Specialty;
            

            await _repo.UpdateAsync(doctor);
            return true;
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
