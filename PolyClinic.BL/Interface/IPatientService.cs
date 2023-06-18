using PolyClinic.Common.Models;

namespace PolyClinic.BL.Interface
{
    public interface IPatientService
    {
        List<Patient> GetAllPatients();
        Patient GetPatientById(string patientId);
        string AddNewPatient(Patient patient);
        bool UpdatePatientAge(string patientId, byte age);
        bool RemovePatient(string patientId);
    }
}
