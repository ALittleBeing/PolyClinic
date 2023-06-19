using Microsoft.Extensions.Logging;
using PolyClinic.BL.Interface;
using PolyClinic.Common.Models;
using PolyClinic.DAL.Interface;
using LogEvents = PolyClinic.Common.Logger.LogEvents;

namespace PolyClinic.BL.Services
{
    /// <summary>
    /// Business Logic service class for Patient model
    /// </summary>
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;

        private readonly IMapper<Common.Models.Patient, DAL.Models.Patient> _mapper;

        private readonly ILogger<PatientService> _logger;

        /// <summary>
        /// Creates Patient service instance with specified repository and logger instances
        /// </summary>
        /// <param name="repository">Repository instance</param>
        /// <param name="logger">ILogger instance</param>
        public PatientService(IPatientRepository repository, ILogger<PatientService> logger)
        {
            _repository = repository;
            _mapper = new Mapper.PatientMapper();
            _logger = logger;
        }

        /// <summary>
        /// Fetches details of all the patients
        /// </summary>
        /// <returns>List of Patient instances if found. Otherwise, null</returns>
        public List<Patient> GetAllPatients()
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

            List<Patient> patients = null;
            try
            {
                // Fetches all instance(s) of DAL Appointment model
                var patientsDAO = _repository.GetAllPatients();
                //converts into Common Appointment model
                if (patientsDAO != null)
                {
                    patients = patientsDAO.ConvertAll(_mapper.Map);
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
            return patients;
        }

        /// <summary>
        /// Fetches details of a particular patient using patient Id
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <returns>Specified Patient instance if found. Otherwise, null</returns>
        public Patient GetPatientById(string patientId)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

            Patient patient = null;
            try
            {
                // Fetches specific instance of DAL Patient model
                var patientDAO = _repository.GetPatientById(patientId);
                //converts into Common Patient model using custom mapper
                if (patientDAO != null)
                {
                    patient = _mapper.Map(patientDAO);
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
            return patient;
        }

        /// <summary>
        /// Adds new patient details
        /// </summary>
        /// <param name="patient">Patient object instance</param>
        /// <returns>Patient ID of the new instance added. Otherwise, null</returns>
        public string AddNewPatient(Patient patient)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

            string patientId;
            try
            {
                // Converts specified Patient instance of Common Patient model into DAL Patient model using mapper
                // and calls AddNewPatientDetails method of DAL Repository to insert Patient entry into database
                patientId = _repository.AddNewPatient(_mapper.Map(patient));
            }
            catch (Exception ex)
            {
                patientId = null;
                _logger.LogError(ex, message: LogEvents.ErrorMessage(ex.Message, this.GetType().FullName));
            }
            finally
            {
                _logger.LogTrace(message: LogEvents.TraceMethodExitMessage(this.GetType().FullName));
            }
            return patientId;
        }

        /// <summary>
        /// Updates age of a particular patient using patient Id
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <param name="age">Patient Age</param>
        /// <returns>1 if Patient age is updated successfully. 0 if Patient is not found. Otherwise, -1</returns>
        public int UpdatePatientAge(string patientId, byte age)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

            int status = 0;
            if (age <= 130)
            {
                try
                {
                    status = _repository.UpdatePatientAge(patientId, age);
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
            }

            return status;
        }

        /// <summary>
        /// Deletes a Patient instance using Patient Id
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <returns>1 if specified Patient instance is deleted. 0 if Patient is not found. Otherwise, -1</returns>
        public int RemovePatient(string patientId)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

            int status = 0;
            try
            {
                status = _repository.RemovePatient(patientId);
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
