using PolyClinic.DAL.Models;

namespace PolyClinic.DAL.Interface
{
    public interface IAppointmentRepository : IDisposable
    {
        int BookAppointment(Appointment appointment);
        int CancelAppointment(int appointmentNo);
        List<Appointment> GetAllAppointments();
        Appointment GetAppointment(int appointmentNo);
    }
}