using Application.Common.Interfaces.Persistence.Repository;
using Application.Patients.Command.InsertPatient;
using Application.Patients.Query.GetPatientById;
using Application.UnitTests.Patients.TestUtils;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;

namespace Application.UnitTests.Patients.Query.GetPatientById;

public class GetPatientByIdQueryHandlerTest
{
    private readonly GetPatientByIdQueryValidation _validation;
    private readonly Mock<IPatientRepository> _mockRepository;
    private readonly GetPatientByIdQueryHandler _getPatientHandler;
    private readonly InsertPatientCommandHandler _insertHandler;

    public GetPatientByIdQueryHandlerTest()
    {
        _validation = new GetPatientByIdQueryValidation();
        _mockRepository = new Mock<IPatientRepository>();
        _getPatientHandler = new GetPatientByIdQueryHandler(_mockRepository.Object);
        _insertHandler = new InsertPatientCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task HandleGetPatientById_WhenPatientDoesNotExist_ShouldReturnAnError()
    {
        // Arrange
        // Setup mock repository
        _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()));

        var query = GetPatientByIdQueryTestUtils.CreateQuery(Guid.NewGuid().ToString()); // generate a random guid.

        /* Then return patient */
        // Act
        var validationResult = _validation.TestValidate(query);
        var result = await _getPatientHandler.Handle(query, default);

        // Assert
        validationResult.IsValid.Should().BeTrue(); // making sure id is valid
        result.IsError.Should().BeTrue(); // making sure that is an error.
        _mockRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once); // Verify if repository was called once and get the patient.
    }

    [Fact]
    public async Task HandleGetPatientById_WhenPatientExisted_ShouldReturnThePatient()
    {
        // Arrange
        /* Setup mock repository */
        var insertPatientCommand = InsertPatientCommandTestUtils.CreateCorrectCommand();
        var insertResult = await _insertHandler.Handle(insertPatientCommand, default);

        // Setup mock repository
        _mockRepository.Setup(repo => repo.GetByIdAsync(insertResult.Value.Patient.Id))
            .ReturnsAsync(insertResult.Value.Patient);

        var query = GetPatientByIdQueryTestUtils.CreateQuery(insertResult.Value.Patient.Id.ToString());

        /* Then return patient */
        // Act
        var validationResult = _validation.TestValidate(query);
        var result = await _getPatientHandler.Handle(query, default);

        // Assert
        validationResult.IsValid.Should().BeTrue(); // making sure id is valid
        result.IsError.Should().BeFalse(); // making sure that is no errors.
        _mockRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once); // Verify if repository was called once and get the patient.
    }

    [Fact]
    public async Task HandleGetPatientById_WhenPatientIdIsNotValidGuid_ShouldReturnAnError()
    {
        // Arrange
        /* Setup mock repository */
        _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()));

        var query = GetPatientByIdQueryTestUtils.CreateQuery("jasdjiwd123123"); // invalid guid.

        /* Then return patient */
        // Act
        var validationResult = _validation.TestValidate(query);
        var result = await _getPatientHandler.Handle(query, default);

        // Assert
        validationResult.IsValid.Should().BeFalse(); // making sure id is not valid
        result.IsError.Should().BeTrue(); // making sure that is an error.
        _mockRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never); // Verify if repository not called.
    }

}