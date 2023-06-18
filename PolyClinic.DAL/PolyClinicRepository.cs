using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PolyClinic.DAL.Models;

namespace PolyClinic.DAL
{
    public class PolyClinicRepository : IPolyClinicRepository
    {
        private readonly PolyclinicDbContext context;
        private readonly ILogger<PolyClinicRepository> _logger;
        public PolyClinicRepository(ILogger<PolyClinicRepository> logger)
        {
            context = new PolyclinicDbContext();
            _logger = logger;
        }

        #region Patient
        public List<Patient> GetAllPatientDetails()
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            List<Patient> patientlist;
            try
            {
                patientlist = (from patient in context.Patients orderby patient.PatientId select patient).AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                patientlist = null;
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType());
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \tMethod: \t{name}", this.GetType());
            }

            return patientlist;

        }

        public Patient GetPatientDetails(string patientId)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            Patient patientDetails;
            try
            {
                patientDetails = context.Patients.Where(p => p.PatientId == patientId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                patientDetails = null;
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType());
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \tMethod: \t{name}", this.GetType());
            }

            return patientDetails;
        }

        public string AddNewPatientDetails(Patient patientObj)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            string newPatientId;
            try
            {
                newPatientId = GenerateNewPatientId();
                if (!string.IsNullOrEmpty(newPatientId))
                {
                    patientObj.PatientId = newPatientId;
                    context.Patients.Add(patientObj);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newPatientId = null;
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType());
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \tMethod: \t{name}", this.GetType());
            }

            return newPatientId;

        }

        public string GenerateNewPatientId()
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            string newPatientId;
            try
            {
                var lastPatient = context.Patients.OrderByDescending(p => p.PatientId).ToList().FirstOrDefault();
                if (lastPatient != null)
                {
                    newPatientId = "P" + (Convert.ToUInt16(lastPatient.PatientId[1..].TrimEnd()) + 1);
                }
                else
                {
                    newPatientId = "P1";
                }
            }
            catch (Exception ex)
            {
                newPatientId = null;
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType());
            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \tMethod: \t{name}", this.GetType());
            }

            return newPatientId;
        }

        public bool UpdatePatientAge(string patientId, byte newAge)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            bool status = false;
            try
            {
                Patient patientObj = context.Patients.Find(patientId);

                if (patientObj != null)
                {
                    patientObj.Age = newAge;
                    context.SaveChanges();
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

        public bool RemovePatient(string patientId)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            bool status = false;
            try
            {
                Patient patient = context.Patients.Find(patientId);
                if (patient != null)
                {
                    context.Patients.Remove(patient);
                    context.SaveChanges();
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

        #region Appointment


        public List<Appointment> GetAllAppointments()
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            List<Appointment> appointments;
            try
            {
                appointments = context.Appointments.AsNoTracking().ToList();
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

        public Appointment GetAppointment(int appointmentNo)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            Appointment appointment;
            try
            {
                appointment = context.Appointments
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

        public bool CancelAppointment(int appointmentNo)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            bool status = false;
            try
            {
                Appointment appointmentsObj = context.Appointments.Find(appointmentNo);
                if (appointmentsObj != null)
                {
                    context.Appointments.Remove(appointmentsObj);
                    context.SaveChanges();
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

        #region Doctor
        public List<Doctor> GetAllDoctors()
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            List<Doctor> doctors;
            try
            {
                doctors = context.Doctors.AsNoTracking().ToList();
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

        public Doctor GetDoctorDetails(string doctorId)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            Doctor doctor;
            try
            {
                doctor = context.Doctors.Find(doctorId);
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
                    context.Doctors.Add(doctor);
                    context.SaveChanges();
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
        public string GenerateNewDoctorId()
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            string newDoctorId;
            try
            {
                var lastDoctor = context.Doctors.OrderByDescending(d => d.DoctorId).ToList().FirstOrDefault();
                if (lastDoctor != null)
                {
                    newDoctorId = "D" + (Convert.ToUInt16(lastDoctor.DoctorId[1..].TrimEnd()) + 1);
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

        public bool UpdateDoctorFees(string doctorId, decimal fees)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            bool status = false;
            try
            {
                Doctor doctor = context.Doctors.Find(doctorId);
                if (doctor != null)
                {
                    doctor.Fees = fees;
                    context.SaveChanges();
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

        public bool RemoveDoctor(string doctorId)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType());

            bool status = false;
            try
            {
                Doctor doctor = context.Doctors.Find(doctorId);
                if (doctor != null)
                {
                    context.Doctors.Remove(doctor);
                    context.SaveChanges();
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
    }
}
