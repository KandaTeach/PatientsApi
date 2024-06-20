namespace Contracts.Request;

/// <summary>
/// A record request to insert patient.
/// </summary>
public record InsertPatientRequest(
    string FirstName,
    string LastName,
    string City,
    string Active
);