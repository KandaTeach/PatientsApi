using FluentValidation;

namespace Application.Patients.Query.GetPatientById;

/// <summary>
/// Input validator for the <see cref="GetPatientByIdQuery"/>.
/// </summary>
public class GetPatientByIdQueryValidation : AbstractValidator<GetPatientByIdQuery>
{
    /// <summary>
    /// Contains the validation rules for <see cref="GetPatientByIdQuery"/>
    /// </summary>
    public GetPatientByIdQueryValidation()
    {
        RuleFor(p => p.Id)
            .NotEmpty()
            .NotNull()
            .Must(IsIdGuid).WithMessage("Id must be a valid GUID");
    }

    /// <summary>
    /// Checks if the id is a valid GUID.
    ///</summary>
    private bool IsIdGuid(string id)
    {
        return Guid.TryParse(id, out _);
    }
}