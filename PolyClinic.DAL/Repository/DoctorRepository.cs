using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PolyClinic.DAL.Interface;
using PolyClinic.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyClinic.DAL.Repository
{
    /// <summary>
    /// Repository class for Doctor model
    /// </summary>
    public class DoctorRepository : IDoctorRepository
    {
        private readonly PolyclinicDbContext _context;
        private readonly ILogger<DoctorRepository> _logger;

        /// <summary>
        /// Creates Doctor Repository object instance with specified logger instance and new DbContext instance
        /// </summary>
        /// <param name="logger">ILogger instance</param>
        public DoctorRepository(ILogger<DoctorRepository> logger)
        {
            _context = new PolyclinicDbContext();
            _logger = logger;
        }

        #region Doctor

        /// <summary>
        /// Fetches details of all the Doctors from database
        /// </summary>
        /// <returns>List of Doctor instances if found. Otherwise, null</returns>
        public List<Doctor> GetAllDoctors()
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            List<Doctor> doctors;
            try
            {
                doctors = _context.Doctors.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                doctors = null;
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType());
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \tMethod: \t{name}", this.GetType());
            }

            return doctors;
        }
        
        /// <summary>
        /// Fetches details of a particular Doctor using Doctor Id from database
        /// </summary>
        /// <param name="doctorId">Doctor Id</param>
        /// <returns>Specified Doctor object instance if found. Otherwise, null</returns>
        public Doctor GetDoctorById(string doctorId)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            Doctor doctor;
            try
            {
                doctor = _context.Doctors.Find(doctorId);
            }
            catch (Exception ex)
            {
                doctor = null;
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType());
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \tMethod: \t{name}", this.GetType());
            }

            return doctor;
        }

        /// <summary>
        /// Inserts new Doctor details into the database
        /// </summary>
        /// <param name="doctor">Doctor object instance</param>
        /// <returns>Doctor ID of the new instance added. Otherwise, null</returns>
        public string AddDoctor(Doctor doctor)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            string newDoctorId;
            try
            {
                newDoctorId = GenerateNewDoctorId();
                if (!string.IsNullOrEmpty(newDoctorId))
                {
                    doctor.DoctorId = newDoctorId;
                    _context.Doctors.Add(doctor);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newDoctorId = null;
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType());
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \tMethod: \t{name}", this.GetType());
            }

            return newDoctorId;
        }

        /// <summary>
        /// Generates new Doctor Id
        /// </summary>
        ///<remarks>
        /// Fetches last DoctorID from database and generates new DoctorID by incrementing it. 
        /// Returns D1 if no existing IDs found
        ///</remarks>
        /// <returns>A new Doctor Id. Otherwise, null</returns>
        private string GenerateNewDoctorId()
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            string newDoctorId;
            try
            {
                var doctors = _context.Doctors.AsNoTracking().ToList();
                if (doctors != null)
                {
                    var doctorIds = doctors.ConvertAll(p => Convert.ToUInt16(p.DoctorId[1..].TrimEnd()));
                    doctorIds.Sort();
                    var lastId = doctorIds.Last();                    
                    newDoctorId = "D" + (lastId + 1);
                }
                else
                {
                    newDoctorId = "D1";
                }
            }
            catch (Exception ex)
            {
                newDoctorId = null;
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType());
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \tMethod: \t{name}", this.GetType());
            }

            return newDoctorId;
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
                Doctor doctor = _context.Doctors.Find(doctorId);
                if (doctor != null)
                {
                    doctor.Fees = fees;
                    _context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType());
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \tMethod: \t{name}", this.GetType());
            }

            return status;
        }

        /// <summary>
        /// Removes a doctor instance using Doctor Id from database
        /// </summary>
        /// <param name="doctorId">Doctor Id</param>
        /// <returns>true if specified doctor instance is deleted. Otherwise, false</returns>
        public bool RemoveDoctor(string doctorId)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            bool status = false;
            try
            {
                Doctor doctor = _context.Doctors.Find(doctorId);
                if (doctor != null)
                {
                    _context.Doctors.Remove(doctor);
                    _context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType());
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \tMethod: \t{name}", this.GetType());
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
