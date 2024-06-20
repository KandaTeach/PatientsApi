using Application.Common.Interfaces.Persistence.Repository;
using Domain.Models;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repository
{
    /// <summary>
    /// Repository for managing patient data.
    /// </summary>
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientInMemoryContext _context;

        /// <summary>
        /// Injecting the in-memory context to be used in this repository class.
        /// </summary>
        /// <param name="context">The in-memory context for storing patient data.</param>
        public PatientRepository(PatientInMemoryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Inserts a new patient into the datasource.
        /// </summary>
        /// <param name="patient">The patient to insert.</param>
        public Task InsertAsync(Patient patient)
        {
            _context.Insert(patient);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Updates an existing patient in the datasource.
        /// </summary>
        /// <param name="id">The unique identifier of the patient.</param>
        /// <param name="updatedPatient">The patient with updated information.</param>
        public Task<Patient> UpdateAsync(Patient patient, Patient updatedPatient)
        {
            var patientUpdated = _context.Update(patient, updatedPatient);

            return Task.FromResult(patientUpdated);
        }

        /// <summary>
        /// Deletes a patient from the datasource.
        /// </summary>
        /// <param name="patient">The patient to be deleted.</param>
        public Task DeleteAsync(Patient patient)
        {
            _context.Delete(patient);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Returns all patients from the repository.
        /// </summary>
        public Task<IEnumerable<Patient>> GetAllAsync()
        {
            return Task.FromResult(_context.Patients.AsEnumerable());
        }

        /// <summary>
        /// Returns a patient by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the patient.</param>
        /// <returns>Returns the patient with the specified unique identifier.</returns>
        public Task<Patient> GetByIdAsync(Guid id)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(patient!);
        }

        /// <summary>
        /// Returns a paged list of patients based on the specified pagination, filter, and sorting criteria.
        /// </summary>
        /// <param name="page">The page number to return. Default value is 1.</param>
        /// <param name="pageSize">The number of items per page. Default value is 10.</param>
        /// <param name="filter">A string to filter the patients by their details.</param>
        /// <param name="sortBy">Sort by firstname, lastname, city, and active.</param>
        /// <returns>Returns the patients with pages and items.</returns>
        public Task<(IEnumerable<Patient>, int, int)> GetPagedAsync(
            int page = 1, int pageSize = 10, string filter = null!, string sortBy = null!)
        {
            var query = _context.Patients.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(p =>
                    p.FirstName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                    p.LastName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                    p.City.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                    p.Active.ToString().Equals(filter, StringComparison.OrdinalIgnoreCase));
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "firstname" => query.OrderBy(p => p.FirstName),
                    "lastname" => query.OrderBy(p => p.LastName),
                    "active" => query.OrderBy(p => p.Active),
                    "city" => query.OrderBy(p => p.City),
                    _ => query.OrderBy(p => p.FirstName),
                };
            }
            else
            {
                // Default sorting (optional)
                query = query.OrderBy(p => p.FirstName);
            }

            // Paging
            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var patients = query.Skip((page - 1) * pageSize).Take(pageSize);

            return Task.FromResult((patients.AsEnumerable(), totalItems, totalPages));
        }
    }
}
