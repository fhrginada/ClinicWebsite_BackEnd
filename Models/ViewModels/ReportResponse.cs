namespace PatientApi.Models.ViewModels
{
    public class ReportResponse
    {
        public int TotalAppointments { get; set; }
        public int TotalConsultations { get; set; }
        public DateTime? Date { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}
