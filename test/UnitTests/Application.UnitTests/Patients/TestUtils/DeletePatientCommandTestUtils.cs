using Application.Patients.Command.DeletePatient;

namespace Application.UnitTests.Patients.TestUtils;

public static class DeletePatientCommandTestUtils
{
    public static DeletePatientCommand CreateCommand(string id) =>
        new DeletePatientCommand(id);
}