using ClinicBackend_Final.Services.Interfaces;

namespace ClinicBackend_Final.Services
{
    public class PdfExportService : IPdfExportService
    {
        public byte[] ExportPrescriptionToPdf(int prescriptionId)
        {
            // «” Œœ„Ì iText7 √Ê QuestPDF Â‰«
            return new byte[0]; // placeholder
        }
    }
}
