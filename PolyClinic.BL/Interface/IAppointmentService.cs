using PolyClinic.Common.Models;

namespace PolyClinic.BL.Interface
{
    public interface IAppointmentService
    {
        List<Appointment> GetAllAppointments();
        Appointment GetAppointmentByNo(int appointmentNo);
        bool CancelAppointment(int appointmentNo);
    }
}