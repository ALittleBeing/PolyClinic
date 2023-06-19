using Microsoft.Extensions.Logging;
using PolyClinic.BL.Interface;
using PolyClinic.Common.Models;
using PolyClinic.DAL.Interface;
using LogEvents = PolyClinic.Common.Logger.LogEvents;

namespace PolyClinic.BL.Services
{
    /// <summary>
    /// Business Logic service class for Doctor model
    /// </summary>
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;

        private readonly IMapper<Common.Models.Doctor, DAL.Models.Doctor> _mapper;

        private readonly ILogger<DoctorService> _logger;

        /// <summary>
        /// Creates Doctor service object instance with specified repository and logger instances
        /// </summary>
        /// <param name="repository">Repository instance</param>
        /// <param name="logger">ILogger instance</param>
        public DoctorService(IDoctorRepository repository, ILogger<DoctorService> logger)
        {
            _repository = repository;
            _mapper = new Mapper.DoctorMapper();
            _logger = logger;
        }

        /// <summary>
        /// Fetches details of all the Doctors
        /// </summary>
        /// <returns>List of Doctor instances if found. Otherwise, null</returns>
        public List<Doctor> GetAllDoctors()
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));
            List<Doctor> doctors = null;
            try
            {
                // Fetches all instance(s) of DAL Doctor model
                var doctorsDAO = _repository.GetAllDoctors();
                // Converts into Common Doctor model
                if (doctorsDAO != null)
                {
                    doctors = doctorsDAO.ConvertAll(_mapper.Map);
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
            return doctors;
        }

        /// <summary>
        /// Fetches details of a particular Doctor using Doctor Id
        /// </summary>
        /// <param name="doctorId">Doctor Id</param>
        /// <returns>Specified Doctor object instance if found. Otherwise, null</returns>
        public Doctor GetDoctorById(string doctorId)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));
            Doctor doctor = null;
            try
            {
                // Fetches specific instance of DAL Doctor model
                var doctorDAO = _repository.GetDoctorById(doctorId);
                // Converts into Common Doctor model using custom mapper
                if (doctorDAO != null)
                {
                    doctor = _mapper.Map(doctorDAO);
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
            return doctor;
        }

        /// <summary>
        /// Adds new Doctor details
        /// </summary>
        /// <param name="doctor">Doctor object instance</param>
        /// <returns>Doctor ID of the new instance added. Otherwise, null</returns>
        public string AddDoctor(Doctor doctor)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

            string doctorId;
            try
            {
                // Converts specified Doctor instance of Common Doctor model into DAL Doctor model using mapper
                // and calls AddDoctor method of DAL Repository to insert Doctor entry into database
                doctorId = _repository.AddDoctor(_mapper.Map(doctor));
            }
            catch (Exception ex)
            {
                doctorId = null;
                _logger.LogError(ex, message: LogEvents.ErrorMessage(ex.Message, this.GetType().FullName));

            }
            finally
            {
                _logger.LogTrace(message: LogEvents.TraceMethodExitMessage(this.GetType().FullName));
            }
            return doctorId;
        }

        /// <summary>
        /// Updates fees of a particular Doctor using Doctor Id
        /// </summary>
        /// <param name="doctorId">Doctor Id</param>
        /// <param name="fees">Doctor Age</param>
        /// <returns>1 if Fees updated successfully. 0 if Doctor is not found. Otherwise, -1</returns>
        public int UpdateDoctorFees(string doctorId, decimal fees)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));
            int status = 0;
            try
            {
                status = _repository.UpdateDoctorFees(doctorId, fees);
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

        /// <summary>
        /// Removes a doctor instance using Doctor Id
        /// </summary>
        /// <param name="doctorId">Doctor Id</param>
        /// <returns>1 if specified Doctor instance is deleted. 0 if Doctor is not found. Otherwise, -1</returns>
        public int RemoveDoctor(string doctorId)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

            int status = 0;
            try
            {
                status = _repository.RemoveDoctor(doctorId);
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
