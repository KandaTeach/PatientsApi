using Application.Patients.Query.GetPaged;

namespace Application.UnitTests.Patients.TestUtils;

public static class GetPagedQueryTestUtils
{
    public static GetPagedQuery CreateQuery(int page = 1, int pageSize = 10, string filter = null!, string sortBy = null!) =>
        new GetPagedQuery(page, pageSize, filter, sortBy);
}