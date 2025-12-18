namespace ClinicBackend_Final.Services.Interfaces
{
    public interface IPdfExportService
    {
        byte[] ExportPrescriptionToPdf(int prescriptionId);
    }
}
