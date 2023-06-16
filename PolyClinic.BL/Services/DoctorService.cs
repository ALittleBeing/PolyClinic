using PolyClinic.BL.Interface;
using PolyClinic.DAL;
using PolyClinic.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PolyClinic.BL.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IPolyClinicRepository _repository;

        private readonly IMapper<Common.Models.Doctor, DAL.Models.Doctor> _mapper;

        private readonly ILogger<DoctorService> _logger;
        public DoctorService(IPolyClinicRepository repository, ILogger<DoctorService> logger)
        {
            _repository = repository;
            _mapper = new Mapper.DoctorMapper();
            _logger = logger;
        }

        /// <summary>
        /// Method to fetch details of all the Doctors
        /// </summary>
        /// <returns>Returns list of Doctors</returns>
        public List<Doctor> GetAllDoctors()
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}",this.GetType());
            List<Doctor> doctors;
            try
            {
                doctors = _repository.GetAllDoctors().ConvertAll(_mapper.Map);
            }
            catch (Exception ex)
            {
                doctors = null;
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType());
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \t\tMethod: \t{name}", this.GetType());
            }
            return doctors;
        }

        /// <summary>
        /// Method to fetch details of a particular Doctor using Doctor Id
        /// </summary>
        /// <param name="doctorId">Doctor Id</param>
        /// <returns>Return details of a Doctor</returns>
        public Doctor GetDoctorDetails(string doctorId)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());
            Doctor doctor;
            try
            {
                doctor = _mapper.Map(_repository.GetDoctorDetails(doctorId));
            }
            catch (Exception ex)
            {
                doctor = null;
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: \t{name}", ex.Message, this.GetType());
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \t\tMethod: \t{name}", this.GetType());
            }
            return doctor;
        }

        /// <summary>
        /// Method to add new Doctor details
        /// </summary>
        /// <param name="doctor">Doctor object instance</param>
        /// <returns>Returns Doctor ID upon successful action</returns>
        public string AddDoctor(Doctor doctor)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            string doctorId;
            try
            {
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
        /// Method to update fees of a particular Doctor using Doctor Id
        /// </summary>
        /// <param name="doctorId">Doctor Id</param>
        /// <param name="fees">Doctor Age</param>
        /// <returns>Returns true on successful action. Otherwise, returns false</returns>
        public bool UpdateDoctorFess(string doctorId, decimal fees)
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
        /// Method to delete doctor entry using Doctor Id
        /// </summary>
        /// <param name="doctorId">Doctor Id</param>
        /// <returns>Returns true on successful action. Otherwise, returns false</returns>
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
