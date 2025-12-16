using System;
using System.ComponentModel.DataAnnotations;
using PatientApi.Data;

namespace PatientApi.Models.Entities


{
    public class NurseSchedule
    {
        [Key]
        public int NurseScheduleId { get; set; }

        public int NurseId { get; set; }
        public Nurse Nurse { get; set; }

        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
