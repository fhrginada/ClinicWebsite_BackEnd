using PatientApi.Models.Entities;
using System.Threading.Tasks;

namespace PatientApi.Services
{
    public class PdfGeneratorService
    {
        public Task<byte[]> GeneratePrescriptionPdfAsync(Prescription prescription)
        {
            
            return Task.FromResult(new byte[0]);
        }
    }
}
