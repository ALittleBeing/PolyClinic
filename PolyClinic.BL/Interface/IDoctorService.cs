using PolyClinic.Common.Models;

namespace PolyClinic.BL.Interface
{
    public interface IDoctorService
    {
        string AddDoctor(Doctor doctor);
        List<Doctor> GetAllDoctors();
        Doctor GetDoctorById(string doctorId);
        int UpdateDoctorFees(string doctorId, decimal fees);
        int RemoveDoctor(string doctorId);
    }
}