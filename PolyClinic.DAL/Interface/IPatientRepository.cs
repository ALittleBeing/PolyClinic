using PolyClinic.DAL.Models;

namespace PolyClinic.DAL.Interface
{
    public interface IPatientRepository: IDisposable
    {
        string AddNewPatient(Patient patientObj);
        List<Patient> GetAllPatients();
        Patient GetPatientById(string patientId);
        bool RemovePatient(string patientId);
        bool UpdatePatientAge(string patientId, byte newAge);
    }
}