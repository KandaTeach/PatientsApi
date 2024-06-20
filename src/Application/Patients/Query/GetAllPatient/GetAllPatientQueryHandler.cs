using Application.Common.Interfaces.Persistence.Repository;
using Application.Patients.Common;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Patients.Query.GetAllPatient;

/// <summary>
/// A query handler that handles the returning all patients.
/// </summary>
public class GetAllPatientQueryHandler : IRequestHandler<GetAllPatientQuery, ErrorOr<EnumerablePatientResult>>
{
    private readonly IPatientRepository _repository;

    /// <summary>
    /// Injecting the patient repository to be used in this handler class.
    /// </summary>
    /// <param name="repository">The repository for managing patient data.</param>
    public GetAllPatientQueryHandler(
        IPatientRepository repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Handles the process of returning all patients.
    /// </summary>
    /// <param name="request">The query request containing the patient details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    public async Task<ErrorOr<EnumerablePatientResult>> Handle(GetAllPatientQuery request, CancellationToken cancellationToken)
    {
        var patients = await _repository.GetAllAsync();

        if (!patients.Any())
        {
            return Errors.Patient.DataSourceIsEmpty;
        }

        return new EnumerablePatientResult(
            patients
        );
    }
}