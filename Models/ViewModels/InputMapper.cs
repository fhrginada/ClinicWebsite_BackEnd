using PatientApi.Models.Entities;
using PatientApi.Models.ViewModels;

namespace PatientApi.Models.ViewModels;

public static class InputMapper
{
    public static Patient ToEntity(PatientCreateViewModel vm)
    {
        return new Patient
        {
            FirstName = vm.FirstName,
            LastName = vm.LastName,
            Address = vm.Address,
        };
    }

    public static void ApplyUpdate(Patient entity, PatientUpdateViewModel vm)
    {
        if (!string.IsNullOrWhiteSpace(vm.FirstName))
            entity.FirstName = vm.FirstName;
        if (!string.IsNullOrWhiteSpace(vm.LastName))
            entity.LastName = vm.LastName;
        if (!string.IsNullOrWhiteSpace(vm.Address))
            entity.Address = vm.Address;
    }

    public static PatientDashboardResponse ToDashboard(
        Patient patient,
        List<AppointmentResponse> appointments,
        List<NotificationResponse> notifications)
    {
        return new PatientDashboardResponse
        {
            FirstName = patient.FirstName,
            UpcomingAppointments = appointments,
            Notifications = notifications
        };
    }
}
