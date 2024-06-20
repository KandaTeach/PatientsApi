using Application.Patients.Command.UpdatePatient;
using Application.UnitTests.TestUtils.Constants;

namespace Application.UnitTests.Patients.TestUtils;

public static class UpdatePatientCommandTestUtils
{
    public static UpdatePatientCommand CreateCorrectCommand(string id) =>
        new UpdatePatientCommand(
            id,
            "Firstname of Patient",
            "Lastname of Patient",
            "City of Patient",
            Constants.Patient.ActiveNo.ToLower()
        );

    public static UpdatePatientCommand CreateIncorrectCommand(string id) =>
        new UpdatePatientCommand(
            id,
            "Firstname of Patientttttttttttttttttttttttttttttttt",
            "Lastname of Patienttttttttttttttttttttttttttttttttt",
            "City of Patientttttttttttttttttttttttttttttttttttttt",
            "Not Sure"
        );
}