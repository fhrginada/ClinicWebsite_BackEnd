using System;
using System.Collections.Generic;

namespace ClinicBackend_Final.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public int ConsultationId { get; set; } // FK → Consultation
        public int DoctorId { get; set; } // FK → Doctor
        public int PatientId { get; set; } // FK → Patient
        public string Status { get; set; } = "Draft"; // Draft / Confirmed / Exported

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual List<PrescriptionItem> Items { get; set; }
        public virtual List<DigitalSignatureToken> Tokens { get; set; }
    }
}
