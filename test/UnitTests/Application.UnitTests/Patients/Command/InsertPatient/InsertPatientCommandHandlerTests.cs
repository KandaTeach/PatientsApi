using Application.Common.Interfaces.Persistence.Repository;
using Application.Patients.Command.InsertPatient;
using Application.UnitTests.Patients.TestUtils;
using Domain.Models;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;

namespace Application.UnitTests.Patients.Command.InsertPatient;

public class InsertPatientCommandHandlerTests
{
    private readonly InsertPatientCommandValidation _validation;
    private readonly InsertPatientCommandHandler _handler;
    private readonly Mock<IPatientRepository> _mockRepository;

    public InsertPatientCommandHandlerTests()
    {
        _validation = new InsertPatientCommandValidation();
        _mockRepository = new Mock<IPatientRepository>();
        _handler = new InsertPatientCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task HandleInsertPatientCommand_WhenPatientDetailsAreAllCorrect_ShouldCreateAndReturnPatient()
    {
        // Arrange
        var insertPatientCommand = InsertPatientCommandTestUtils.CreateCorrectCommand();

        // Act
        var validationResult = _validation.TestValidate(insertPatientCommand);
        var result = await _handler.Handle(insertPatientCommand, default);

        // Assert
        validationResult.IsValid.Should().BeTrue(); // making sure that inputs are valid
        result.IsError.Should().BeFalse(); // making sure that there is no errors.
        _mockRepository.Verify(repo => repo.InsertAsync(It.IsAny<Patient>()), Times.Once); // Verify if repository was called once and add the patient.
    }

    [Fact]
    public async Task HandleInsertPatientCommand_WhenPatientDetailsNotCorrect_ShouldReturnAnError()
    {
        // Arrange
        var insertPatientCommand = InsertPatientCommandTestUtils.CreateIncorrectCommand();

        // Act
        var validationResult = _validation.TestValidate(insertPatientCommand);
        var result = await _handler.Handle(insertPatientCommand, default);

        // Assert
        validationResult.IsValid.Should().BeFalse(); // making sure that inputs are not valid
    }
}