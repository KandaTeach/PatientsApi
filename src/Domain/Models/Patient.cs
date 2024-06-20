namespace Domain.Models;

/// <summary>
/// This model represents the patient.
/// The model is designed following the rich domain model instead of using the anemic domain model, which is considered an anti-pattern.
/// </summary>
public sealed class Patient
{
    /// <summary>
    /// Gets the patients unique identifier.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the patients first name.
    /// </summary>
    public string FirstName { get; private set; }

    /// <summary>
    /// Gets the patients last name.
    /// </summary>
    public string LastName { get; private set; }

    /// <summary>
    /// Gets the city where the patients resides.
    /// </summary>
    public string City { get; private set; }

    /// <summary>
    /// Gets the indicator whether the patients is active or not.
    /// </summary>
    public string Active { get; private set; }

    /// <summary>
    /// Initializes a new instance of <see cref="Patient"> class.
    /// </summary>
    /// <param name="id">The patients unique identifier.</param>
    /// <param name="firstName">The patients first name.</param>
    /// <param name="lastName">The patients last name.</param>
    /// <param name="city">The city where the patients resides.</param>
    /// <param name="active">A value indicator whether the patients is active.</param>
    private Patient(
        Guid id,
        string firstName,
        string lastName,
        string city,
        string active
    )
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        City = city;
        Active = active;
    }

    /// <summary>
    /// Creates a new instance of <see cref="Patient"> class.
    /// </summary>
    /// <param name="firstName">The patients first name.</param>
    /// <param name="lastName">The patients last name.</param>
    /// <param name="city">The city where the patients resides.</param>
    /// <param name="active">A value indicator whether the patients is active.</param>
    /// <returns>A new <see cref="Patient"/> instance.</returns>
    public static Patient Create(
        string firstName,
        string lastName,
        string city,
        string active
    )
    {
        return new(
            Guid.NewGuid(),
            firstName,
            lastName,
            city,
            active
        );
    }

    /// <summary>
    /// Updates the patients information.
    /// </summary>
    /// <param name="firstName">The patients first name.</param>
    /// <param name="lastName">The patients last name.</param>
    /// <param name="city">The city where the patients resides.</param>
    /// <param name="active">A value indicator whether the patients is active.</param>
    public void Update(
        string? firstName = null,
        string? lastName = null,
        string? city = null,
        string? active = null
    )
    {
        if (!string.IsNullOrEmpty(firstName))
        {
            FirstName = firstName;
        }

        if (!string.IsNullOrEmpty(lastName))
        {
            LastName = lastName;
        }

        if (!string.IsNullOrEmpty(city))
        {
            City = city;
        }

        if (!string.IsNullOrEmpty(active))
        {
            Active = active;
        }

    }
}