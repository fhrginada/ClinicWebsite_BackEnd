using System;

namespace ClinicBackend_Final.Models
{
    public class DigitalSignatureToken
    {
        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public virtual Prescription Prescription { get; set; }
    }
}
