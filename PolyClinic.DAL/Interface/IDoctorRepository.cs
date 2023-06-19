using PolyClinic.DAL.Models;

namespace PolyClinic.DAL.Interface
{
    public interface IDoctorRepository : IDisposable
    {
        string AddDoctor(Doctor doctor);
        List<Doctor> GetAllDoctors();
        Doctor GetDoctorById(string doctorId);
        int RemoveDoctor(string doctorId);
        int UpdateDoctorFees(string doctorId, decimal fees);
    }
}