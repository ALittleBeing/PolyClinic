using Microsoft.Extensions.Logging;
using PolyClinic.BL.Interface;
using PolyClinic.Common.Models;
using PolyClinic.DAL.Interface;
using LogEvents = PolyClinic.Common.Logger.LogEvents;

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
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

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
                _logger.LogError(ex, message: LogEvents.ErrorMessage(ex.Message, this.GetType().FullName));
            }
            finally
            {
                _logger.LogTrace(message: LogEvents.TraceMethodExitMessage(this.GetType().FullName));
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
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

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
                _logger.LogError(ex, message: LogEvents.ErrorMessage(ex.Message, this.GetType().FullName));

            }
            finally
            {
                _logger.LogTrace(message: LogEvents.TraceMethodExitMessage(this.GetType().FullName));
            }
            return appointment;
        }

        /// <summary>
        /// Adds new Appointment into the database
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <param name="doctorId">Doctor Id</param>
        /// <param name="dateofAppointment">Date/Time of Appointment</param>
        /// <returns>New Appointment number if new Appointment gets added successfully. Otherwise, -1</returns>
        public int BookAppointment(Appointment appointment)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));
            int newAppointmentNo = -1;
            try
            {
                var appointmentDAO = _mapper.Map(appointment);
                newAppointmentNo = _repository.BookAppointment(appointmentDAO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: LogEvents.ErrorMessage(ex.Message, this.GetType().FullName));
            }
            finally
            {
                _logger.LogTrace(message: LogEvents.TraceMethodExitMessage(this.GetType().FullName));
            }

            return newAppointmentNo;
        }

        /// <summary>
        /// Removes an appointment instance
        /// </summary>
        /// <param name="appointmentNo">Appointment Number</param>
        /// <returns>1 if specified Appointment instance is removed successfully. 0 if Appointment is not found. Otherwise, -1</returns>
        public int CancelAppointment(int appointmentNo)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

            int status = 0;
            try
            {
                status = _repository.CancelAppointment(appointmentNo);
            }
            catch (Exception ex)
            {
                status = -1;
                _logger.LogError(ex, message: LogEvents.ErrorMessage(ex.Message, this.GetType().FullName));
            }
            finally
            {
                _logger.LogTrace(message: LogEvents.TraceMethodExitMessage(this.GetType().FullName));
            }

            return status;
        }

    }
}
