using Application.Common.Interfaces.Persistence.Repository;
using Application.Patients.Common;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Patients.Query.GetPaged;

/// <summary>
/// A query handler that handles the returning of paginated list of patients.
/// </summary>
public class GetPagedQueryHandler : IRequestHandler<GetPagedQuery, ErrorOr<EnumerablePatientPageResult>>
{
    private readonly IPatientRepository _repository;

    /// <summary>
    /// Injecting the patient repository to be used in this handler class.
    /// </summary>
    /// <param name="repository">The repository for managing patient data.</param>
    public GetPagedQueryHandler(
        IPatientRepository repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Handles the process of returning paginated list of patients.
    /// </summary>
    /// <param name="request">The query request containing the patient details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    public async Task<ErrorOr<EnumerablePatientPageResult>> Handle(GetPagedQuery request, CancellationToken cancellationToken)
    {
        var patientPages = await _repository.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.Filter,
            request.SortBy
        );

        if (!patientPages.Item1.Any())
        {
            return Errors.Patient.NoPatientToBePaged;
        }

        return new EnumerablePatientPageResult(
            patientPages.Item1,
            patientPages.Item2,
            patientPages.Item3
        );
    }
}
