using Domain.Models;

namespace Application.Patients.Common;

/// <summary>
/// A record result to store paginated results of patient.
/// </summary>
public record EnumerablePatientPageResult(
    IEnumerable<Patient> Patients,
    int TotalItems,
    int TotalPages
);