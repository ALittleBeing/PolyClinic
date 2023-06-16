using Microsoft.EntityFrameworkCore;
using PolyClinic.DAL.Models;

namespace PolyClinic.DAL
{
    public class PolyClinicRepository : IPolyClinicRepository
    {
        readonly PolyclinicDbContext context;
        public PolyClinicRepository()
        {
            context = new PolyclinicDbContext();
        }

        #region Patient
        public List<Patient> GetAllPatientDetails()
        {
            List<Patient> patientlist;
            try
            {
                patientlist = (from patient in context.Patients orderby patient.PatientId select patient).AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                patientlist = null;
                Console.WriteLine(ex.Message);
            }

            return patientlist;

        }

        public Patient GetPatientDetails(string patientId)
        {
            Patient patientDetails;
            try
            {
                patientDetails = context.Patients.Where(p => p.PatientId == patientId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                patientDetails = null;
                Console.WriteLine(ex.Message);
            }
            return patientDetails;
        }

        public string AddNewPatientDetails(Patient patientObj)
        {
            string newPatientId;
            try
            {
                newPatientId = GenerateNewPatientId();
                Console.WriteLine("newPatientId: {0}", newPatientId);
                patientObj.PatientId = newPatientId;
                context.Patients.Add(patientObj);
                context.SaveChanges();
                //newPatientId = patientObj.PatientId;
            }
            catch (Exception ex)
            {
                newPatientId = null;
                Console.WriteLine(ex.StackTrace);
                throw;
            }

            return newPatientId;

        }

        public bool UpdatePatientAge(string patientId, byte newAge)
        {
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
                Console.WriteLine(ex.Message);
                //status = false;
            }

            return status;
        }

        public bool RemovePatient(string patientId)
        {
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
                Console.WriteLine(ex.Message);
            }

            return status;
        }

        public string GenerateNewPatientId()
        {
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
            catch (Exception)
            {
                //Console.WriteLine(ex.Message);
                throw;
            }
            return newPatientId;
        }
        #endregion

        #region Appointment


        public List<Appointment> GetAllAppointments()
        {
            List<Appointment> appointments;
            try
            {
                appointments = context.Appointments.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                appointments = null;
                Console.WriteLine(ex.Message);
            }
            return appointments;

        }

        public Appointment GetAppointment(int appointmentNo)
        {
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
                Console.WriteLine(ex.Message);
            }

            return appointment;
        }

        public string BookAppointment()
        {
            throw new NotImplementedException();
        }

        public bool CancelAppointment(int appointmentNo)
        {
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
                Console.WriteLine(ex.Message);
            }

            return status;
        }
        #endregion

        #region Doctor
        public List<Doctor> GetAllDoctors()
        {
            List<Doctor> doctors;
            try
            {
                doctors = context.Doctors.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                doctors = null;
                Console.WriteLine(ex.Message);
            }
            return doctors;
        }

        public Doctor GetDoctorDetails(string doctorId)
        {
            Doctor doctor;
            try
            {
                doctor = context.Doctors.Find(doctorId);
            }
            catch (Exception ex)
            {
                doctor = null;
                Console.WriteLine(ex.Message);
            }
            return doctor;
        }

        public string AddDoctor(Doctor doctor)
        {
            string newDoctorId;
            try
            {
                newDoctorId = GenerateNewDoctorId();
                Console.WriteLine("newDoctorId: {0}", newDoctorId);

                doctor.DoctorId = newDoctorId;
                context.Doctors.Add(doctor);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                newDoctorId = null;
                Console.WriteLine(ex.InnerException);
                throw;
            }

            return newDoctorId;
        }
        public bool UpdateDoctorFees(string doctorId, decimal fees)
        {
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
                Console.WriteLine(ex.Message);
            }

            return status;
        }

        public bool RemoveDoctor(string doctorId)
        {
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
                Console.WriteLine(ex.Message);
            }

            return status;
        }

        public string GenerateNewDoctorId()
        {
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
            catch (Exception)
            {
                //Console.WriteLine(ex.Message);
                throw;
            }
            return newDoctorId;
        }
        #endregion
    }
}
