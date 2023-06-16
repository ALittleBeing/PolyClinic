using PolyClinic.Common.Models;
using PolyClinic.BL.Interface;
using PolyClinic.DAL;

namespace PolyClinic.BL.Services
{
    /// <summary>
    /// Business Logic service for Patient model
    /// </summary>
    public class PatientService : IPatientService
    {
        private readonly IPolyClinicRepository _repository;

        private readonly IMapper<Common.Models.Patient, DAL.Models.Patient> _mapper;
        public PatientService(IPolyClinicRepository repository)
        {
            _repository = repository;
            _mapper = new Mapper.PatientMapper();
        }

        /// <summary>
        /// Method to fetch details of all the patients
        /// </summary>
        /// <returns>Returns list of Patients</returns>
        public List<Patient> GetAllPatientDetails()
        {
            List<Patient> patients;
            try
            {
                patients = _repository.GetAllPatientDetails().ConvertAll(_mapper.Map);
            }
            catch (Exception ex)
            {
                patients = null;
                Console.WriteLine(ex.Message);
            }
            return patients;
        }

        /// <summary>
        /// Method to fetch details of a particular patient using patient Id
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <returns>Return details of a patient</returns>
        public Patient GetPatientDetails(string patientId)
        {
            Patient patient;
            try
            {
                patient = _mapper.Map(_repository.GetPatientDetails(patientId));
            }
            catch (Exception ex)
            {
                patient = null;
                Console.WriteLine(ex.Message);
            }
            return patient;
        }

        /// <summary>
        /// Method to add new patient details
        /// </summary>
        /// <param name="patient"></param>
        /// <returns>Returns patient ID upon successful action</returns>
        public string AddNewPatientDetails(Patient patient)
        {
            string patientId;
            try
            {
                patientId = _repository.AddNewPatientDetails(_mapper.Map(patient));
            }
            catch (Exception ex)
            {
                patientId = null;
                Console.WriteLine(ex.Message);
            }
            return patientId;
        }

        /// <summary>
        /// Method to update age of a particular patient using patient Id
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <param name="age">Patient Age</param>
        /// <returns>Returns true on successful action. Otherwise, returns false</returns>
        public bool UpdatePatientAge(string patientId, byte age)
        {
            bool status = false;
            if (age <= 130)
            {
                try
                {
                    status = _repository.UpdatePatientAge(patientId, age);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            
            return status;
        }

        /// <summary>
        /// Method to delete Patient entry using Patient Id
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <returns>Returns true on successful action. Otherwise, returns false</returns>
        public bool RemovePatient(string patientId)
        {
            bool status;
            try
            {
                status = _repository.RemovePatient(patientId);
            }
            catch (Exception)
            {
                throw;
            }

            return status;
        }
    }
}
