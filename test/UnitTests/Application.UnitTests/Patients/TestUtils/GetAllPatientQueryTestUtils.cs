using Application.Patients.Query.GetAllPatient;

namespace Application.UnitTests.Patients.TestUtils;

public static class GetAllPatientQueryTestUtils
{
    public static GetAllPatientQuery CreateQuery() =>
        new GetAllPatientQuery();
}