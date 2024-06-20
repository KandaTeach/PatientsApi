using Domain.Models;

namespace Application.Common.Interfaces.Persistence.Repository
{
    /// <summary>
    /// Interface for patient repository to manage patient data.
    /// </summary>
    public interface IPatientRepository
    {
        /// <summary>
        /// Gets all patients.
        /// </summary>
        /// <returns>Returns the list of all patients.</returns>
        Task<IEnumerable<Patient>> GetAllAsync();

        /// <summary>
        /// Gets a patient by their id.
        /// </summary>
        /// <param name="id">The unique identifier of the patient.</param>
        /// <returns>Returns the patient with the id.</returns>
        Task<Patient> GetByIdAsync(Guid id);

        /// <summary>
        /// Add a new patient into the datasource.
        /// </summary>
        /// <param name="patient">The patient data to insert.</param>
        Task InsertAsync(Patient patient);

        /// <summary>
        /// Updates an existing patient in the datasource.
        /// </summary>
        /// <param name="patient">The patients to be updated.</param>
        /// <param name="updatedPatient">The patients updated information.</param>
        Task<Patient> UpdateAsync(Patient patient, Patient updatedPatient);

        /// <summary>
        /// Deletes a patient from the datasource.
        /// </summary>
        /// <param name="patient">The patient to be deleted.</param>
        Task DeleteAsync(Patient patient);

        /// <summary>
        /// Gets a paginated list of patients, with optional filtering and sorting.
        /// </summary>
        /// <param name="page">The page number to get.</param>
        /// <param name="pageSize">The number of patients per page.</param>
        /// <param name="filter">The filter to apply to the patient data.</param>
        /// <param name="sortBy">The field to sort the results.</param>
        /// <returns>
        /// Returns the total number of filtered items, and the total number of pages.
        /// </returns>
        Task<(IEnumerable<Patient>, int, int)> GetPagedAsync(
            int page,
            int pageSize,
            string filter,
            string sortBy
        );
    }
}
