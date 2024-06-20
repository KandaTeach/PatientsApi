using ErrorOr;
using MediatR;

namespace Application.Patients.Command.DeletePatient;

/// <summary>
/// Command request for deleting a patient.
/// </summary>
/// <param name="Id">The patients id.</param>
public record DeletePatientCommand(
    string Id
) : IRequest<ErrorOr<Unit>>;