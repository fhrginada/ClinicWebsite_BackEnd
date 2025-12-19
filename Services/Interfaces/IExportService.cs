namespace PatientApi.Services.Interfaces
{
    public interface IExportService
    {
        byte[] ExportPatientsToExcel();
        byte[] ExportAppointmentsToPdf();
    }

}
