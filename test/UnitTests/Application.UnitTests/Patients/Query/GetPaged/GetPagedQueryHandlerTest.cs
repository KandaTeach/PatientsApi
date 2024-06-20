using Application.Common.Interfaces.Persistence.Repository;
using Application.Patients.Query.GetPaged;
using Application.UnitTests.Patients.TestUtils;
using Application.UnitTests.TestUtils.Constants;
using Domain.Models;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Patients.Query.GetPaged;

public class GetPagedQueryHandlerTest
{
    private readonly Mock<IPatientRepository> _mockRepository;
    private readonly GetPagedQueryHandler _handler;

    public GetPagedQueryHandlerTest()
    {
        _mockRepository = new Mock<IPatientRepository>();
        _handler = new GetPagedQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task HandleGetPageQuery_WhenDataSourceIsEmpty_ShouldReturnAnError()
    {
        // Arrange
        var query = GetPagedQueryTestUtils.CreateQuery();

        /* Setup mock repository */
        _mockRepository.Setup(repo => repo.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()));

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsError.Should().BeTrue(); // making sure that it is an error.
        _mockRepository.Verify(repo =>
            repo.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once
        ); // Verify if repository was called once and get the paged patient.
    }

    [Fact]
    public async Task HandleGetPageQuery_WhenAllParametersAreNotSetted_ShouldReturnPagedPatientsInAllDefaults()
    {
        // Arrange
        var query = GetPagedQueryTestUtils.CreateQuery(); // Not setted;

        /* Setup mock repository */
        _mockRepository.Setup(repo => repo.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(AllPatientsPaged());

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsError.Should().BeFalse(); // making sure that there is no errors.
        _mockRepository.Verify(repo =>
            repo.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once
        ); // Verify if repository was called once and get the paged patient.
    }

    [Fact]
    public async Task HandleGetPageQuery_WhenParametersPageAndPageSizeIsSetted_ShouldReturnPagedPatientsBasedOnSettedParametersWithDefaultSorting()
    {
        // Arrange
        var query = GetPagedQueryTestUtils.CreateQuery(
            page: 1,
            pageSize: 5
        ); // page and pageSize are setted

        /* Setup mock repository */
        _mockRepository.Setup(repo => repo.GetPagedAsync(query.Page, query.PageSize, It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(AllPatientsPaged(query.Page, query.PageSize));

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsError.Should().BeFalse(); // making sure that there is no errors.
        _mockRepository.Verify(repo =>
            repo.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once
        ); // Verify if repository was called once and get the paged patient.
    }

    [Fact]
    public async Task HandleGetPageQuery_WhenParameterPageIsOverlySetted_ShouldReturnAnError()
    {
        // Arrange
        var query = GetPagedQueryTestUtils.CreateQuery(
            page: 5, // page is 4 but it was setted 5 which is over.
            pageSize: 5
        ); // page and pageSize are setted

        /* Setup mock repository */
        _mockRepository.Setup(repo => repo.GetPagedAsync(query.Page, query.PageSize, It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(AllPatientsPaged(query.Page, query.PageSize));

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsError.Should().BeTrue(); // making sure that it is an error.
        _mockRepository.Verify(repo =>
            repo.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once
        ); // Verify if repository was called once and get the paged patient.
    }

    [Fact]
    public async Task HandleGetPageQuery_WhenParametersPagePageSizeAndFilterIsSetted_ShouldReturnPagedPatientsBasedOnSettedParametersWithDefaultSorting()
    {
        // Arrange
        var query = GetPagedQueryTestUtils.CreateQuery(
            page: 1,
            pageSize: 5,
            filter: "Patients"
        ); // page, pageSize, and filter are setted

        /* Setup mock repository */
        _mockRepository.Setup(repo => repo.GetPagedAsync(query.Page, query.PageSize, query.Filter, It.IsAny<string>()))
            .ReturnsAsync(AllPatientsPaged(query.Page, query.PageSize, query.Filter));

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsError.Should().BeFalse(); // making sure that there is no errors.
        _mockRepository.Verify(repo =>
            repo.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once
        ); // Verify if repository was called once and get the paged patient.
    }

    [Fact]
    public async Task HandleGetPageQuery_WhenParameterFilterIsWronglySetted_ShouldReturnAnError()
    {
        // Arrange
        var query = GetPagedQueryTestUtils.CreateQuery(
            page: 1,
            pageSize: 5,
            filter: "John" // All data values are all started as patients. John does not exist.
        ); // page, pageSize, and filter are setted

        /* Setup mock repository */
        _mockRepository.Setup(repo => repo.GetPagedAsync(query.Page, query.PageSize, query.Filter, It.IsAny<string>()))
            .ReturnsAsync(AllPatientsPaged(query.Page, query.PageSize, query.Filter));

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsError.Should().BeTrue(); // making sure it is an error.
        _mockRepository.Verify(repo =>
            repo.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once
        ); // Verify if repository was called once and get the paged patient.
    }

    [Fact]
    public async Task HandleGetPageQuery_WhenAllParametersAreSetted_ShouldReturnPagedPatientsBasedOnSettedParameters()
    {
        // Arrange
        var query = GetPagedQueryTestUtils.CreateQuery(
            page: 1,
            pageSize: 5,
            filter: "Patients",
            sortBy: "Lastname"
        ); // all parameters are setted

        /* Setup mock repository */
        _mockRepository.Setup(repo => repo.GetPagedAsync(query.Page, query.PageSize, query.Filter, query.SortBy))
            .ReturnsAsync(AllPatientsPaged(query.Page, query.PageSize, query.Filter, query.SortBy));

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsError.Should().BeFalse(); // making sure that there is no errors.
        _mockRepository.Verify(repo =>
            repo.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once
        ); // Verify if repository was called once and get the paged patient.
    }

    [Fact]
    public async Task HandleGetPageQuery_WhenParameterSortByIsWronglySetted_ShouldReturnPagedPatientsOnDefaultSorting()
    {
        // Arrange
        var query = GetPagedQueryTestUtils.CreateQuery(
            page: 1,
            pageSize: 5,
            filter: "Patients",
            sortBy: "Gender" // Only firstname, lastname, city, and active are needed to be sorted. Gender is not included.
        ); // all parameters are setted

        /* Setup mock repository */
        _mockRepository.Setup(repo => repo.GetPagedAsync(query.Page, query.PageSize, query.Filter, query.SortBy))
            .ReturnsAsync(AllPatientsPaged(query.Page, query.PageSize, query.Filter, query.SortBy));

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsError.Should().BeFalse(); // making sure that there is no errors.
        _mockRepository.Verify(repo =>
            repo.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once
        ); // Verify if repository was called once and get the paged patient.
    }

    private static (IEnumerable<Patient>, int, int) AllPatientsPaged(
        int page = 1,
        int pageSize = 10,
        string filter = null!,
        string sortBy = null!
    )
    {
        var patients = AllPatients().AsEnumerable();

        var query = patients.AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(filter))
        {
            query = query.Where(p =>
                p.FirstName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                p.LastName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                p.City.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                p.Active.ToString().Equals(filter, StringComparison.OrdinalIgnoreCase));
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "firstname" => query.OrderBy(p => p.FirstName),
                "lastname" => query.OrderBy(p => p.LastName),
                "active" => query.OrderBy(p => p.Active),
                "city" => query.OrderBy(p => p.City),
                _ => query.OrderBy(p => p.FirstName),
            };
        }
        else
        {
            // Default sorting (optional)
            query = query.OrderBy(p => p.FirstName);
        }

        // Paging
        int totalItems = query.Count();
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        var result = query.Skip((page - 1) * pageSize).Take(pageSize);

        return new(result.AsEnumerable(), totalItems, totalPages);
    }

    private static List<Patient> AllPatients()
    {
        // has 20 patients
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
                Constants.Patient.ActiveNo
            ),
            Patient.Create(
                Constants.Patient.FirstName,
                Constants.Patient.LastName,
                Constants.Patient.City,
                Constants.Patient.ActiveNo
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
                Constants.Patient.ActiveNo
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
                Constants.Patient.ActiveNo
            ),
            Patient.Create(
                Constants.Patient.FirstName,
                Constants.Patient.LastName,
                Constants.Patient.City,
                Constants.Patient.ActiveNo
            ),
            Patient.Create(
                Constants.Patient.FirstName,
                Constants.Patient.LastName,
                Constants.Patient.City,
                Constants.Patient.ActiveNo
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
                Constants.Patient.ActiveNo
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
                Constants.Patient.ActiveNo
            )
        };
    }
}