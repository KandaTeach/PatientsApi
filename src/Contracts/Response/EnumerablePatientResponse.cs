namespace Contracts.Response;

/// <summary>
/// A record response to store a list of patient results.
/// </summary>
public record EnumerablePatientResponse(
    IEnumerable<PatientInfoResponse> Patients
);