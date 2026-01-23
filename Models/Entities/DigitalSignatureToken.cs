using System;

namespace PatientApi.Models.Entities
{
    public class DigitalSignatureToken
    {
        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
