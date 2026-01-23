using System;
using System.Collections.Generic;

namespace PatientApi.Models.Entities
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }
        public int ConsultationId { get; set; } // link to consultation
        public int DoctorId { get; set; }
        public int PatientId { get; set; }

        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } // draft, confirmed, exported

        public ICollection<PrescriptionDetail> Items { get; set; }
    }
}
