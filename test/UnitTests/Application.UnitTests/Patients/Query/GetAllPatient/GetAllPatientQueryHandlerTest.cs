using Application.Common.Interfaces.Persistence.Repository;
using Application.Patients.Query.GetAllPatient;
using Application.UnitTests.Patients.TestUtils;
using Application.UnitTests.TestUtils.Constants;
using Domain.Models;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Patients.Query.GetAllPatient;

public class GetAllPatientQueryHandlerTest
{
    private readonly Mock<IPatientRepository> _mockRepository;
    private readonly GetAllPatientQueryHandler _handler;

    public GetAllPatientQueryHandlerTest()
    {
        _mockRepository = new Mock<IPatientRepository>();
        _handler = new GetAllPatientQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task HandleGetAllPatientQuery_WhenDataSourceIsEmpty_ShouldReturnAnError()
    {
        // Arrange
        /* Setup mock repository */
        _mockRepository.Setup(repo => repo.GetAllAsync());

        var query = GetAllPatientQueryTestUtils.CreateQuery();

        /* Then return all patient */
        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsError.Should().BeTrue(); // making sure that there is an error.
        _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once); // Verify if repository was called once and return all patient.

    }

    [Fact]
    public async Task HandleGetAllPatientQuery_WhenDataSourceIsNotEmpty_ShouldReturnAllPatients()
    {
        // Arrange
        /* Setup mock repository */
        _mockRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(AllPatients().AsEnumerable());

        var query = GetAllPatientQueryTestUtils.CreateQuery();

        /* Then return all patient */
        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsError.Should().BeFalse(); // making sure that there is no errors.
        _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once); // Verify if repository was called once and return all patient.

    }

    private static List<Patient> AllPatients()
    {
        // has 5 patients
        return new List<Patient>
        {
            Patient.Create(
                Constants.Patient.FirstName,
                Constants.Patient.LastName,
                Constants.Patient.City,
                Constants.Patient.ActiveYes
            ),
            Patient.Create(
                Constants.Patient.FirstName,
                Constants.Patient.LastName,
                Constants.Patient.City,
                Constants.Patient.ActiveYes
            ),
            Patient.Create(
                Constants.Patient.FirstName,
                Constants.Patient.LastName,
                Constants.Patient.City,
                Constants.Patient.ActiveYes
            ),
            Patient.Create(
                Constants.Patient.FirstName,
                Constants.Patient.LastName,
                Constants.Patient.City,
                Constants.Patient.ActiveYes
            ),
            Patient.Create(
                Constants.Patient.FirstName,
                Constants.Patient.LastName,
                Constants.Patient.City,
                Constants.Patient.ActiveYes
            ),
        };
    }
}