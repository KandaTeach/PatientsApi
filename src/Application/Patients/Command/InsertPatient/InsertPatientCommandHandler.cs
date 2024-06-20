using Application.Common.Interfaces.Persistence.Repository;
using Application.Patients.Common;
using Domain.Models;
using ErrorOr;
using MediatR;

namespace Application.Patients.Command.InsertPatient;

/// <summary>
/// A command handler that handles the insertion of a new patient.
/// </summary>
public class InsertPatientCommandHandler : IRequestHandler<InsertPatientCommand, ErrorOr<PatientResult>>
{
    private readonly IPatientRepository _repository;

    /// <summary>
    /// Injecting the patient repository to be used in this handler class.
    /// </summary>
    /// <param name="repository">The repository for managing patient data.</param>
    public InsertPatientCommandHandler(
        IPatientRepository repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Handles the process of inserting a new patient.
    /// </summary>
    /// <param name="request">The command request containing the patient details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    public async Task<ErrorOr<PatientResult>> Handle(InsertPatientCommand request, CancellationToken cancellationToken)
    {
        var patient = Patient.Create(
            request.FirstName,
            request.LastName,
            request.City,
            request.Active.ToLower()
        );

        await _repository.InsertAsync(patient);

        return new PatientResult(
            patient
        );
    }
}