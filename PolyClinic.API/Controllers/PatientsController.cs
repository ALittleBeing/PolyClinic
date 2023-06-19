using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolyClinic.Common.Models;

namespace PolyClinic.API.Controllers
{
    /// <summary>
    /// Patient Controller class
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly BL.Interface.IPatientService _patientService;
        private readonly ILogger<PatientsController> _logger;
        /// <summary>
        /// Constructor of Patient controller
        /// </summary>
        /// <param name="service">BL service injected through Dependency Injection</param>
        /// <param name="logger"></param>
        public PatientsController(BL.Interface.IPatientService service, ILogger<PatientsController> logger)
        {
            _patientService = service;
            _logger = logger;
        }

        /// <summary>
        /// Fetches details of all the patients
        /// </summary>
        /// <returns>Returns list of Patients</returns>
        /// <response code="200">If GET action is successful</response>
        /// <response code="204">If patient details are not present</response>
        /// <response code="400">If any error occurs</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<Patient>> GetAllPatients()
        {
            List<Patient> patients;
            patients = _patientService.GetAllPatients();

            if (patients != null)
            {
                _logger.LogInformation("Fetched all patients successfully");
                return Ok(patients);
            }
            else
            {
                _logger.LogInformation("No patients found");
                return NoContent();
            }
        }

        /// <summary>
        /// Fetchs details of a specific patient
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <returns>Returns the patient details</returns>
        /// <response code="200">Returns the patient details if GET action is successful</response>
        /// <response code="400">If Patient Id is invalid or if any error occurs </response>
        /// <response code="404">If patient details are not present</response>
        [HttpGet("{patientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Patient> GetPatientById(string patientId)
        {
            if (string.IsNullOrEmpty(patientId))
            {
                _logger.LogInformation("Invalid PatientId provided");
                return BadRequest("Please provide valid Patient Id");
            }
            var patient = _patientService.GetPatientById(patientId);
            if (patient != null)
            {
                _logger.LogInformation("Fetched Patient with ID [{patientId}] successfully", patientId);
                return Ok(patient);
            }
            else
            {
                _logger.LogInformation("Patient with ID [{patientId}] not found", patientId);
                return NotFound();
            }
        }

        /// <summary>
        /// Adds a new patient
        /// </summary>
        /// <param name="patient"></param>
        /// <returns>Returns patient ID upon successful action</returns>
        /// <response code="201">Returns the new patient Id</response>
        /// <response code="400">If any error occurs </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddNewPatient([FromBody] Patient patient)
        {

            string patientId;
            patientId = _patientService.AddNewPatient(patient);

            if (!string.IsNullOrEmpty(patientId))
            {
                _logger.LogInformation("Added new Patient with ID [{patientId}] successfully", patientId);
                return CreatedAtAction(nameof(GetPatientById), new { patientId }, patientId);
            }
            else
            {
                return BadRequest("Some error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Updates age of a specific patient
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <param name="age">Patient Age</param>
        /// <returns>Returns Status Code 200 upon successful action</returns>
        /// <response code="200">If UPDATE action is successful</response>
        /// <response code="400">If Patient Id is invalid or if any error occurs </response>
        /// <response code="404">If patient details are not present</response>
        [HttpPut("{patientId}/{age}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePatientAge(string patientId, byte age)
        {
            if (string.IsNullOrEmpty(patientId))
            {
                _logger.LogInformation("Invalid PatientId provided");
                return BadRequest("Please provide valid Patient Id");
            }

            int status = _patientService.UpdatePatientAge(patientId, age);

            if (status == 1)
            {
                _logger.LogInformation("Age updated for Patient ID [{patientId}] successfully", patientId);
                return Ok("Patient age updated successfully");
            }
            else if (status == 0)
            {
                _logger.LogInformation("Patient with ID [{patientId}] not found", patientId);
                return NotFound("Patient details not found. Make sure Patient Id is correct");
            }
            else
            {
                return BadRequest("Some error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Deletes a specific Patient entry
        /// </summary>
        /// <param name="patientId">Patient ID</param>
        /// <returns>Returns Status Code 200 (Ok response) upon successful action</returns>
        /// <response code="200">If DELETE action is successful</response>
        /// <response code="400">If Patient Id is invalid or if any error occurs </response>
        /// <response code="404">If patient details are not present</response>
        [HttpDelete("{patientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RemovePatient(string patientId)
        {
            if (string.IsNullOrEmpty(patientId))
            {
                _logger.LogInformation("Invalid PatientId provided");
                return BadRequest("Please provide valid Patient Id");
            }

            int status = _patientService.RemovePatient(patientId);

            if (status == 1)
            {
                _logger.LogInformation("Removed Patient with ID [{patientId}] successfully", patientId);
                return Ok($"Removed Patient (ID:{patientId}) successfully");
            }
            else if (status == 0)
            {
                _logger.LogInformation("Patient with ID [{patientId}] not found", patientId);
                return NotFound("Patient details not found. Make sure Patient Id is correct");
            }
            else
            {
                return BadRequest("Some error occurred. Please try again later.");
            }
        }

    }
}
