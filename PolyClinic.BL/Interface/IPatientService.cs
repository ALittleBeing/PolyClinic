using PolyClinic.Common.Models;

namespace PolyClinic.BL.Interface
{
    public interface IPatientService
    {
        List<Patient> GetAllPatientDetails();
        Patient GetPatientDetails(string id);
        string AddNewPatientDetails(Patient patient);
        bool UpdatePatientAge(string patientId, byte age);
        bool RemovePatient(string patientId);
        
    }
}
