using System;
using System.Collections.Generic;

namespace PatientApi.ViewModels.PrescriptionViewModels
{
    public class PrescriptionViewModel
    {
        public int Id { get; set; }
        public int ConsultationId { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public List<PrescriptionItemDetailsViewModel> Items { get; set; } = new();
    }

    public class PrescriptionItemDetailsViewModel
    {
        public string MedicationName { get; set; } = string.Empty;
        public string Dose { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
    }
}
