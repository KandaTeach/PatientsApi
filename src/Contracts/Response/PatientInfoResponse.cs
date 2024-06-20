namespace Contracts.Response;

/// <summary>
/// A record response to store patient result.
/// </summary>
public record PatientInfoResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string City,
    string Active
);