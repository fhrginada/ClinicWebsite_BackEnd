using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.Models.ViewModels;
using PatientApi.Services.Interfaces;

public class ReportService : IReportService
{
    private readonly AppDbContext _context;

    public ReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ReportResponse> GetDailyStatsAsync(DateTime date)
    {
        var appointments = await _context.Appointments
            .CountAsync(a => a.AppointmentDate.Date == date.Date);

        var consultations = await _context.Consultations
            .CountAsync(c => c.ConsultationDate.Date == date.Date);

        return new ReportResponse
        {
            TotalAppointments = appointments,
            TotalConsultations = consultations,
            Date = date
        };
    }

    public async Task<ReportResponse> GetMonthlyStatsAsync(int year, int month)
    {
        var consultations = await _context.Consultations
            .CountAsync(c => c.ConsultationDate.Year == year && c.ConsultationDate.Month == month);

        return new ReportResponse
        {
            TotalConsultations = consultations,
            Month = month,
            Year = year
        };
    }
}
