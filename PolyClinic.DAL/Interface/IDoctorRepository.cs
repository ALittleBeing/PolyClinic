using PolyClinic.DAL.Models;

namespace PolyClinic.DAL.Interface
{
    public interface IDoctorRepository: IDisposable
    {
        string AddDoctor(Doctor doctor);
        List<Doctor> GetAllDoctors();
        Doctor GetDoctorById(string doctorId);
        bool RemoveDoctor(string doctorId);
        bool UpdateDoctorFees(string doctorId, decimal fees);
    }
}