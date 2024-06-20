using FluentValidation;

namespace Application.Patients.Command.InsertPatient;

/// <summary>
/// Input validator for the <see cref="InsertPatientCommand"/>.
/// </summary>
public class InsertPatientCommandValidation : AbstractValidator<InsertPatientCommand>
{
    /// <summary>
    /// Contains the validation rules for <see cref="InsertPatientCommand"/>.
    /// </summary>
    public InsertPatientCommandValidation()
    {
        RuleFor(p => p.FirstName)
            .MaximumLength(50)
            .NotEmpty()
            .NotNull();

        RuleFor(p => p.LastName)
            .MaximumLength(50)
            .NotEmpty()
            .NotNull();

        RuleFor(p => p.City)
            .MaximumLength(50)
            .NotEmpty()
            .NotNull();

        RuleFor(p => p.Active)
            .MaximumLength(4)
            .Must(value =>
                value.Equals("yes", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("no", StringComparison.OrdinalIgnoreCase)
            ).WithMessage("Active must be either yes or no")
            .NotEmpty()
            .NotNull();

    }
}