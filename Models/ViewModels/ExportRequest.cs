namespace PatientApi.Models.ViewModels
{
    public class ExportRequest
    {
        public string Type { get; set; } = null!; // "Patients" or "Appointments"
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
