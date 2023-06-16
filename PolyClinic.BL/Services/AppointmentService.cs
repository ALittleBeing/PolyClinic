using PolyClinic.Common.Models;
using PolyClinic.BL.Interface;
using PolyClinic.DAL;


namespace PolyClinic.BL.Services
{
    /// <summary>
    /// Business Logic service for Appointment model
    /// </summary>
    public class AppointmentService : IAppointmentService
    {
        private readonly IPolyClinicRepository _repository;

        private readonly IMapper<Common.Models.Appointment, DAL.Models.Appointment> _mapper;
        public AppointmentService(IPolyClinicRepository repository)
        {
            _repository = repository;
            _mapper = new Mapper.AppointmentMapper();
        }

        /// <summary>
        /// Method to fetch all the appointments
        /// </summary>
        /// <returns>Returns list of appointments</returns>
        public List<Appointment> GetAllAppointments()
        {
            List<Appointment> appointments;
            try
            {
                appointments = _repository.GetAllAppointments().ToList().ConvertAll(_mapper.Map);
            }
            catch (Exception ex)
            {
                appointments = null;
                Console.WriteLine(ex.Message);
            }
            return appointments;
        }

        /// <summary>
        /// Method to fetch details of a particular appointment
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <returns>Return details of a patient</returns>
        public Appointment GetAppointmentByNo(int appointmentNo)
        {
            Appointment appointment;
            try
            {
                appointment = _mapper.Map(_repository.GetAppointment(appointmentNo));
            }
            catch (Exception ex)
            {
                appointment = null;
                Console.WriteLine(ex.Message);
            }
            return appointment;
        }

        /// <summary>
        /// Method to cancel an appointment
        /// </summary>
        /// <param name="appointmentNo">Appointment Number</param>
        /// <returns>Returns true on successful action. Otherwise, returns false</returns>
        public bool CancelAppointment(int appointmentNo)
        {
            bool status;
            try
            {
                status = _repository.CancelAppointment(appointmentNo);
            }
            catch (Exception ex)
            {
                status = false;
                Console.WriteLine(ex.Message);
            }
            return status;
        }
    }
}
