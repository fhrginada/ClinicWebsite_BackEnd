using System.Collections.Generic;

namespace PatientApi.Models.ViewModels
{
    public class PrescriptionRequest
    {
        public int ConsultationId { get; set; }
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
        public string Status { get; set; }
        public List<PrescriptionItemRequest> Items { get; set; }
    }
}
