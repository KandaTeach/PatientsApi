using ErrorOr;

namespace Domain.Errors;

/// <summary>
/// Static partial class containing error definitions.
/// </summary>
public static partial class Errors
{
    /// <summary>
    /// Contains error definitions for patient-related operations.
    /// </summary>
    public static class Patient
    {

        /// <summary>
        /// Error indicating that a patient was not found.
        /// </summary>
        public static Error PatientNotFound =>
            Error.NotFound(
                code: "Patient.PatientNotFound",
                description: "Patient does not exist."
            );


        /// <summary>
        /// Error indicating that the datasource is empty.
        /// </summary>
        public static Error DataSourceIsEmpty =>
            Error.Conflict(
                code: "Patient.DataSourceIsEmpty",
                description: "In-memory data source is empty."
            );

        /// <summary>
        /// Error indicating that there are no patients available to be paged.
        /// </summary>
        public static Error NoPatientToBePaged =>
            Error.Conflict(
                code: "Patient.NoPatientToBePaged",
                description: "No patient to be paged."
            );

        /// <summary>
        /// Error indicating that id is not a valid GUID.
        /// </summary>
        public static Error InvalidPatientId =>
            Error.Unexpected(
                code: "Patient.InvalidPatientId",
                description: "Patient id is not a valid guid."
            );
    }
}