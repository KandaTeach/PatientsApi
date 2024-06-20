using Application.Patients.Common;
using ErrorOr;
using MediatR;

namespace Application.Patients.Query.GetPatientById;

/// <summary>
/// Query request for returning patient thru its id.
/// </summary>
/// <param name="Id">The patients id.</param>
public record GetPatientByIdQuery(
    string Id
) : IRequest<ErrorOr<PatientResult>>;