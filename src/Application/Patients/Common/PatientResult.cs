using Domain.Models;

namespace Application.Patients.Common;

/// <summary>
/// A record result to store patient result.
/// </summary>
public record PatientResult(
    Patient Patient
);