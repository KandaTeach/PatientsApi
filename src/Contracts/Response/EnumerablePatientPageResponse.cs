namespace Contracts.Response;

/// <summary>
/// A record response to store paginated results of patients.
/// </summary>
public record EnumerablePatientPageResponse(
    IEnumerable<PatientInfoResponse> Patients,
    int TotalItems,
    int TotalPages
);