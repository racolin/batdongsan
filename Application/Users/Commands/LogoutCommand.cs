using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;

namespace Application.Users.Commands;

public class LogoutCommand : IRequest<DataResponse<bool>>
{
    public LogoutCommand() {}

    public class Handler : IRequestHandler<LogoutCommand, DataResponse<bool>>
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _identityService = identityService;
            _currentUserService = currentUserService;
        }

        public async Task<DataResponse<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                return DataResponse<bool>.Error("Không thể đăng nhập vì bạn chưa đăng nhập!");
            }
            var result = await _identityService.LogoutAsync(userId.Value);

            return result;
        }
    }
}