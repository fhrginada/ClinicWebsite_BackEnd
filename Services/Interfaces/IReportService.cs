using PatientApi.Models.ViewModels;

namespace PatientApi.Services.Interfaces
{
    public interface IReportService
    {
        Task<ReportResponse> GetDailyStatsAsync(DateTime date);
        Task<ReportResponse> GetMonthlyStatsAsync(int year, int month);
    }

}
