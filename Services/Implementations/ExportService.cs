using PatientApi.Data;
using PatientApi.Services.Interfaces;

namespace PatientApi.Services.Implementations
{
    public class ExportService : IExportService
    {
        private readonly AppDbContext _context;

        public ExportService(AppDbContext context)
        {
            _context = context;
        }

        public byte[] ExportPatientsToExcel()
        {
            // ClosedXML logic here
            return Array.Empty<byte>();
        }

        public byte[] ExportAppointmentsToPdf()
        {
            // PDF service wrapper (Member 4)
            return Array.Empty<byte>();
        }
    }

}
