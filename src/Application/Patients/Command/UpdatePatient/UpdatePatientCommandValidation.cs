using FluentValidation;

namespace Application.Patients.Command.UpdatePatient;

/// <summary>
/// Input validator for the <see cref="UpdatePatientCommand"/>.
/// </summary>
public class UpdatePatientCommandValidation : AbstractValidator<UpdatePatientCommand>
{
    /// <summary>
    /// Contains the validation rules for <see cref="UpdatePatientCommand"/>
    /// </summary>
    public UpdatePatientCommandValidation()
    {
        RuleFor(p => p.Id)
            .NotEmpty()
            .NotNull()
            .Must(IsIdGuid).WithMessage("Id must be a valid GUID");

        RuleFor(p => p.FirstName)
            .MaximumLength(50);

        RuleFor(p => p.LastName)
            .MaximumLength(50);

        RuleFor(p => p.City)
            .MaximumLength(50);

        RuleFor(p => p.Active)
            .MaximumLength(4)
            .Must(value => value == "yes" || value == "no").WithMessage("Active must be either yes or no");

    }

    /// <summary>
    /// Checks if the id is a valid GUID.
    ///</summary>
    private bool IsIdGuid(string id)
    {
        return Guid.TryParse(id, out _);
    }
}