using PolyClinic.Common.Models;
using PolyClinic.BL.Interface;
using Microsoft.Extensions.Logging;
using PolyClinic.DAL.Interface;

namespace PolyClinic.BL.Services
{
    /// <summary>
    /// Business Logic service for Appointment model
    /// </summary>
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;
        private readonly IMapper<Common.Models.Appointment, DAL.Models.Appointment> _mapper;
        private readonly ILogger<AppointmentService> _logger;

        /// <summary>
        /// Creates Appointment service instance with specified repository and logger instances
        /// </summary>
        /// <param name="repository">Repository instance</param>
        /// <param name="logger">ILogger instance</param>
        public AppointmentService(IAppointmentRepository repository, ILogger<AppointmentService> logger)
        {
            _repository = repository;
            _mapper = new Mapper.AppointmentMapper();
            _logger = logger;
        }

        /// <summary>
        /// Fetches all the appointments
        /// </summary>
        /// <returns>List of Appointment instances if found. Otherwise, null</returns>
        public List<Appointment> GetAllAppointments()
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            List<Appointment> appointments = null;
            try
            {
                // Fetches all instance(s) of DAL Appointment model
                var appointmentsDAO = _repository.GetAllAppointments();
                // Converts into Common Appointment model
                if (appointmentsDAO != null)
                {
                    appointments = appointmentsDAO.ConvertAll(_mapper.Map);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType());
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \t\tMethod: \t{name}", this.GetType());
            }
            return appointments;
        }

        /// <summary>
        /// Fetches details of a particular appointment
        /// </summary>
        /// <param name="appointmentNo">Appointment number</param>
        /// <returns>Specified Appointment instance if found. Otherwise, null</returns>
        public Appointment GetAppointmentByNo(int appointmentNo)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            Appointment appointment = null;
            try
            {
                // Fetches specific instance of DAL Appointment model
                var appointmentDAO = _repository.GetAppointment(appointmentNo);
                // Converts into Common Appointment model using custom mapper
                if (appointmentDAO != null)
                {
                    appointment = _mapper.Map(appointmentDAO);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType());

            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \t\tMethod: \t{name}", this.GetType());
            }
            return appointment;
        }

        /// <summary>
        /// Removes an appointment instance
        /// </summary>
        /// <param name="appointmentNo">Appointment Number</param>
        /// <returns>true if specified Appointment instance removed successfully. Otherwise, false</returns>
        public bool CancelAppointment(int appointmentNo)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            bool status;
            try
            {
                status = _repository.CancelAppointment(appointmentNo);
            }
            catch (Exception ex)
            {
                status = false;
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType());
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \t\tMethod: \t{name}", this.GetType());
            }

            return status;
        }

    }
}
