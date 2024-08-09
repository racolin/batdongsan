using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Domain.Constants;
using System.Security.Claims;
using Domain;
using System.Text;
using AutoMapper;
using Infrastructure.Persistence;
using Application.Common.Responses;
using Infrastructure.Services;
using System.Text.RegularExpressions;
using Azure;
using Application.Common.Requests;
using System.Linq.Expressions;
using System.Threading;
using Application.Common.Responses.Admin;

namespace Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly IFileService _fileService;
    private readonly JwtConfigOptions _jwtOptions;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IdentityService> _logger;
    private readonly UserManager<User> _userManager;
    private readonly IAuthorizationService _authorizationService;
    private readonly IMailService _mailService;
    private readonly SignInManager<User> _signInManager;
    private readonly IUserClaimsPrincipalFactory<User> _userClaimsPrincipalFactory;

    public IdentityService
        (
            IFileService fileService,
            ApplicationDbContext context,
            ILogger<IdentityService> logger,
            IOptions<JwtConfigOptions> jwtOptions,
            UserManager<User> userManager,
            IAuthorizationService authorizationService,
            SignInManager<User> signInManager,
            IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory,
            IMailService mailService 
        )
    {
        _mailService = mailService;
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _fileService = fileService;
        _logger = logger;
        _authorizationService = authorizationService;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<List<string>?> GetUserRolesAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if (user == null)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);

        return roles == null ? new List<string>() : roles.ToList();
    }
    public async Task<bool?> IsInRolesAsync(int userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return null;
        }

        return await _userManager.IsInRoleAsync(user, roleName);
    }
    public async Task<bool?> AuthorizeAsync(int userId, string policyName)
    {
        var user =  await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return null;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded; 
    }
    public async Task<DataResponse<string>> LoginAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
            return DataResponse<string>.Error("Thông tin đăng nhập không hợp lệ.");

        var isLockout = await _userManager.IsLockedOutAsync(user);
        if (isLockout)
            return DataResponse<string>.Error("Tài khoản của bạn đã bị khoá. Vui lòng liên hệ tư vấn viên để được hỗ trợ.");

        var result = await _signInManager.PasswordSignInAsync(user, password, false, true);
        if (!result.Succeeded)
            return DataResponse<string>.Error($"Thông tin đăng nhập không hợp lệ. Bạn còn {RoleConstant.MaxLoginFail - user.AccessFailedCount} lần thử!");

        return DataResponse<string>.Success(user.Name);
    }
    public async Task<DataResponse<bool>> LogoutAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return DataResponse<bool>.Error("Không tìm thấy người dùng này!");
        }

        await _signInManager.SignOutAsync();

        return DataResponse<bool>.Success(true);
    }
    public async Task<DataResponse<bool>> GeneratePasswordResetTokenAsync(string username, string operation, string browser)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return DataResponse<bool>.Error("Không tìm thấy người dùng này!");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        if (token == null)
        {
            return DataResponse<bool>.Error("Không thực hiện yêu cầu cấp lại mật khẩu!");
        }

        var message = await _mailService.SendAdminResetPassword(user.Email, user.UserName, token, user.Name, operation, browser);

        if (message != null)
        {
            return DataResponse<bool>.Error("Quá trình gửi email gặp sự cố, hãy thử lại!", [message]);
        }

        return DataResponse<bool>.Success(true);
    }
    public async Task<DataResponse<bool>> ResetPasswordWithTokenAsync(string username, string token, string password)
    {
        if (!Regex.IsMatch(password, RegexConstant.Password))
        {
            return DataResponse<bool>.Error("Mật khẩu không đúng định dạng!");
        }

        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return DataResponse<bool>.Error("Không tìm thấy người dùng này!");
        }

        var result = await _userManager.ResetPasswordAsync(user, token, password);
        if (result == null)
        {
            return DataResponse<bool>.Error("Không thể thực hiện yêu cầu đặt lại mật khẩu!");
        }

        if (!result!.Succeeded)
        {
            return DataResponse<bool>.Error("Không thể thực hiện yêu cầu đặt lại mật khẩu!", []);
        }

        return DataResponse<bool>.Success(true);
    }
    public async Task<DataResponse<PagingResponse<UserResponse>>> GetPagingUserModelsAsync(SearchRequest request, CancellationToken cancellationToken)
    {

        bool? isLock = null;
        if (request.IsLock.Count == 0 || (request.IsLock.Contains(true) && request.IsLock.Contains(false))) 
        {
            isLock = null;
        } else
        {
            isLock = request.IsLock.Contains(true);
        }

        var userName = request.Value;
        var email = request.ValueFilter1;

        // Lấy số lượng tin được lọc theo yêu cầu
        var count = await _userManager.Users.AsNoTracking()
            .Where(x =>
                (x.UserName == null ? false : x.UserName.Contains(userName ?? ""))
                && (x.Email == null ? false : x.Email.Contains(email ?? ""))
                && (isLock == null ? true : isLock == (x.LockoutEnd != null))
            ).CountAsync(cancellationToken);

        // Lấy danh sách theo phân trang
        if (count < request.Start)
        {
            request.CurrentPage = 0;
        }
        var users = await _userManager.Users
            .Include(x => x.UserRoles).ThenInclude(x => x.Role)
            .Where(x =>
                (x.UserName == null ? false : x.UserName.Contains(userName ?? ""))
                && (x.Email == null ? false : x.Email.Contains(email ?? ""))
                && (isLock == null ? true : isLock == (x.LockoutEnd != null))
            )
            .Skip(request.Start)
            .Take(request.Length)
            .Select(x => new UserResponse
            {
                Id = x.Id,
                Name = x.Name,
                Username = x.UserName ?? "",
                DateOfBirth = x.DateOfBirth,
                Email = x.Email ?? "",
                LockoutEnd = x.LockoutEnd == null ? null : x.LockoutEnd.Value.LocalDateTime,
                Gender = x.Gender,
            })
            .ToListAsync(cancellationToken);

        var max = count / request.Length + (count % request.Length == 0 ? 0 : 1);
        var current = (request.Start + 1) / request.Length + ((request.Start + 1) % request.Length == 0 ? 0 : 1);

        var paging = new PagingResponse<UserResponse>
        {
            List = users,
            PerPage = request.Length,
            Max = max,
            Current = current,
        };

        return DataResponse<PagingResponse<UserResponse>>.Success(paging);
    }
    public async Task<DataResponse<UserResponse>> GetUserAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return DataResponse<UserResponse>.Error("Không tìm thấy người dùng này!");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var u = new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Username = user.UserName,
            Email = user.Email,
            Phone = user.PhoneNumber,
            Gender = user.Gender,
            DateOfBirth = user.DateOfBirth,
            LockoutEnd = user.LockoutEnd == null ? null : user.LockoutEnd.Value.LocalDateTime,
            Roles = roles.ToList(),
        };
        return DataResponse<UserResponse>.Success(u);
    }
    public async Task<DataResponse<bool>> AdminUpdatePasswordAsync(int userId, string password)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return DataResponse<bool>.Error("Không tìm thấy người dùng này!");
        }

        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, password);

        if (!result!.Succeeded)
        {
            return DataResponse<bool>.Error("Không thể thực hiện yêu cầu đặt lại mật khẩu!", []);
        }

        return DataResponse<bool>.Success(true);
    }
    public async Task<DataResponse<bool>> UpdatePasswordAsync(string username, string password, string newPassword)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return DataResponse<bool>.Error("Không tìm thấy người dùng này!");
        }
        var checkPassword = await _userManager.CheckPasswordAsync(user, password);
        if (!checkPassword)
        {
            return DataResponse<bool>.Error("Mật khẩu cũ không chính xác!", []);
        }
        var result = await _userManager.ChangePasswordAsync(user, password, newPassword);

        if (!result!.Succeeded)
        {
            return DataResponse<bool>.Error("Không thể thực hiện yêu cầu đặt lại mật khẩu!", []);
        }

        return DataResponse<bool>.Success(true);
    }
    public async Task<DataResponse<int>> SaveUserAsync(int? userId, string username, string email, string name, string phone, int gender, DateTime? dateOfBirth)
    {

        if (userId == null)
        {
            User? user = new User
            {
                UserName = username,
                Email = email,
                Name = name,
                PhoneNumber = phone,
                Gender = gender,
                DateOfBirth = dateOfBirth,
            };
            var result = await _userManager.CreateAsync(user, DefaultConstant.DefaultPassword);

            if (!result.Succeeded)
            {
                return DataResponse<int>.Error("Có lỗi xảy ra khi tạo mới người dùng này!", []);
            }
            return DataResponse<int>.Success(user.Id);
        } 
        else 
        {
            var user = await _userManager.FindByIdAsync(userId.Value.ToString());
            if (user == null) {
                return DataResponse<int>.Error("Không tìm thấy người dùng này!");
            }
            user.UserName = username;
            user.Email = email;
            user.Name = name;
            user.PhoneNumber = phone;
            user.Gender = gender;
            user.DateOfBirth = dateOfBirth;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return DataResponse<int>.Error("Có lỗi xảy ra khi cập nhật người dùng này!");
            }
            return DataResponse<int>.Success(user.Id);
        }
    }
    public async Task<DataResponse<bool>> AddRoleAsync(int userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return DataResponse<bool>.Error("Không tìm thấy người dùng này!");
        }

        var result = await _userManager.AddToRoleAsync(user!, roleName);

        if (!result.Succeeded)
        {
            return DataResponse<bool>.Error("Không thể thêm vai trò này!");
        }

        return DataResponse<bool>.Success(true);
    }
    public async Task<DataResponse<bool>> RemoveRoleAsync(int userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return DataResponse<bool>.Error("Không tìm thấy người dùng này!");
        }

        var result = await _userManager.RemoveFromRoleAsync(user!, roleName);

        if (!result.Succeeded)
        {
            return DataResponse<bool>.Error("Không thể xóa vai trò này!");
        }

        return DataResponse<bool>.Success(true);
    }
    public async Task<DataResponse<bool>> SetLockUser(int userId, bool isLock)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return DataResponse<bool>.Error("Người dùng không tồn tại.");

        var isLocked = await _userManager.IsLockedOutAsync(user);
        if ((isLocked && isLock) || !(isLocked || isLock))
        {
            return DataResponse<bool>.Success(true);
        }

        if (isLock) //Lock
        {
            var lockedDate = DateTimeOffset.UtcNow.AddYears(1);
            await _userManager.SetLockoutEndDateAsync(user, lockedDate);
            await _userManager.UpdateSecurityStampAsync(user);
        }
        else // UnLock
        {
            await _userManager.SetLockoutEndDateAsync(user, null);
            user.AccessFailedCount = 0;
            await _context.SaveChangesAsync();
        }
        return DataResponse<bool>.Success(true);
    }

    //    public async Task<List<string?>> GetUsernames(List<int> userIds)
    //    {
    //        userIds = userIds.Distinct().ToList();
    //        return await _userManager.Users
    //            .Where(x => userIds.Contains(x.Id))
    //            .Select(x => x.UserName)
    //            .ToListAsync();
    //    }
    //    public async Task<List<User>> GetUserProfileBasic(List<int> userIds)
    //    {
    //        userIds = userIds.Distinct().ToList();
    //        return await _context.UserProfiles.Select(x => new User
    //        {
    //            FullName = x.FullName,
    //            PersonalPhoto= x.PersonalPhoto,
    //            PhoneNumber = x.PhoneNumber,
    //            Status= x.Status,
    //            UserId= x.UserId,
    //            UserUD = x.UserUD,
    //        }).Where(x => userIds.Contains(x.UserId??0) && x.Status == StatusConstant.Active).ToListAsync();
    //    }
    //    public async Task<List<User>> GetUsersByDate(DateTime startDate, DateTime endDate)
    //    {

    //        var users = await _context.UserProfiles.Select(x => new User
    //        {
    //            Id = x.Id,
    //            UserId = x.UserId,
    //            UserUD = x.UserUD,
    //            FullName = x.FullName,
    //            CreatedDate = x.CreatedDate,
    //            PhoneNumber = x.PhoneNumber,
    //            PersonalPhoto = x.PersonalPhoto
    //        }).Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate).ToListAsync();
    //        return users;

    //    }

    //    public async Task<User> UpdateProfile(int userId, SaveUserRequest model)
    //    {
    //        var userInfo = await _context.UserProfiles
    //            .Include(x => x.User)
    //            .FirstOrDefaultAsync(x => x.UserId == userId);
    //        if (userInfo == null)
    //        {
    //            userInfo = new UserProfile
    //            {
    //                UserId = userId,
    //                UserUD = model.UserUD,
    //                Status = StatusConstant.Active,
    //            };
    //            if (string.IsNullOrEmpty(userInfo.UserUD))
    //                userInfo.UserUD = GenerateUD.generationUD("ITR-", 5, new List<String>());
    //        }

    //        if (userInfo.User != null)
    //        {
    //            var loginInfos = await _userManager.GetLoginsAsync(userInfo.User);
    //            var isEmailLogin = loginInfos.Any(x => x.LoginProvider is "Google" or "Facebook");
    //            if (!isEmailLogin)
    //            {
    //                userInfo.User.Email = model.Email;
    //            }

    //            // Normal member
    //            if (!loginInfos.Any())
    //            {
    //                userInfo.PhoneNumber = model.PhoneNumber;
    //            }
    //            //Update username
    //            //fix lỗi lưu mất user name
    //            if (!string.IsNullOrEmpty(model.UserName))
    //                userInfo.User.UserName = model.UserName;
    //        }

    //        userInfo.PhoneNumber = model.PhoneNumber;
    //        userInfo.FullName = model.FullName;
    //        userInfo.BranchIds = model.BranchIDs;
    //        userInfo.DateOfBirth = model.DateOfBirth;
    //        userInfo.Gender = model.Gender;
    //        userInfo.Description = model.Description;
    //        userInfo.Certification = model.Certification;
    //        userInfo.CurriculumVitae = model.CurriculumVitae;
    //        userInfo.CountryCode = model.CountryCode;
    //        userInfo.ProvinceCode = model.ProvinceCode;
    //        userInfo.DistrictCode = model.DistrictCode;
    //        userInfo.WardCode = model.WardCode;
    //        userInfo.Address = model.Address;
    //        userInfo.ZipCode = model.ZipCode;

    //        if (userInfo.Id == 0)
    //        {
    //            _context.UserProfiles.Add(userInfo);
    //        }
    //        await _context.SaveChangesAsync();

    //        if (model.Files.Any())
    //        {
    //            var fileName = await _fileService.SaveFile(model.Files[0], userInfo.UserUD, "User");
    //            userInfo.PersonalPhoto = fileName;
    //        }

    //        if (model.CertificationFile != null)
    //        {
    //            var fileName = await _fileService.SaveFile(model.CertificationFile, userInfo.UserUD, "cf", "User");
    //            userInfo.Certification = fileName;
    //        }

    //        if (model.CurriculumVitaeFile != null)
    //        {
    //            var fileName = await _fileService.SaveFile(model.CurriculumVitaeFile, userInfo.UserUD, "cv", "User");
    //            userInfo.CurriculumVitae = fileName;
    //        }

    //        await _context.SaveChangesAsync();

    //        var mapper = _mapperConfiguration.CreateMapper();
    //        var result = mapper.Map<User>(userInfo);
    //        return result;
    //    }

    //    public async Task<DataResponse<object>> SaveOtp(string phone, string otp, bool isResend = false)
    //    {
    //        var user = await _context.Users.Include(x => x.UserProfile).FirstOrDefaultAsync(x => x.UserName == phone);
    //        if (user == null || user.UserProfile == null)
    //        {
    //            return DataResponse<object>.Error("Thông tin đăng nhập không hợp lệ.");
    //        }

    //        if (!isResend && user.UserProfile.OtpExpiration.CompareTo(DateTime.UtcNow) >= 0 && user.UserProfile.Otp != null)
    //        {
    //            //return DataResponse<SliderEntity>.Error("OTP cũ vẫn còn hiệu lực. Hãy kiểm tra điện thoại để nhập OTP.");
    //            return new DataResponse<object>();
    //        }
    //        user.UserProfile.Otp = otp;
    //        user.UserProfile.OtpFailedCount = 0;
    //        user.UserProfile.OtpExpiration = DateTime.UtcNow.AddMinutes(5);
    //        await _context.SaveChangesAsync();
    //        return new DataResponse<object>();
    //    }

    //    public async Task<DataResponse<object>> ResetPassword(int userId, string newPassword, bool isMobile = false)
    //    {
    //        var user = await _userManager.FindByIdAsync(userId.ToString());
    //        if (user == null)
    //            return DataResponse<object>.Error("User not found.");

    //        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    //        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);


    //        if (!result.Succeeded)
    //            return DataResponse<object>.Error("Đã có lỗi xảy ra. Vui lòng liên hệ kỹ thuật viên để dược hỗ trợ");


    //        if (isMobile)
    //        {
    //            var data = await GenerateJwtToken(user.UserName);
    //            return data;
    //        }

    //        await _signInManager.SignInWithClaimsAsync(user, false, new List<Claim>()
    //        {
    //            // SignalR use it
    //            new(ClaimTypes.NameIdentifier, user.UserName)
    //        });
    //        return new DataResponse<object>();
    //    }

    //    public async Task<DataResponse<object>> CheckOtp(string phone, string otp, bool isMobile)
    //    {
    //        var user = await _context.Users.Include(x => x.UserProfile).FirstOrDefaultAsync(x => x.UserName == phone);
    //        if (user == null || user.UserProfile == null)
    //        {
    //            return DataResponse<object>.Error("Thông tin đăng nhập không hợp lệ.");
    //        }

    //        var isLockout = await _userManager.IsLockedOutAsync(user);
    //        if (isLockout)
    //        {
    //            return DataResponse<object>.Error("Tài khoản của bạn đã bị khoá. Vui lòng liên hệ tư vấn viên để được hỗ trợ.");
    //        }

    //        var isInActive = user.UserProfile.Status == StatusConstant.InActive;
    //        if (isInActive)
    //        {
    //            return DataResponse<object>.Error("Tài khoản của bạn đã bị khoá. Vui lòng liên hệ tư vấn viên để được hỗ trợ.");
    //        }

    //        if (user.UserProfile.Otp == null)
    //        {
    //            return DataResponse<object>.Error("Không tìm thấy OTP");
    //        }

    //        if (user.UserProfile.OtpExpiration.CompareTo(DateTime.UtcNow) < 0)
    //        {
    //            return DataResponse<object>.Error("Đã quá thời gian phản hồi.");
    //        }

    //        if (user.UserProfile.OtpFailedCount > 4)
    //        {
    //            user.UserProfile.OtpFailedCount = 0;
    //            user.UserProfile.Otp = null;
    //            await _context.SaveChangesAsync();
    //            return DataResponse<object>.Error("Bạn đã nhập sai OTP quá số lần quy định.");
    //        }
    //        //Mặc định OTP 123456
    //        if (user.UserProfile.Otp != otp && otp != "123456")
    //        {
    //            user.UserProfile.OtpFailedCount++;
    //            if (user.UserProfile.OtpFailedCount > 4)
    //            {
    //                user.UserProfile.OtpFailedCount = 0;
    //                user.UserProfile.Otp = null;
    //                await _context.SaveChangesAsync();
    //                return DataResponse<object>.Error("Bạn đã nhập sai OTP quá số lần quy định.");
    //            }
    //            await _context.SaveChangesAsync();
    //            return DataResponse<object>.Error("OTP không chính xác " + user.UserProfile.OtpFailedCount + " lần, vui lòng thử lại.");
    //        }

    //        user.UserProfile.Otp = null;
    //        user.UserProfile.OtpFailedCount = 0;
    //        await _context.SaveChangesAsync();

    //        if (isMobile)
    //        {
    //            var data = await GenerateJwtToken(user.UserName, true);
    //            return data;
    //        }

    //        await _signInManager.SignInWithClaimsAsync(user, false, new List<Claim>()
    //        {
    //            // SignalR use it
    //            new(ClaimTypes.NameIdentifier, user.UserName),
    //            new("isNeedResetPassword", "true")
    //        });
    //        return new DataResponse<object>();
    //    }

    //    public async Task<User?> SaveUserImage(int userId, List<IFormFile> files, CancellationToken cancellationToken)
    //    {
    //        var userInfo = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
    //        if (userInfo == null)
    //            return null;

    //        if (files.Any())
    //        {
    //            var fileName = await _fileService.SaveFile(files[0], userInfo.UserUD, "User");
    //            userInfo.PersonalPhoto = fileName;
    //        }
    //        await _context.SaveChangesAsync(cancellationToken);
    //        var mapper = _mapperConfiguration.CreateMapper();
    //        var model = mapper.Map<User>(userInfo);
    //        return model;
    //    }

    //    public async Task<bool> AddRolesAsync(int userId, string roleOld, string roleNew)
    //    {

    //        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);
    //        await _userManager.RemoveFromRoleAsync(user, roleOld);
    //        await _userManager.AddToRoleAsync(user, roleNew);
    //        return user != null && await _userManager.IsInRoleAsync(user, roleNew);
    //    }

    //    public async Task<User> GetProfileById(int id)
    //    {
    //        var user = await _context.UserProfiles.AsNoTracking()
    //            .Include(x => x.User)
    //            .FirstOrDefaultAsync(x => x.UserId == id);
    //        if (user == null)
    //        {
    //            return null;
    //        }

    //        var mapper = _mapperConfiguration.CreateMapper();
    //        var model = mapper.Map<User>(user);
    //        return model;
    //    }

    //    public async Task<User> GetProfileByName(string userName)
    //    {
    //        var user = await _userManager.Users.AsNoTracking()
    //            .Include(x => x.UserProfile)
    //            .FirstOrDefaultAsync(x => x.UserName == userName);
    //        if (user == null)
    //        {
    //            return null;
    //        }

    //        var mapper = _mapperConfiguration.CreateMapper();
    //        var model = mapper.Map<User>(user.UserProfile);
    //        model.UserName = userName;
    //        if (user.UserProfile.CountryCode != null)
    //        {
    //            var country = await _context.Countries.Where(x => x.Code == user.UserProfile.CountryCode).FirstOrDefaultAsync();
    //            model.CountryName = country == null ? "" : country.Name;
    //        }
    //        if (user.UserProfile.ProvinceCode != null)
    //        {
    //            var province = await _context.Provinces.Where(x => x.Code == user.UserProfile.ProvinceCode).FirstOrDefaultAsync();
    //            model.ProvinceName = province == null ? "" : province.Name;
    //        }
    //        if (user.UserProfile.DistrictCode != null)
    //        {
    //            var district = await _context.Districts.Where(x => x.Code == user.UserProfile.DistrictCode).FirstOrDefaultAsync();
    //            model.DistrictName = district == null ? "" : district.Name;
    //        }
    //        if (user.UserProfile.WardCode != null)
    //        {
    //            var ward = await _context.Wards.Where(x => x.Code == user.UserProfile.WardCode).FirstOrDefaultAsync();
    //            model.WardName = ward == null ? "" : ward.Name;
    //        }
    //        return model;
    //    }

    //    public async Task<List<User>> GetUserProfiles(List<int> userIds, int start = 0, int length = 0, int branchId = 0, string keySearch = null)
    //    {
    //        if (branchId == 0)
    //        {
    //             if (length > 0)
    //                    return await _context.UserProfiles
    //                        .Where(x => userIds.Contains(x.UserId!.Value) && x.Status == StatusConstant.Active && (keySearch == null || EF.Functions.Like(x.FullName, $"%{keySearch}%")))
    //                        .Skip(start).Take(length)
    //                        .OrderByDescending(x => x.CreatedDate)
    //                        .ProjectToListAsync<User>(_mapperConfiguration);
    //                else
    //                {
    //                    return await _context.UserProfiles
    //                                .Where(x => userIds.Contains(x.UserId!.Value) && x.Status == StatusConstant.Active && (keySearch == null || EF.Functions.Like(x.FullName, $"%{keySearch}%")))
    //                                .OrderByDescending(x => x.CreatedDate)
    //                                .ProjectToListAsync<User>(_mapperConfiguration);
    //                }
    //        }
    //        else
    //        {
    //            if (length > 0)
    //            {
    //                var user = await _context.UserProfiles
    //                   .Where(x => userIds.Contains(x.UserId!.Value) && x.Status == StatusConstant.Active && (keySearch == null || EF.Functions.Like(x.FullName, $"%{keySearch}%")))
    //                   .Skip(start).Take(length)
    //                   .OrderByDescending(x => x.CreatedDate)
    //                   .ProjectToListAsync<User>(_mapperConfiguration);
    //                var rs = user.Where(x => !string.IsNullOrEmpty(x.BranchIds) && x.BranchIds.Split(",").ToList().Where(p => p == branchId.ToString()).Any());
    //                return rs.ToList();
    //            }

    //            else
    //            {
    //                var user = await _context.UserProfiles
    //                             .Where(x => userIds.Contains(x.UserId!.Value) && x.Status == StatusConstant.Active && (keySearch == null || EF.Functions.Like(x.FullName, $"%{keySearch}%")))
    //                             .OrderByDescending(x => x.CreatedDate)
    //                             .ProjectToListAsync<User>(_mapperConfiguration);
    //                var rs = user.Where(x => !string.IsNullOrEmpty(x.BranchIds) && x.BranchIds.Split(",").ToList().Where(p => p == branchId.ToString()).Any());
    //                return rs.ToList();
    //            }
    //        }

    //    }

    //    public async Task<int> TotalUserActive(string role)
    //    {
    //        var userInRoles = await _userManager.GetUsersInRoleAsync(role);
    //        if (userInRoles == null)
    //            return 0;
    //        List<int> userIds = userInRoles.Select(x => x.Id).ToList();

    //        return _context.UserProfiles
    //                    .Where(x => userIds.Contains(x.UserId!.Value) && x.Status == StatusConstant.Active).Count();
    //    }
    //    public async Task<List<User>> GetUserProfiles(string role, CancellationToken cancellationToken, int start = 0, int length = 0, string keySearch = null)
    //    {
    //        var userInRoles = await _userManager.GetUsersInRoleAsync(role);
    //        List<int> userIds;
    //        if (userInRoles != null)
    //            userIds = userInRoles.Select(x => x.Id).ToList();
    //        else
    //            userIds = new List<int>();
    //        return await GetUserProfiles(userIds, start, length, 0, keySearch);
    //    }
    //    public async Task<List<User>> GetUserProfiles(string role, CancellationToken cancellationToken, int start = 0, int length = 0, int branchId = 0, string keySearch = null)
    //    {
    //        var userInRoles = await _userManager.GetUsersInRoleAsync(role);
    //        List<int> userIds;
    //        if (userInRoles != null)
    //            userIds = userInRoles.Select(x => x.Id).ToList();
    //        else
    //            userIds = new List<int>();
    //        return await GetUserProfiles(userIds, start, length, branchId, keySearch);
    //    }
    //     public async Task<List<User>> GetIntructorByBranch(string role, CancellationToken cancellationToken, int branchId = 0)
    //    {
    //        var userInRoles = await _userManager.GetUsersInRoleAsync(role);
    //        List<int> userIds;
    //        if (userInRoles != null)
    //            userIds = userInRoles.Select(x => x.Id).ToList();
    //        else
    //            userIds = new List<int>();
    //        return await GetUserProfiles(userIds, 0, 0, branchId);
    //    }

    //    public async Task<List<string>> GetUserRoles(string userName)
    //    {
    //        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
    //        if (user == null)
    //            return new List<string>();

    //        var roles = await _userManager.GetRolesAsync(user);
    //        return roles.ToList();
    //    }

    //    public async Task<DataResponse<object>> TokenLogin(string username, string password)
    //    {
    //        var user = await _userManager.FindByNameAsync(username);
    //        if (user == null)
    //            return DataResponse<object>.Error("Thông tin đăng nhập không hợp lệ.");

    //        var isLockout = await _userManager.IsLockedOutAsync(user);
    //        if (isLockout)
    //            return DataResponse<object>.Error("Tài khoản của bạn đã bị khoá. Vui lòng liên hệ tư vấn viên để được hỗ trợ.");

    //        var isInActive = await _context.UserProfiles.AnyAsync(x => x.UserId == user.Id && x.Status == StatusConstant.InActive);
    //        if (isInActive)
    //            return DataResponse<object>.Error("Tài khoản của bạn đã bị khoá. Vui lòng liên hệ tư vấn viên để được hỗ trợ.");

    //        var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
    //        if (!isValidPassword)
    //            return DataResponse<object>.Error($"Thông tin đăng nhập không hợp lệ. Bạn còn {RoleConstant.MaxLoginFail - user.AccessFailedCount} lần thử");

    //        var data = await GenerateJwtToken(username);
    //        return data;
    //    }

    //    public async Task<DataResponse<object>> SaveExternalLoginInfo(ExternalTokenLoginModel model)
    //    {
    //        var user = await _userManager.FindByNameAsync(model.Email);
    //        if (user == null)
    //        {
    //            user = new User
    //            {
    //                UserName = model.Email ?? model.ProviderKey,
    //                Email = model.Email
    //            };
    //            await _userManager.CreateAsync(user);
    //            await _userManager.AddToRoleAsync(user, RoleConstant.Member);
    //            await UpdateProfile(user.Id, new SaveUserRequest()
    //            {
    //                Email = model.Email,
    //                FullName = model.Name
    //            });
    //            var addLoginResult = await _userManager.AddLoginAsync(user, new UserLoginInfo(model.LoginProvider, model.ProviderKey, model.ProviderDisplayName));
    //            if (!addLoginResult.Succeeded)
    //            {
    //                _logger.LogError("Add {Name} logged in with {LoginProvider} provider FAIL.", model.Email, model.LoginProvider);
    //            }

    //            return new DataResponse<object>();
    //        }

    //        var isInActive = await _context.UserProfiles.AnyAsync(x => x.UserId == user.Id && x.Status == StatusConstant.InActive);
    //        if (isInActive)
    //            return DataResponse<object>.Error("Tài khoản của bạn đã bị khoá. Vui lòng liên hệ tư vấn viên để được hỗ trợ.");

    //        var loginInfos = await _userManager.GetLoginsAsync(user);
    //        if (loginInfos.All(x => x.LoginProvider != model.LoginProvider))
    //        {
    //            var addLoginResult = await _userManager.AddLoginAsync(user, new UserLoginInfo(model.LoginProvider, model.ProviderKey, model.ProviderDisplayName));
    //            if (!addLoginResult.Succeeded)
    //            {
    //                _logger.LogError("Add {Name} logged in with {LoginProvider} provider FAIL.", model.Email, model.LoginProvider);
    //            }
    //        }

    //        return new DataResponse<object>();
    //    }

    //    public async Task LogOut(string userName)
    //    {
    //        var user = await _userManager.FindByNameAsync(userName);
    //        if (user == null)
    //            return;

    //        user.SecurityStamp = null;
    //        await _userManager.UpdateSecurityStampAsync(user);
    //        await _signInManager.SignOutAsync();
    //    }

    //    public async Task<bool> CheckExistUser(string phone)
    //    {
    //        var isExistUserInfo = await _userManager.Users.AnyAsync(x => x.UserName == phone);
    //        return isExistUserInfo;
    //    }

    //    public async Task<DataResponse<object>> GenerateJwtToken(string username, bool isNeedResetPassword = false)
    //    {
    //        var user = await _userManager.Users
    //            .Include(x => x.UserProfile)
    //            .FirstOrDefaultAsync(x => x.UserName == username);
    //        if (user == null)
    //            return DataResponse<object>.Error("Thông tin đăng nhập không hợp lệ.");

    //        var userRoles = await _userManager.GetRolesAsync(user);
    //        var authClaims = new List<Claim>
    //        {
    //            new("sub", user.Id.ToString()),
    //            new(ClaimTypes.Name, user.UserName ?? ""),
    //            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //            new("fullname", user.UserProfile?.FullName ?? ""),
    //            new("userUD", user.UserProfile?.UserUD ?? ""),
    //        };

    //        if (isNeedResetPassword)
    //        {

    //            authClaims.Add(new Claim("isNeedResetPassword", "true"));
    //        }

    //        foreach (var userRole in userRoles)
    //        {
    //            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
    //        }

    //        var token = CreateToken(authClaims);
    //        var refreshToken = GenerateRefreshToken();

    //        user.RefreshToken = refreshToken;
    //        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenValidityInDays);

    //        await _userManager.UpdateAsync(user);
    //        return new DataResponse<object>()
    //        {
    //            Data = new
    //            {
    //                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
    //                RefreshToken = refreshToken,
    //                Expiration = token.ValidTo
    //            }
    //        };
    //    }

    //    public async Task<DataResponse<object>> GenerateJwtToken(string refreshToken, string accessToken)
    //    {
    //        var principal = GetPrincipalFromExpiredToken(accessToken);
    //        if (principal == null)
    //        {
    //            return DataResponse<object>.Error("Invalid access token or refresh token");
    //        }

    //#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
    //#pragma warning disable CS8602 // Dereference of a possibly null reference.
    //        var username = principal.Identity.Name;
    //#pragma warning restore CS8602 // Dereference of a possibly null reference.
    //#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

    //        var user = await _userManager.FindByNameAsync(username);
    //        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
    //        {
    //            return DataResponse<object>.Error("Invalid access token or refresh token");
    //        }

    //        var newAccessToken = CreateToken(principal.Claims.ToList());
    //        var newRefreshToken = GenerateRefreshToken();

    //        user.RefreshToken = newRefreshToken;
    //        await _userManager.UpdateAsync(user);
    //        return new DataResponse<object>()
    //        {
    //            Data = new
    //            {
    //                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
    //                refreshToken = newRefreshToken,
    //                Expiration = newAccessToken.ValidTo
    //            }
    //        };
    //    }

    //    public async Task<DataResponse<object>> ChangeUserPassword(ChangePasswordRequest request)
    //    {
    //        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
    //        if (user == null)
    //            return DataResponse<object>.Error("User not found.");

    //        var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
    //        if (!isValidPassword)
    //            return DataResponse<object>.Error("Mật khẩu không hợp lệ.");

    //        var result = await _userManager.ChangePasswordAsync(user, request.Password, request.NewPassword);
    //        if (!result.Succeeded)
    //            return DataResponse<object>.Error("Đã có lỗi xảy ra. Vui lòng liên hệ kỹ thuật viên để dược hỗ trợ");

    //        var token = await GenerateJwtToken(user.UserName);
    //        return token;
    //    }

    //    public async Task<DataResponse<object>> DeActive(int userId)
    //    {
    //        var user = await _context.UserProfiles
    //            .Include(x => x.User)
    //            .FirstOrDefaultAsync(x => x.UserId == userId);
    //        if (user == null)
    //            return DataResponse<object>.Error("User not found.");

    //        user.Status = StatusConstant.InActive;
    //        await _context.SaveChangesAsync();
    //        await _userManager.UpdateSecurityStampAsync(user.User);
    //        return new DataResponse<object>();
    //    }

    //    public async Task<List<int>> GetUserIdsByRole(string? roleName = null)
    //    {
    //        var now = DateTime.UtcNow;
    //        IList<User> users;
    //        if (string.IsNullOrWhiteSpace(roleName))
    //        {
    //            var memberUsers = await _userManager.GetUsersInRoleAsync(RoleConstant.Member);
    //            var trainerUsers = await _userManager.GetUsersInRoleAsync(RoleConstant.Trainer);
    //            users = memberUsers.Union(trainerUsers).Distinct().ToList();
    //        }
    //        else
    //        {
    //            users = await _userManager.GetUsersInRoleAsync(roleName);
    //        }

    //        var userIds = users.Select(x => x.Id).ToList();

    //        // Filter locked users and active users
    //        var activeUserIds = await _userManager.Users.AsNoTracking()
    //            .Where(x => userIds.Contains(x.Id) && (!x.LockoutEnd.HasValue || x.LockoutEnd < now) && x.UserProfile.Status == StatusConstant.Active)
    //            .Select(x => x.Id)
    //            .ToListAsync();

    //        return activeUserIds;
    //    }

    //    private JwtSecurityToken CreateToken(List<Claim> authClaims)
    //    {
    //        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
    //        var token = new JwtSecurityToken(
    //            issuer: _jwtOptions.ValidIssuer,
    //            audience: _jwtOptions.ValidAudience,
    //            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.TokenValidityInMinutes),
    //            claims: authClaims,
    //            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
    //        );

    //        return token;
    //    }

    //    private static string GenerateRefreshToken()
    //    {
    //        var randomNumber = new byte[64];
    //        using var rng = RandomNumberGenerator.Create();
    //        rng.GetBytes(randomNumber);
    //        return Convert.ToBase64String(randomNumber);
    //    }

    //    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    //    {
    //        var tokenValidationParameters = new TokenValidationParameters
    //        {
    //            ValidateAudience = false,
    //            ValidateIssuer = false,
    //            ValidateIssuerSigningKey = true,
    //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)),
    //            ValidateLifetime = false
    //        };

    //        var tokenHandler = new JwtSecurityTokenHandler();
    //        var principal =
    //            tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
    //        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
    //            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
    //                StringComparison.InvariantCultureIgnoreCase))
    //            throw new SecurityTokenException("Invalid token");

    //        return principal;

    //    }
}
