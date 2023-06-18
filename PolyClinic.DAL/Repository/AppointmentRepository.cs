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
    /// Repository for Appointment model
    /// </summary>
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly PolyclinicDbContext _context;
        private readonly ILogger<AppointmentRepository> _logger;
        
        /// <summary>
        /// Creates Appointment Repository instance with specified logger instance and new DbContext instance
        /// </summary>
        /// <param name="logger">ILogger instance</param>
        public AppointmentRepository(ILogger<AppointmentRepository> logger)
        {
            _context = new PolyclinicDbContext();
            _logger = logger;
        }

        #region Appointment

        /// <summary>
        /// Method to fetch all the appointments from Database
        /// </summary>
        /// <returns>List of Appointment instances if found. Otherwise, null</returns>
        public List<Appointment> GetAllAppointments()
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            List<Appointment> appointments;
            try
            {
                appointments = _context.Appointments
                .AsNoTracking()
                .Include(appointment => appointment.Doctor)
                .Include(appointment => appointment.Patient)
                .ToList();
            }
            catch (Exception ex)
            {
                appointments = null;
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType());
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \tMethod: \t{name}", this.GetType());
            }

            return appointments;

        }

        /// <summary>
        /// Method to fetch details of a particular appointment instance from Database
        /// </summary>
        /// <param name="appointmentNo">Appointment number</param>
        /// <returns>Specified Appointment instance if found. Otherwise, null</returns>
        public Appointment GetAppointment(int appointmentNo)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            Appointment appointment;
            try
            {
                appointment = _context.Appointments
                    .Include(appointment => appointment.Doctor)
                    .Include(appointment => appointment.Patient)
                    .Where(a => a.AppointmentNo == appointmentNo)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                appointment = null;
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType());
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \tMethod: \t{name}", this.GetType());
            }

            return appointment;
        }

        public string BookAppointment()
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            throw new NotImplementedException();
            //finally
            //{
            //    _logger.LogTrace(message: "[OnMethodExecuted] \tMethod: \t{name}", this.GetType());
            //}
        }

        /// <summary>
        /// Method to delete an appointment instance from Database
        /// </summary>
        /// <param name="appointmentNo">Appointment Number</param>
        /// <returns>true if specified Appointment instance is removed successfully. Otherwise, false</returns>
        public bool CancelAppointment(int appointmentNo)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            bool status = false;
            try
            {
                Appointment appointmentsObj = _context.Appointments.Find(appointmentNo);
                if (appointmentsObj != null)
                {
                    _context.Appointments.Remove(appointmentsObj);
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
