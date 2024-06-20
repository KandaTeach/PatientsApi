using Application.Patients.Common;
using ErrorOr;
using MediatR;

namespace Application.Patients.Command.InsertPatient;

/// <summary>
/// Command request for inserting a new patient.
/// </summary>
/// <param name="FirstName">The first name of the patient.</param>
/// <param name="LastName">The last name of the patient.</param>
/// <param name="City">The city where the patient resides.</param>
/// <param name="Active">A value indicating whether the patient is active.</param>
public record InsertPatientCommand(
    string FirstName,
    string LastName,
    string City,
    string Active
) : IRequest<ErrorOr<PatientResult>>;