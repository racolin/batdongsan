using Application.Common.Requests;
using Application.Common.Responses;
using Application.Common.Responses.Admin;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<List<string>?> GetUserRolesAsync(int userId);
    Task<bool?> IsInRolesAsync(int userId, string roleName);
    Task<bool?> AuthorizeAsync(int userId, string policyName);
    Task<DataResponse<LoginResponse>> LoginAsync(string username, string password);
    Task<DataResponse<bool>> LogoutAsync(int userId);
    Task<DataResponse<bool>> GeneratePasswordResetTokenAsync(string username, string operation, string browser);
    Task<DataResponse<bool>> ResetPasswordWithTokenAsync(string username, string token, string password);
    Task<DataResponse<PagingResponse<UserResponse>>> GetPagingUserModelsAsync(SearchRequest request, CancellationToken cancellationToken);
    Task<DataResponse<UserResponse>> GetUserAsync(int userId);
    Task<DataResponse<UserResponse>> GetProfileAsync(int userId);
    Task<DataResponse<bool>> AdminUpdatePasswordAsync(int userId, string password);
    Task<DataResponse<bool>> UpdatePasswordAsync(string username, string password, string newPassword);
    Task<DataResponse<int>> SaveUserAsync(int? userId, string username, string email, string name, string phone, int gender, DateTime? dateOfBirth);
    Task<DataResponse<bool>> UpdateProfileAsync(int userId, string name, int gender, DateTime? dateOfBirth);
    Task<DataResponse<bool>> AddRoleAsync(int userId, string roleName);
    Task<DataResponse<bool>> RemoveRoleAsync(int userId, string roleName);
    Task<DataResponse<bool>> SetLockUser(int userId, bool isLock);
    //Task<bool?> AddRolesAsync(int userId, List<string> roles);
    //Task<bool?> RemoveRolesAsync(int userId, List<string> roles);
    //Task<int> CountUsersInRoleActive(string roleName);
    //Task LogOut(string username);
    //Task<bool> CheckExistUser(string username);
    //Task<DataResponse<TokenModel>> GenerateJwtToken(string username);
    //Task<DataResponse<TokenModel>> GenerateJwtToken(string refreshToken, string accessToken);
    //Task<List<int>> GetUserIdsByRole(string? roleName = null);
    //Task<List<string>> GetUsernamesByIds(List<int> userIds);
}
