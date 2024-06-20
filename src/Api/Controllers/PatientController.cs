using Application.Patients.Command.DeletePatient;
using Application.Patients.Command.InsertPatient;
using Application.Patients.Command.UpdatePatient;
using Application.Patients.Query.GetAllPatient;
using Application.Patients.Query.GetPaged;
using Application.Patients.Query.GetPatientById;
using Contracts.Request;
using Contracts.Response;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

///<summary>
/// A sub-controller class for managing patient related client to server operations.
///</summary>
[Route("api/patient")]
public class PatientController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Injecting dependencies to be used in <see cref="PatientController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator for sending commands and queries.</param>
    /// <param name="mapper">The mapper for mapping request and response objects.</param>
    public PatientController(
        IMediator mediator,
        IMapper mapper
    )
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Inserts a new patient.
    /// </summary>
    /// <param name="request">The request containing patient details.</param>
    /// <returns>Returns the new patient.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/patient/insert
    ///     {
    ///         "firstName": "John Keneth",
    ///         "lastName": "Paluca",
    ///         "city": "Dipolog City",
    ///         "active": "yes" -> Either yes or no. Any string value will not be accepted.
    ///     }
    /// </remarks>
    [HttpPost("insert")]
    public async Task<IActionResult> InsertPatientAsync(InsertPatientRequest request)
    {
        var command = _mapper.Map<InsertPatientCommand>(request);

        var result = await _mediator.Send(command);

        return result.Match(
            value => Ok(_mapper.Map<PatientInfoResponse>(value)),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Retrieves all patients.
    /// </summary>
    /// <returns>Returns all the patients</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/patient/get/all
    /// </remarks>
    [HttpGet("get/all")]
    public async Task<IActionResult> GetAllPatientAsync()
    {
        var query = new GetAllPatientQuery();

        var result = await _mediator.Send(query);

        return result.Match(
            value => Ok(_mapper.Map<EnumerablePatientResponse>(value)),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Gets the patient by its id.
    /// </summary>
    /// <param name="id">The patient's id.</param>
    /// <return>Returns the patient by its id.</return>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/patient/get?id=81d3373f-fa06-43c2-a53a-fd70ccce3945
    /// </remarks>
    [HttpGet("get")]
    public async Task<IActionResult> GetPatientByIdAsync([FromQuery] string id)
    {
        var query = new GetPatientByIdQuery(id);

        var result = await _mediator.Send(query);

        return result.Match(
            value => Ok(_mapper.Map<PatientInfoResponse>(value)),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Updates the patient.
    /// </summary>
    /// <param name="request">The request containing the updated patients general information.</param>
    /// <returns>Returns the patient's updated information.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/patient/update
    ///     {
    ///         "id": "81d3373f-fa06-43c2-a53a-fd70ccce3945"
    ///         "firstName": "John Ken", -> From John Keneth
    ///         "lastName": "Paluca",
    ///         "city": "Dipolog City",
    ///         "active": "yes" -> Either yes or no. Any string value will not be accepted.
    ///     }
    /// </remarks>
    [HttpPost("update")]
    public async Task<IActionResult> UpdatePatientAsync(UpdatePatientRequest request)
    {
        var command = _mapper.Map<UpdatePatientCommand>(request);

        var result = await _mediator.Send(command);

        return result.Match(
            value => Ok(_mapper.Map<PatientInfoResponse>(value)),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Remove a patient.
    /// </summary>
    /// <param name="id">The patient's id.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/patient/delete?id=81d3373f-fa06-43c2-a53a-fd70ccce3945
    /// </remarks>
    [HttpDelete("delete")]
    public async Task<IActionResult> DeletePatientAsync([FromQuery] string id)
    {
        var command = new DeletePatientCommand(id);

        var result = await _mediator.Send(command);

        return result.Match(
            value => Ok(),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Returns a paged list of patients based on the specified pagination, filter, and sorting criteria.
    /// </summary>
    /// <param name="page">The page number to return.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="filter">A string to filter the patients by their details.</param>
    /// <param name="sortBy">Sort by firstname, lastname, city, and active.</param>
    /// <returns>Returns the patients with pages and items.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     1.) GET /api/patient/get/paged -> If parameters are not set, the `page` value defaults to 1 and the `pageSize` value defaults to 10. The output is sorted by `firstname` by default.
    ///
    ///     2.) GET /api/patient/get/paged?page=1&amp;pageSize=5 -> If only `page` and `pageSize` are set, the output is sorted by `firstname` by default and returned based on the specified values of `page` and `pageSize`.
    ///
    ///     3.) GET /api/patient/get/paged?page=1&amp;pageSize=10&amp;filter=john -> If `page`, `pageSize`, and `filter` are set, the output is sorted by `firstname` by default and filtered based on the `filter` value.
    ///
    ///     4.) GET /api/patient/get/paged?page=1&amp;pageSize=10&amp;filter=john&amp;keneth&amp;sortBy=city -> If all parameters are set, the output is sorted based on the `sortBy` value and returned based on the specified values of `page` and `pageSize`.
    /// </remarks>
    [HttpGet("get/paged")]
    public async Task<IActionResult> GetPagedAsync([FromQuery]
        int page = 1,
        int pageSize = 10,
        string filter = null!,
        string sortBy = null!
    )
    {
        var query = new GetPagedQuery(
            page,
            pageSize,
            filter,
            sortBy
        );

        var result = await _mediator.Send(query);

        return result.Match(
            value => Ok(_mapper.Map<EnumerablePatientPageResponse>(value)),
            errors => Problem(errors)
        );
    }
}