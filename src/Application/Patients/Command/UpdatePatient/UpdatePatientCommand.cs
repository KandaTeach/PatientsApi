using Application.Patients.Common;
using ErrorOr;
using MediatR;

namespace Application.Patients.Command.UpdatePatient;

/// <summary>
/// Command request for updating a patient's details.
/// </summary>
/// <param name="FirstName">The first name of the patient.</param>
/// <param name="LastName">The last name of the patient.</param>
/// <param name="City">The city where the patient resides.</param>
/// <param name="Active">A value indicating whether the patient is active. Active should be either yes or no.</param>
public record UpdatePatientCommand(
    string Id,
    string FirstName,
    string LastName,
    string City,
    string Active
) : IRequest<ErrorOr<PatientResult>>;