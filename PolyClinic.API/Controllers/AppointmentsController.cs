using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolyClinic.Common.Models;

namespace PolyClinic.API.Controllers
{
    /// <summary>
    /// Appointment Controller class
    /// </summary>
    [Authorize]
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
        /// <response code="400">If any error occurs</response>
        /// <response code="204">If appointment is not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<Appointment>> GetAllAppointments()
        {
            List<Appointment> appointments = _appointmentService.GetAllAppointments();

            if (appointments != null)
            {
                _logger.LogInformation("Fetched all Appointments successfully");
                return Ok(appointments);
            }
            else
            {
                _logger.LogInformation("No appointments found");
                return NotFound("No appointments found");
            }

        }

        /// <summary>
        /// Fetches a specific appointment with appoinment number
        /// </summary>
        /// <param name="appointmentNumber">Appointment Number</param>
        /// <returns>Returns the details of an appointment</returns>
        /// <response code="200">Returns the appointment details if GET action is successful</response>
        /// <response code="400">If appointment number is invalid or if any error occurs </response>
        /// <response code="404">If appointment is not found</response>
        [HttpGet("{appointmentNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Doctor> GetAppointment(int appointmentNumber)
        {
            if (appointmentNumber == 0)
            {
                _logger.LogInformation("Invalid Appointment Number [{appointmentNo}] provided", appointmentNumber);
                return BadRequest("Please provide valid appoinment number");
            }

            var appointment = _appointmentService.GetAppointmentByNo(appointmentNumber);
            if (appointment != null)
            {
                _logger.LogInformation("Fetched Appointment Number [{appointmentNo}] successfully", appointment.AppointmentNo);
                return Ok(appointment);
            }
            else
            {
                _logger.LogInformation("Appointment number [{appointmentNo}] not found", appointment.AppointmentNo);
                return NotFound();
            }
        }

        /// <summary>
        /// Books a new appointment
        /// </summary>
        /// <param name="appointment">Appointment model instance</param>
        /// <returns>Returns new Appointment number. Otherwise, returns error message</returns>
        /// <response code="201">If Appointment is booked successfully</response>
        /// <response code="400">If Patient/Doctor ID is/are invalid or if any error occurs </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult BookAppointment(Appointment appointment)
        {

            int newAppointmentNo = _appointmentService.BookAppointment(appointment);
            if (newAppointmentNo > 0)
            {
                _logger.LogInformation("Appointment Number [{appointmentNo}] has been booked successfully", newAppointmentNo);
                return CreatedAtAction(nameof(GetAppointment), new { appointmentNumber = newAppointmentNo }, appointment);
            }
            else if (newAppointmentNo == -2)
            {
                _logger.LogInformation("Appointment already exist for given Patient ID or Doctor ID  on the same date/time.");
                return BadRequest("Appointment already exist for given Patient ID or Doctor ID  on the same date/time.");
            }
            else if (newAppointmentNo == -3)
            {
                _logger.LogInformation("Invalid Patient ID/Doctor ID. provided");
                return BadRequest("Invalid Patient ID/Doctor ID.");
            }
            else
            {
                return BadRequest("Some error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Cancels an appointment
        /// </summary>
        /// <param name="appointmentNumber">Appointment Number</param>
        /// <returns>Returns success message. Otherwise, returns error message</returns>
        /// <response code="200">If DELETE action is successful</response>
        /// <response code="400">If appointment number is invalid or if any error occurs </response>
        /// <response code="404">If appointment details are not present</response>
        [HttpDelete("{appointmentNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CancelAppointment(int appointmentNumber)
        {
            if (appointmentNumber < 1)
            {
                _logger.LogInformation("Invalid Appointment Number [{appointmentNo}] provided", appointmentNumber);
                return BadRequest("Please provide valid appoinment number");
            }

            int status = _appointmentService.CancelAppointment(appointmentNumber);
            if (status == 1)
            {
                _logger.LogInformation("Appointment Number [{appointmentNo}] has been cancelled successfully", appointmentNumber);
                return Ok("Appointment cancelled successfully.");
            }
            else if (status == 0)
            {
                _logger.LogInformation("Appointment Number [{appointmentNo}] not found", appointmentNumber);
                return NotFound("Appointment not found. Make sure Appointment number is correct");
            }
            else
            {
                return BadRequest("Some error occurred. Please try again later.");
            }
        }

    }
}
