using Application.Patients.Command.InsertPatient;
using Application.UnitTests.TestUtils.Constants;

namespace Application.UnitTests.Patients.TestUtils;

public static class InsertPatientCommandTestUtils
{
    public static InsertPatientCommand CreateCorrectCommand() =>
        new InsertPatientCommand(
            Constants.Patient.FirstName,
            Constants.Patient.LastName,
            Constants.Patient.City,
            Constants.Patient.ActiveYes.ToLower()
        );

    public static InsertPatientCommand CreateIncorrectCommand() =>
        new InsertPatientCommand(
            "Firstname of Patientttttttttttttttttttttttttttttttt",
            "Lastname of Patienttttttttttttttttttttttttttttttttt",
            "City of Patientttttttttttttttttttttttttttttttttttttt",
            "Not Sure"
        );
}