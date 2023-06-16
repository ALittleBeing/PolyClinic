using PolyClinic.Common.Models;

namespace PolyClinic.BL.Interface
{
    public interface IDoctorService
    {
        string AddDoctor(Doctor doctor);
        List<Doctor> GetAllDoctors();
        Doctor GetDoctorDetails(string doctorId);
        bool UpdateDoctorFess(string doctorId, decimal fees);
        bool RemoveDoctor(string doctorId);
    }
}