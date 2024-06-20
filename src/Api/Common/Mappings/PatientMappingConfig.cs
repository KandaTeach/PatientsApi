using Application.Patients.Common;
using Contracts.Response;
using Mapster;

namespace Api.Common.Mappings;

/// <summary>
/// Configures mappings between application results and contact response DTOs using Mapster.
/// </summary>
public class PatientMappingConfig : IRegister
{
    /// <summary>
    /// Registers the mapping configurations.
    /// </summary>
    /// <param name="config">The <see cref="TypeAdapterConfig"/> to configure.</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PatientResult, PatientInfoResponse>()
            .Map(des => des, src => src.Patient);

        config.NewConfig<EnumerablePatientResult, EnumerablePatientResponse>()
            .Map(des => des, src => src.Patients);

        config.NewConfig<EnumerablePatientPageResult, EnumerablePatientPageResponse>()
            .Map(des => des.Patients, src => src.Patients)
            .Map(des => des.TotalItems, src => src.TotalItems)
            .Map(des => des.TotalPages, src => src.TotalPages);
    }
}