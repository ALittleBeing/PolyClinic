using Microsoft.AspNetCore.Mvc;
using PolyClinic.Common.Models;
using PolyClinic.API.Filters;

namespace PolyClinic.API.Controllers
{
    /// <summary>
    /// Appointment Controller class
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly BL.Interface.IAppointmentService _appointmentService;
        private readonly ILogger<AppointmentsController> _logger;
        /// <summary>
        /// Constructor of Appointment controller
        /// </summary>
        /// <param name="appointmentService"></param>
        /// <param name="logger"></param>
        public AppointmentsController(BL.Interface.IAppointmentService appointmentService, ILogger<AppointmentsController> logger)
        {
            _appointmentService = appointmentService;
            _logger = logger;
        }

        /// <summary>
        /// Fetchs all the appointments
        /// </summary>
        /// <returns>Returns list of appointments</returns>
        /// <response code="200">If GET action is successful</response>
        /// <response code="204">If doctor details are not present</response>
        /// <response code="400">If any error occurs</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<Appointment>> GetAllAppointments()
        {
            List<Appointment> appointments = _appointmentService.GetAllAppointments();

            if (appointments != null)
            {
                _logger.LogInformation("Fetched all Appointment successfully", appointments);
                return Ok(appointments);
            }
            else
            {
                _logger.LogInformation("No appointments found", appointments);
                return NoContent();
            }

        }

        /// <summary>
        /// Fetches a specific appointment
        /// </summary>
        /// <param name="appointmentNo">Appointment Number</param>
        /// <returns>Returns the details of an appointment</returns>
        /// <response code="200">Returns the appointment details if GET action is successful</response>
        /// <response code="400">If appointment number is invalid or if any error occurs </response>
        /// <response code="404">If appointment details are not present</response>
        [HttpGet("{appointmentNo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Doctor> GetDoctorDetails(int appointmentNo)
        {
            if (appointmentNo == 0) { return BadRequest("Please provide valid appoinment number"); }

            var appointment = _appointmentService.GetAppointmentByNo(appointmentNo);
            if (appointment != null)
            {
                return Ok(appointment);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Cancels an appointment
        /// </summary>
        /// <param name="appointmentNo">Appointment Number</param>
        /// <returns>Returns success message. Otherwise, returns error message</returns>
        /// <response code="200">If DELETE action is successful</response>
        /// <response code="400">If appointment number is invalid or if any error occurs </response>
        /// <response code="404">If appointment details are not present</response>
        [HttpDelete("{appointmentNo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CancelAppointment(int appointmentNo)
        {
            if (appointmentNo == 0) { return BadRequest("Please provide valid appoinment number"); }

            bool status = _appointmentService.CancelAppointment(appointmentNo);
            if (status)
            {
                _logger.LogInformation("Appointment Number [{appointmentNo}] cancelled successfully", appointmentNo);
                return Ok("Appointment cancelled successfully.");
            }
            else
            {
                _logger.LogInformation("Appointment Number [{appointmentNo}] NOT FOUND", appointmentNo);
                return NotFound("Appointment not found. Make sure Appointment number is correct");
            }

        }

    }
}
