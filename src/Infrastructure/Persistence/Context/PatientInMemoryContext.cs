using Domain.Models;

namespace Infrastructure.Persistence.Context
{
    /// <summary>
    /// The patient's In-memory context.
    /// </summary>
    public class PatientInMemoryContext
    {
        private readonly List<Patient> _patients = new();

        /// <summary>
        /// Gets the list of patients as read-only list.
        /// </summary>
        public IReadOnlyList<Patient> Patients => _patients.AsReadOnly();

        /// <summary>
        /// Inserts a new patient into the context.
        /// </summary>
        /// <param name="patient">The patient to insert.</param>
        public void Insert(Patient patient)
        {
            _patients.Add(patient);
        }

        /// <summary>
        /// Updates an existing patient in the context.
        /// </summary>
        /// <param name="id">The unique identifier of the patient.</param>
        /// <param name="updatedPatient">The patient with updated information.</param>
        public Patient Update(Patient patient, Patient updatedPatient)
        {
            patient.Update(
                updatedPatient.FirstName,
                updatedPatient.LastName,
                updatedPatient.City,
                updatedPatient.Active
            );

            return patient;
        }

        /// <summary>
        /// Deletes a patient from the context.
        /// </summary>
        /// <param name="id">The unique identifier of the patient.</param>
        public void Delete(Patient patient)
        {
            _patients.Remove(patient);
        }
    }
}
