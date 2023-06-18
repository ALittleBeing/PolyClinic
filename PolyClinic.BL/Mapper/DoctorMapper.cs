using PolyClinic.BL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyClinic.BL.Mapper
{
    internal class DoctorMapper : IMapper<Common.Models.Doctor, DAL.Models.Doctor>
    {
        /// <summary>
        /// Maps DAL model to Doctor model class type
        /// </summary>
        /// <param name="doctor"></param>
        /// <returns>Returns new common Doctor model object</returns>
        public Common.Models.Doctor Map(DAL.Models.Doctor doctor)
        {
            return new Common.Models.Doctor()
            {
                DoctorId = doctor.DoctorId.TrimEnd(),
                DoctorName = doctor.DoctorName,
                Specialization = doctor.Specialization,
                Fees = doctor.Fees
            };
        }

        /// <summary>
        /// Maps Common Doctor model to DAL model class type
        /// </summary>
        /// <param name="doctor"></param>
        /// <returns>Returns new DAL Doctor model object</returns>
        public DAL.Models.Doctor Map(Common.Models.Doctor doctor)
        {
            return new DAL.Models.Doctor()
            {
                DoctorId = doctor.DoctorId,
                DoctorName = doctor.DoctorName,
                Specialization = doctor.Specialization,
                Fees = doctor.Fees
            };
        }
    }
}
