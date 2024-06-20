using FluentValidation;

namespace Application.Patients.Command.DeletePatient;

/// <summary>
/// Input validator for the <see cref="DeletePatientCommand"/>.
/// </summary>
public class DeletePatientCommandValidation : AbstractValidator<DeletePatientCommand>
{
    /// <summary>
    /// Contains the validation rules for <see cref="DeletePatientCommand"/>.
    /// </summary>
    public DeletePatientCommandValidation()
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