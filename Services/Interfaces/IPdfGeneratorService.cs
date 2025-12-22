using PatientApi.Models.Entities;
using System.Threading.Tasks;

namespace PatientApi.Services.Interfaces
{
    public interface IPdfGeneratorService
    {
        Task<byte[]> GeneratePrescriptionPdfAsync(Prescription prescription);
    }
}
