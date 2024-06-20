using Domain.Models;

namespace Application.Patients.Common;

/// <summary>
/// A record result to store a list of patients.
/// </summary>
public record EnumerablePatientResult(
    IEnumerable<Patient> Patients
);