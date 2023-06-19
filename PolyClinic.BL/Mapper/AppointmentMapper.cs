using PolyClinic.BL.Interface;

namespace PolyClinic.BL.Mapper
{
    public class AppointmentMapper : IMapper<Common.Models.Appointment, DAL.Models.Appointment>
    {
        /// <summary>
        /// Maps DAL model to Appointment model class type
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns>Returns new common Appointment model object</returns>
        public Common.Models.Appointment Map(DAL.Models.Appointment appointment)
        {
            return new Common.Models.Appointment()
            {
                AppointmentNo = appointment.AppointmentNo,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                DateofAppointment = DateOnly.FromDateTime(appointment.DateofAppointment),
                DoctorName = appointment.Doctor.DoctorName,
                PatientName = appointment.Patient.PatientName
            };
        }

        /// <summary>
        /// Maps Common Appointment model to DAL model class type
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns>Returns new DAL Appointment model object</returns>
        public DAL.Models.Appointment Map(Common.Models.Appointment appointment)
        {
            return new DAL.Models.Appointment()
            {
                AppointmentNo = appointment.AppointmentNo,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                DateofAppointment = appointment.DateofAppointment.ToDateTime(TimeOnly.MinValue)
            };
        }
    }
}
