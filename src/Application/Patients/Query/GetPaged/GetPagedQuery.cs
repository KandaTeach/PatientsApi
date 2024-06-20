using Application.Patients.Common;
using ErrorOr;
using MediatR;

namespace Application.Patients.Query.GetPaged;

/// <summary>
/// Query request for returning a paginated list of patients.
/// </summary>
/// <param name="Page">The page number to return.</param>
/// <param name="PageSize">The number of items per page.</param>
/// <param name="Filter">A string to filter the patients by their details.</param>
/// <param name="SortBy">Sort by firstname, lastname, city, and active.</param>
public record GetPagedQuery(
    int Page,
    int PageSize,
    string Filter,
    string SortBy
) : IRequest<ErrorOr<EnumerablePatientPageResult>>;