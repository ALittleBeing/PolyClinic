using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolyClinic.Common.Models;

namespace PolyClinic.API.Controllers
{
    /// <summary>
    /// Doctors Controller class
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly BL.Interface.IDoctorService _doctorService;
        private readonly ILogger<DoctorsController> _logger;

        /// <summary>
        /// Creates Doctors controller instance with injected doctor service instance
        /// </summary>
        /// <param name="doctorService"></param>
        /// <param name="logger"></param>
        public DoctorsController(BL.Interface.IDoctorService doctorService, ILogger<DoctorsController> logger)
        {
            _doctorService = doctorService;
            _logger = logger;
        }

        /// <summary>
        /// Fetches details of all the doctors
        /// </summary>
        /// <returns>Returns list of Doctors</returns>
        /// <response code="200">If GET action is successful</response>
        /// <response code="204">If doctor details are not present</response>
        /// <response code="400">If any error occurs</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<Doctor>> GetAllDoctors()
        {
            List<Doctor> doctors;
            doctors = _doctorService.GetAllDoctors();

            if (doctors != null)
            {
                _logger.LogInformation("Fetched all doctors successfully");
                return Ok(doctors);
            }
            else
            {
                _logger.LogInformation("No doctors found");
                return NoContent();
            }
        }

        /// <summary>
        /// Fetches details of a specific doctor
        /// </summary>
        /// <param name="doctorId">Doctor Id</param>
        /// <returns>Returns the details of a doctor</returns>
        /// <response code="200">Returns the doctor details if GET action is successful</response>
        /// <response code="400">If doctor Id is invalid or if any error occurs </response>
        /// <response code="404">If doctor details are not present</response>
        [HttpGet("{doctorId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Doctor> GetDoctorById(string doctorId)
        {
            if (string.IsNullOrEmpty(doctorId))
            {
                _logger.LogInformation("Invalid DoctorID provided");
                return BadRequest("Please provide valid Doctor Id");
            }
            var doctor = _doctorService.GetDoctorById(doctorId);
            if (doctor != null)
            {
                _logger.LogInformation("Fetched Doctor with ID [{doctorId}] successfully", doctor.DoctorId);
                return Ok(doctor);
            }
            else
            {
                _logger.LogInformation("Doctor with ID [{doctorId}] not found", doctorId);
                return NotFound();
            }
        }

        /// <summary>
        /// Adds a new doctor
        /// </summary>
        /// <param name="doctor"></param>
        /// <returns>Returns new doctor ID upon successful action</returns>
        /// <response code="201">Returns the new doctor Id</response>
        /// <response code="400">If any error occurs </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddNewDoctor(Doctor doctor)
        {

            string doctorId;
            doctorId = _doctorService.AddDoctor(doctor);

            if (!string.IsNullOrEmpty(doctorId))
            {
                _logger.LogInformation("Added new Doctor with ID [{doctorId}] successfully", doctorId);
                return CreatedAtAction(nameof(GetDoctorById), new { doctorId }, doctorId);
            }
            else
            {
                return BadRequest("Some error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Updates fees for a specific doctor
        /// </summary>
        /// <param name="doctorId">Doctor Id</param>
        /// <param name="fees">New fees</param>
        /// <returns>Returns Status Code 200 (Ok response) upon successful action</returns>
        /// <response code="200">If UPDATE action is successful</response>
        /// <response code="400">If doctor Id is invalid or fees less than 101 if any error occurs </response>
        /// <response code="404">If doctor details are not present</response>
        [HttpPut("{doctorId}/{fees}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateDoctorFees(string doctorId, decimal fees)
        {
            if (string.IsNullOrEmpty(doctorId))
            {
                _logger.LogInformation("Invalid DoctorID provided");
                return BadRequest("Please provide valid Doctor Id");
            }

            if (fees < 101)
            {
                _logger.LogInformation("Invalid Fees [{fees}] provided", fees);
                return BadRequest("Please provide fees greater than 101");
            }

            int status = _doctorService.UpdateDoctorFees(doctorId, fees);

            if (status == 1)
            {
                _logger.LogInformation("Fees updated for Doctor ID [{doctorId}] successfully", doctorId);
                return Ok("Doctor fees updated successfully");
            }
            else if (status == 0)
            {
                _logger.LogInformation("Doctor with ID [{doctorId}] not found", doctorId);
                return NotFound("Doctor details not found. Make sure Doctor Id is correct");
            }
            else
            {
                return BadRequest("Some error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Removes a specific doctor entry
        /// </summary>
        /// <param name="doctorId">Doctor ID</param>
        /// <returns>Returns Status Code 200 (Ok response) upon successful action</returns>
        /// <response code="200">If DELETE action is successful</response>
        /// <response code="400">If doctor Id is invalid or if any error occurs </response>
        /// <response code="404">If doctor details are not present</response>
        [HttpDelete("{doctorId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RemoveDoctor(string doctorId)
        {
            if (string.IsNullOrEmpty(doctorId))
            {
                _logger.LogInformation("Invalid DoctorID provided");
                return BadRequest("Please provide valid Doctor Id");
            }

            int status = _doctorService.RemoveDoctor(doctorId);

            if (status == 1)
            {
                _logger.LogInformation("Removed Doctor with ID [{doctorId}] successfully", doctorId);
                return Ok($"Removed Doctor (ID:{doctorId}) successfully");
            }
            else if (status == 0)
            {
                _logger.LogInformation("Doctor with ID [{doctorId}] not found", doctorId);
                return NotFound("Doctor details not found. Make sure Doctor Id is correct");
            }
            else
            {
                return BadRequest("Some error occurred. Please try again later.");
            }
        }

    }
}
