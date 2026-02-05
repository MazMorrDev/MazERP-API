using MazErpBack.DTOs.Company;

namespace MazErpBack.Services.Interfaces;

public interface IUserCompanyService
{
    // This method has to ensure that if an user is already at the wf then update the values
    public Task<UserCompanyDto> AssignUserWithRoleToCompanyAsync(UserCompanyDto userCompanyDto);
    public Task<bool> SoftDeleteUserCompanyAsync(DeleteUserCompanyDto userCompanyDto);

    // For very old registries that we don't want
    public Task<bool> DeleteUserCompanyAsync(DeleteUserCompanyDto userCompanyDto);
}
