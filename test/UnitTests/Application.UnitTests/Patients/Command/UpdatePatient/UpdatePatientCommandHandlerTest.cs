using Application.Common.Interfaces.Persistence.Repository;
using Application.Patients.Command.InsertPatient;
using Application.Patients.Command.UpdatePatient;
using Application.UnitTests.Patients.TestUtils;
using Domain.Models;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;

namespace Application.UnitTests.Patients.Command.UpdatePatient;

public class UpdatePatientCommandHandlerTest
{
    private readonly UpdatePatientCommandValidation _validation;
    private readonly UpdatePatientCommandHandler _updateHandler;
    private readonly InsertPatientCommandHandler _insertHandler;
    private readonly Mock<IPatientRepository> _mockRepository;

    public UpdatePatientCommandHandlerTest()
    {
        _validation = new UpdatePatientCommandValidation();
        _mockRepository = new Mock<IPatientRepository>();
        _updateHandler = new UpdatePatientCommandHandler(_mockRepository.Object);
        _insertHandler = new InsertPatientCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task HandleUpdatePatientCommand_WhenPatientDoesNotExist_ShouldReturnError()
    {
        // Arrange
        // Setup mock repository
        _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()));

        var command = UpdatePatientCommandTestUtils.CreateCorrectCommand(Guid.NewGuid().ToString()); // generate a random GUID.

        /* Then update the patient */
        // Act
        var validationResult = _validation.TestValidate(command);
        var result = await _updateHandler.Handle(command, default);

        // Assert
        validationResult.IsValid.Should().BeTrue(); // making sure that update details are valid
        result.IsError.Should().BeTrue(); // making sure that it is an error.
        _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Patient>(), It.IsAny<Patient>()), Times.Never); // Verify if repository was not called.
    }

    [Fact]
    public async Task HandleUpdatePatientCommand_WhenPatientExistedAndPatientDetailsAreCorrect_ShouldUpdateAndReturnPatient()
    {
        // Arrange
        /* 
            Since it is an in-memory datasource, so we need to insert a patient first to satisfy this test case 
            We need to setup the mock repository to the inserted patient so that it can be retrieved 
        */
        var insertPatientCommand = InsertPatientCommandTestUtils.CreateCorrectCommand();
        var insertResult = await _insertHandler.Handle(insertPatientCommand, default);

        // Setup mock repository to return the inserted patient
        _mockRepository.Setup(repo => repo.GetByIdAsync(insertResult.Value.Patient.Id))
            .ReturnsAsync(insertResult.Value.Patient);

        var command = UpdatePatientCommandTestUtils.CreateCorrectCommand(insertResult.Value.Patient.Id.ToString());

        /* Then update the patient */
        // Act
        var validationResult = _validation.TestValidate(command);
        var result = await _updateHandler.Handle(command, default);

        // Assert
        validationResult.IsValid.Should().BeTrue(); // making sure that update details are valid
        result.IsError.Should().BeFalse(); // making sure that updating a patient has no errors.
        _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Patient>(), It.IsAny<Patient>()), Times.Once); // Verify if repository was called once and update the patient.
    }

    [Fact]
    public async Task HandleUpdatePatientCommand_WhenPatientDetailsAreUncorrect_ShouldReturnAnError()
    {
        // Arrange
        // Setup mock repository to return the inserted patient
        _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()));

        var command = UpdatePatientCommandTestUtils.CreateIncorrectCommand(Guid.NewGuid().ToString());

        /* Then update the patient */
        // Act
        var validationResult = _validation.TestValidate(command);
        var result = await _updateHandler.Handle(command, default);

        // Assert
        validationResult.IsValid.Should().BeFalse(); // making sure that update details are not valid
        result.IsError.Should().BeTrue(); // making sure that there is an error.
        _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Patient>(), It.IsAny<Patient>()), Times.Never); // Verify if repository was not called.
    }
}