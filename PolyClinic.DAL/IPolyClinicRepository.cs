using PolyClinic.DAL.Models;

namespace PolyClinic.DAL
{
    public interface IPolyClinicRepository
    {
        #region Patient
        string AddNewPatientDetails(Patient patientObj);
        List<Patient> GetAllPatientDetails();
        Patient GetPatientDetails(string patientId);
        bool UpdatePatientAge(string patientId, byte newAge);

        bool RemovePatient(string patientId);
        #endregion

        #region Appointment
        List<Appointment> GetAllAppointments();
        Appointment GetAppointment(int appointmentNo);
        string BookAppointment();
        bool CancelAppointment(int appointmentNo);
        #endregion

        #region Doctor
        List<Doctor> GetAllDoctors();
        Doctor GetDoctorDetails(string doctorId);

        bool UpdateDoctorFees(string doctorId, decimal fees);
        string AddDoctor(Doctor doctor);
        bool RemoveDoctor(string doctorId);
        #endregion
    }
}