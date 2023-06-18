using PolyClinic.BL.Interface;
using PolyClinic.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PolyClinic.DAL.Interface;

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
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());
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
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType());
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \t\tMethod: \t{name}", this.GetType());
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
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());
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
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: \t{name}", ex.Message, this.GetType());
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \t\tMethod: \t{name}", this.GetType());
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
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

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
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: \t{name}", ex.Message, this.GetType());

            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \tMethod: \t{name}", this.GetType());
            }
            return doctorId;
        }

        /// <summary>
        /// Updates fees of a particular Doctor using Doctor Id
        /// </summary>
        /// <param name="doctorId">Doctor Id</param>
        /// <param name="fees">Doctor Age</param>
        /// <returns>true on successful action. Otherwise, false</returns>
        public bool UpdateDoctorFees(string doctorId, decimal fees)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());
            bool status = false;
            try
            {
                status = _repository.UpdateDoctorFees(doctorId, fees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: \t{name}", ex.Message, this.GetType());
                throw;
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \t\tMethod: \t{name}", this.GetType());
            }
            return status;
        }

        /// <summary>
        /// Removes a doctor instance using Doctor Id
        /// </summary>
        /// <param name="doctorId">Doctor Id</param>
        /// <returns>true if specified doctor instance is deleted. Otherwise, false</returns>
        public bool RemoveDoctor(string doctorId)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            bool status = false;
            try
            {
                status = _repository.RemoveDoctor(doctorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: \t{name}", ex.Message, this.GetType());
                throw;
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \t\tMethod: \t{name}", this.GetType());
            }
            return status;
        }

    }
}
