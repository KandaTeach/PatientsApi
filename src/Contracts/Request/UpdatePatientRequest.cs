namespace Contracts.Request;

/// <summary>
/// A record request to update patient.
/// </summary>
public record UpdatePatientRequest(
    string Id,
    string FirstName,
    string LastName,
    string City,
    string Active
);