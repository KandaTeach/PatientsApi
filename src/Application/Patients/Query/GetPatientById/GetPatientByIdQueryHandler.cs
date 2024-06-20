using Application.Common.Interfaces.Persistence.Repository;
using Application.Patients.Common;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Patients.Query.GetPatientById;

/// <summary>
/// A query handler that handles the returning of patient thru its id.
/// </summary>
public class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, ErrorOr<PatientResult>>
{
    private readonly IPatientRepository _repository;

    /// <summary>
    /// Injecting the patient repository to be used in this handler class.
    /// </summary>
    /// <param name="repository">The repository for managing patient data.</param>
    public GetPatientByIdQueryHandler(
        IPatientRepository repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Handles the process of returning the patient thru its id.
    /// </summary>
    /// <param name="request">The query request containing the patient details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    public async Task<ErrorOr<PatientResult>> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
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

        return new PatientResult(
            patient!
        );
    }
}
