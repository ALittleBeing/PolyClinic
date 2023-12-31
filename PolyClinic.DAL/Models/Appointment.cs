﻿namespace PolyClinic.DAL.Models;

public partial class Appointment
{
    public int AppointmentNo { get; set; }

    public string PatientId { get; set; } = null!;

    public string DoctorId { get; set; } = null!;

    public DateTime DateofAppointment { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
