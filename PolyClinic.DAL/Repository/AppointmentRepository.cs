using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PolyClinic.DAL.Interface;
using PolyClinic.DAL.Models;
using LogEvents = PolyClinic.Common.Logger.LogEvents;

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
        /// Fetches all the appointments from Database
        /// </summary>
        /// <returns>List of Appointment instances if found. Otherwise, null</returns>
        public List<Appointment> GetAllAppointments()
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

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
                _logger.LogError(ex.InnerException ?? ex, message: LogEvents.ErrorMessage(ex.InnerException?.Message ?? ex.Message, this.GetType().FullName));
            }
            finally
            {
                _logger.LogTrace(message: LogEvents.TraceMethodExitMessage(this.GetType().FullName));
            }

            return appointments;

        }

        /// <summary>
        /// Fetches details of a particular appointment instance from Database
        /// </summary>
        /// <param name="appointmentNo">Appointment number</param>
        /// <returns>Specified Appointment instance if found. Otherwise, null</returns>
        public Appointment GetAppointment(int appointmentNo)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

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
                _logger.LogError(ex.InnerException ?? ex, message: LogEvents.ErrorMessage(ex.InnerException?.Message ?? ex.Message, this.GetType().FullName));
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
        /// <param name="appointment">Appointment instance</param>
        /// <returns>New Appointment number if new Appointment gets added successfully. -2 if Appointment already exists for either Doctor or Patient. - 3 if PatientID/DoctorID is/are invalid. Otherwise, -1</returns>
        public int BookAppointment(Appointment newAppointment)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));
            int newAppointmentNo;
            try
            {
                // Check if there is appointment for given PatientId or DoctorId with same datetime 
                var isOccupied = _context.Appointments.Any(a =>
                    a.DateofAppointment.Equals(newAppointment.DateofAppointment)
                    && (a.DoctorId.Equals(newAppointment.DoctorId)
                    || a.PatientId.Equals(newAppointment.PatientId)));

                // If no appointment found for given PatientId or DoctorId with same datetime 
                if (!isOccupied)
                {
                    _context.Appointments.Add(newAppointment);
                    _context.SaveChanges();
                    newAppointmentNo = newAppointment.AppointmentNo;
                }
                else
                {
                    newAppointmentNo = -2;
                }

            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("conflicted with the FOREIGN KEY constraint") ?? false)
            {
                //Handling Foreign key conflict issue
                newAppointmentNo = -3;
                //Microsoft.Data.SqlClient.SqlException (0x80131904): The INSERT statement conflicted with the FOREIGN KEY constraint "fk_PatientID". The conflict occurred in database "PolyclinicDB", table "dbo.Patients", column 'PatientID'.
                _logger.LogError(ex.InnerException, message: LogEvents.ErrorMessage(ex.InnerException.Message, this.GetType().FullName));
            }
            catch (Exception ex) // Logging other exceptions
            {
                newAppointmentNo = -1;
                _logger.LogError(ex.InnerException ?? ex, message: LogEvents.ErrorMessage(ex.InnerException?.Message ?? ex.Message, this.GetType().FullName));
            }
            finally
            {
                _logger.LogTrace(message: LogEvents.TraceMethodExitMessage(this.GetType().FullName));
            }

            return newAppointmentNo;
        }

        /// <summary>
        /// Deletes an appointment instance from Database
        /// </summary>
        /// <param name="appointmentNo">Appointment Number</param>
        /// <returns>1 if specified Appointment instance is removed successfully. 0 if Appointment is not found. Otherwise, -1</returns>
        public int CancelAppointment(int appointmentNo)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

            int status = 0;
            try
            {
                Appointment appointmentsObj = _context.Appointments.Find(appointmentNo);
                if (appointmentsObj != null)
                {
                    _context.Appointments.Remove(appointmentsObj);
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
