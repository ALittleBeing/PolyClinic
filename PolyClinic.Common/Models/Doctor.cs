using System.ComponentModel.DataAnnotations;

namespace PolyClinic.Common.Models
{
    public class Doctor
    {

        public string DoctorId { get; set; }
        [Required]
        public string Specialization { get; set; }
        [Required]
        public string DoctorName { get; set; }
        [Required]
        [Range(101, double.MaxValue)]
        public decimal Fees { get; set; }

    }
}
