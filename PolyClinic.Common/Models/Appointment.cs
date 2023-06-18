using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PolyClinic.Common.Models
{
    public class Appointment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentNo { get; set; }
        [Required]
        public string PatientId { get; set; }
        [Required]
        public string DoctorId { get; set; }
        [Required]
        public DateTime DateofAppointment { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
    }
}
