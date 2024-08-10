using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using MediatR;

namespace Application.Account.Commands;

public class UpdateMyPasswordCommand : IRequest<DataResponse<bool>>
{
    public UpdateMyPasswordRequest Request { get; }
    public UpdateMyPasswordCommand(UpdateMyPasswordRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<UpdateMyPasswordCommand, DataResponse<bool>>
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _identityService = identityService;
            _currentUserService = currentUserService;
        }

        public async Task<DataResponse<bool>> Handle(UpdateMyPasswordCommand request, CancellationToken cancellationToken)
        {
            var currentUsername = _currentUserService.UserName;
            if (string.IsNullOrEmpty(currentUsername))
            {
                return DataResponse<bool>.Error("Người dùng không xác định. Hãy liên hệ admin!");
            }
            var result = await _identityService.UpdatePasswordAsync(currentUsername, request.Request.Password, request.Request.NewPassword);
            return result;
        }

    }
}