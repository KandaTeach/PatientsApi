using Application.Patients.Common;
using ErrorOr;
using MediatR;

namespace Application.Patients.Query.GetAllPatient;

/// <summary>
/// Query request for returning all patients.
/// </summary>
public record GetAllPatientQuery() : IRequest<ErrorOr<EnumerablePatientResult>>;