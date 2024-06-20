using Application.Common.Interfaces.Persistence.Repository;
using Application.Patients.Command.DeletePatient;
using Application.Patients.Command.InsertPatient;
using Application.UnitTests.Patients.TestUtils;
using Domain.Models;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;

namespace Application.UnitTests.Patients.Command.DeletePatient;

public class DeletePatientCommandTest
{
    private readonly DeletePatientCommandValidation _validation;
    private readonly DeletePatientCommandHandler _deleteHandler;
    private readonly InsertPatientCommandHandler _insertHandler;
    private readonly Mock<IPatientRepository> _mockRepository;

    public DeletePatientCommandTest()
    {
        _validation = new DeletePatientCommandValidation();
        _mockRepository = new Mock<IPatientRepository>();
        _deleteHandler = new DeletePatientCommandHandler(_mockRepository.Object);
        _insertHandler = new InsertPatientCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task HandleDeletePatientCommand_WhenPatientDoesNotExist_ShouldReturnAnError()
    {
        // Arrange
        // Setup mock repository
        _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()));

        var deletePatientCommand = DeletePatientCommandTestUtils.CreateCommand(Guid.NewGuid().ToString()); // generate a random id

        // Act
        var validationResult = _validation.TestValidate(deletePatientCommand);
        var deleteResult = await _deleteHandler.Handle(deletePatientCommand, default);

        // Assert
        validationResult.IsValid.Should().BeTrue(); // making sure that delete details are valid
        deleteResult.IsError.Should().BeTrue(); // making sure it is an error.
        _mockRepository.Verify(m => m.DeleteAsync(It.IsAny<Patient>()), Times.Never); // Verify if repository was not.
    }

    [Fact]
    public async Task HandleDeletePatientCommand_WhenPatientExisted_ShouldRemovePatient()
    {
        // Arrange
        /* 
            Since it is an in-memory datasource, so we need to insert a patient first to satisfy this test case 
            We need to setup the mock repository to the inserted patient so that it can be retrieved 
        */
        var insertPatientCommand = InsertPatientCommandTestUtils.CreateCorrectCommand();
        var insertResult = await _insertHandler.Handle(insertPatientCommand, default);

        // Setup mock repository
        _mockRepository.Setup(repo => repo.GetByIdAsync(insertResult.Value.Patient.Id))
            .ReturnsAsync(insertResult.Value.Patient);

        var deletePatientCommand = DeletePatientCommandTestUtils.CreateCommand(insertResult.Value.Patient.Id.ToString());

        /* Then delete patient */
        // Act
        var validationResult = _validation.TestValidate(deletePatientCommand);
        var deleteResult = await _deleteHandler.Handle(deletePatientCommand, default);

        // Assert
        validationResult.IsValid.Should().BeTrue(); // making sure that delete details are valid
        deleteResult.IsError.Should().BeFalse(); // making sure that deleting a patient has no errors.
        _mockRepository.Verify(m => m.DeleteAsync(It.IsAny<Patient>()), Times.Once); // Verify if repository was called once and delete the patient.
    }

    [Fact]
    public async Task HandleDeletePatientCommand_WhenIdIsNotAValidGuid_ShouldReturnAnError()
    {
        // Setup mock repository
        _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()));

        var deletePatientCommand = DeletePatientCommandTestUtils.CreateCommand("01293812ljawdaw123"); // Not a valid Guid

        /* Then delete patient */
        // Act
        var validationResult = _validation.TestValidate(deletePatientCommand);
        var deleteResult = await _deleteHandler.Handle(deletePatientCommand, default);

        // Assert
        validationResult.IsValid.Should().BeFalse(); // making sure that delete details not valid
        deleteResult.IsError.Should().BeTrue(); // making sure it is an error.
        _mockRepository.Verify(m => m.DeleteAsync(It.IsAny<Patient>()), Times.Never); // Verify if repository was not.
    }
}