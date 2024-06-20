using Application.Patients.Query.GetPatientById;

namespace Application.UnitTests.Patients.TestUtils;

public static class GetPatientByIdQueryTestUtils
{
    public static GetPatientByIdQuery CreateQuery(string id) =>
        new GetPatientByIdQuery(id);
}