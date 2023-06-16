using PolyClinic.BL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolyClinic.Common.Models;
using PolyClinic.DAL.Models;

namespace PolyClinic.BL.Mapper
{
    public class PatientMapper : IMapper<Common.Models.Patient, DAL.Models.Patient>
    {
        /// <summary>
        /// Maps DAL model to Patient model class type
        /// </summary>
        /// <param name="patient"></param>
        /// <returns>Returns new common Patient model object</returns>
        public Common.Models.Patient Map(DAL.Models.Patient patient)
        {
            return new Common.Models.Patient
            {
                PatientId = patient.PatientId,
                PatientName = patient.PatientName,
                Age = patient.Age,
                Gender = patient.Gender,
                ContactNumber = patient.ContactNumber,
            };
        }

        /// <summary>
        /// Maps Common Patient model to DAL model class type
        /// </summary>
        /// <param name="patient"></param>
        /// <returns>Returns new DAL Patient model object</returns>
        public DAL.Models.Patient Map(Common.Models.Patient patient)
        {
            return new DAL.Models.Patient
            {
                PatientId = patient.PatientId,
                PatientName = patient.PatientName,
                Age = patient.Age,
                Gender = patient.Gender,
                ContactNumber = patient.ContactNumber
            };
        }
    }
}
