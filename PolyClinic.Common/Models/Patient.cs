using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PolyClinic.Common.Models
{
    public class Patient
    {
        
        public string PatientId { get; set; }
        [Required]
        [MinLength(3)]
        public string PatientName { get; set; }
        [Required]
        [Range(1, 130)]
        public byte Age { get; set; }
        [Required]
        [RegularExpression("^[MF]$")]
        public string Gender { get; set; }
        [Required]
        [Phone]
        public string ContactNumber { get; set; }


    }
}
