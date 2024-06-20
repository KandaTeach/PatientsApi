using Application.Common.Interfaces.Persistence.Repository;
using Application.Patients.Common;
using Domain.Errors;
using Domain.Models;
using ErrorOr;
using MediatR;

namespace Application.Patients.Command.UpdatePatient;

/// <summary>
/// A command handler that handles the updating of a patient's details.
/// </summary>
public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, ErrorOr<PatientResult>>
{
    private readonly IPatientRepository _repository;

    /// <summary>
    /// Injecting the patient repository to be used in this handler class.
    /// </summary>
    /// <param name="repository">The repository for managing patient data.</param>
    public UpdatePatientCommandHandler(
        IPatientRepository repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Handles the process of updating a patient.
    /// </summary>
    /// <param name="request">The command request containing the patient details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    public async Task<ErrorOr<PatientResult>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
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

        var updatedPatient = Patient.Create(
            request.FirstName,
            request.LastName,
            request.City,
            request.Active
        );

        var patientUpdated = await _repository.UpdateAsync(patient!, updatedPatient);

        return new PatientResult(
            patientUpdated
        );
    }
}
