using Application.Common.Interfaces.Persistence.Repository;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Patients.Command.DeletePatient;

/// <summary>
/// A command handler that handles the deletion of a patient.
/// </summary>
public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, ErrorOr<Unit>>
{
    private readonly IPatientRepository _repository;

    /// <summary>
    /// Injecting the patient repository to be used in this handler class.
    /// </summary>
    /// <param name="repository">The repository for managing patient data.</param>
    public DeletePatientCommandHandler(
        IPatientRepository repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Handles the process of deleting a patient.
    /// </summary>
    /// <param name="request">The command request containing the patient id.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    public async Task<ErrorOr<Unit>> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        var IsIdGuid = Guid.TryParse(request.Id, out Guid parsedId);

        if (!IsIdGuid)
        {
            return Errors.Patient.InvalidPatientId;
        }

        var patient = await _repository.GetByIdAsync(parsedId);

        if (patient == null)
        {
            return Errors.Patient.PatientNotFound;
        }

        await _repository.DeleteAsync(patient!);

        return Unit.Value;
    }
}
