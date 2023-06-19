using PolyClinic.DAL.Models;

namespace PolyClinic.DAL.Interface
{
    public interface IPatientRepository : IDisposable
    {
        string AddNewPatient(Patient patientObj);
        List<Patient> GetAllPatients();
        Patient GetPatientById(string patientId);
        int RemovePatient(string patientId);
        int UpdatePatientAge(string patientId, byte newAge);
    }
}