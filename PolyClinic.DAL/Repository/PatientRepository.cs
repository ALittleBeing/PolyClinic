using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PolyClinic.DAL.Interface;
using PolyClinic.DAL.Models;
using LogEvents = PolyClinic.Common.Logger.LogEvents;

namespace PolyClinic.DAL.Repository
{
    /// <summary>
    /// Business Logic service class for Patient model
    /// </summary>
    public class PatientRepository : IPatientRepository
    {
        private readonly PolyclinicDbContext _context;
        private readonly ILogger<PatientRepository> _logger;

        /// <summary>
        /// Creates Patient Repository instance with specified logger instance and new DbContext instance
        /// </summary>
        /// <param name="logger">ILogger instance</param>
        public PatientRepository(ILogger<PatientRepository> logger)
        {
            _context = new PolyclinicDbContext();
            _logger = logger;
        }

        #region Patient

        /// <summary>
        /// Fetches details of all the patients
        /// </summary>
        /// <returns>List of Patient instances if found. Otherwise, null</returns>
        public List<Patient> GetAllPatients()
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

            List<Patient> patientlist;
            try
            {
                patientlist = (from patient in _context.Patients orderby patient.PatientId select patient).AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                patientlist = null;
                _logger.LogError(ex.InnerException ?? ex, message: LogEvents.ErrorMessage(ex.InnerException?.Message ?? ex.Message, this.GetType().FullName));
            }
            finally
            {
                _logger.LogTrace(message: LogEvents.TraceMethodExitMessage(this.GetType().FullName));
            }

            return patientlist;

        }

        /// <summary>
        /// Fetches details of a particular patient using patient Id
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <returns>Specified Patient instance if found. Otherwise, null</returns>
        public Patient GetPatientById(string patientId)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

            Patient patientDetails;
            try
            {
                patientDetails = _context.Patients.Where(p => p.PatientId == patientId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                patientDetails = null;
                _logger.LogError(ex.InnerException ?? ex, message: LogEvents.ErrorMessage(ex.InnerException?.Message ?? ex.Message, this.GetType().FullName));
            }
            finally
            {
                _logger.LogTrace(message: LogEvents.TraceMethodExitMessage(this.GetType().FullName));
            }

            return patientDetails;
        }

        /// <summary>
        /// Adds new patient details into the database
        /// </summary>
        /// <param name="patient">Patient object instance</param>
        /// <returns>Patient ID of the new instance added. Otherwise, null</returns>
        public string AddNewPatient(Patient patientObj)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

            string newPatientId;
            try
            {
                newPatientId = GenerateNewPatientId();
                if (!string.IsNullOrEmpty(newPatientId))
                {
                    patientObj.PatientId = newPatientId;
                    _context.Patients.Add(patientObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newPatientId = null;
                _logger.LogError(ex.InnerException ?? ex, message: LogEvents.ErrorMessage(ex.InnerException?.Message ?? ex.Message, this.GetType().FullName));
            }
            finally
            {
                _logger.LogTrace(message: LogEvents.TraceMethodExitMessage(this.GetType().FullName));
            }

            return newPatientId;

        }

        private string GenerateNewPatientId()
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

            string newPatientId;
            try
            {
                var patients = _context.Patients.AsNoTracking().ToList();

                if (patients != null)
                {
                    var patientIds = patients.ConvertAll(p => Convert.ToUInt16(p.PatientId[1..].TrimEnd()));
                    patientIds.Sort();
                    var lastId = patientIds.Last();
                    newPatientId = "P" + (lastId + 1);
                }
                else
                {
                    newPatientId = "P1";
                }
            }
            catch (Exception ex)
            {
                newPatientId = null;
                _logger.LogError(ex.InnerException ?? ex, message: LogEvents.ErrorMessage(ex.InnerException?.Message ?? ex.Message, this.GetType().FullName));
            }
            finally
            {
                _logger.LogTrace(message: LogEvents.TraceMethodExitMessage(this.GetType().FullName));
            }

            return newPatientId;
        }

        /// <summary>
        /// Updates age of a particular patient using patient Id
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <param name="age">Patient Age</param>
        /// <returns>1 if Patient age is updated successfully. 0 if Patient is not found. Otherwise, -1</returns>
        public int UpdatePatientAge(string patientId, byte newAge)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

            int status = 0;
            try
            {
                Patient patientObj = _context.Patients.Find(patientId);

                if (patientObj != null)
                {
                    patientObj.Age = newAge;
                    _context.SaveChanges();
                    status = 1;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                _logger.LogError(ex.InnerException ?? ex, message: LogEvents.ErrorMessage(ex.InnerException?.Message ?? ex.Message, this.GetType().FullName));
            }
            finally
            {
                _logger.LogTrace(message: LogEvents.TraceMethodExitMessage(this.GetType().FullName));
            }

            return status;
        }

        /// <summary>
        /// Deletes a Patient instance using Patient Id from database
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <returns>1 if specified Patient instance is deleted. 0 if Patient is not found. Otherwise, -1</returns>
        public int RemovePatient(string patientId)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

            int status = 0;
            try
            {
                Patient patient = _context.Patients.Find(patientId);
                if (patient != null)
                {
                    _context.Patients.Remove(patient);
                    _context.SaveChanges();
                    status = 1;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                _logger.LogError(ex.InnerException ?? ex, message: LogEvents.ErrorMessage(ex.InnerException?.Message ?? ex.Message, this.GetType().FullName));
            }
            finally
            {
                _logger.LogTrace(message: LogEvents.TraceMethodExitMessage(this.GetType().FullName));
            }

            return status;
        }

        #endregion

        #region Dispose
        private bool disposed = false;

        /// <summary>
        /// Disposes the DbContext instance created
        /// </summary>
        /// <param name="disposing"></param>
        /// <returns></returns>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Calls Dispose method
        /// </summary>
        /// <returns></returns>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
