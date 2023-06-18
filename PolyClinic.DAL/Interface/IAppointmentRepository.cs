using PolyClinic.DAL.Models;

namespace PolyClinic.DAL.Interface
{
    public interface IAppointmentRepository:IDisposable
    {
        string BookAppointment();
        bool CancelAppointment(int appointmentNo);
        List<Appointment> GetAllAppointments();
        Appointment GetAppointment(int appointmentNo);
    }
}