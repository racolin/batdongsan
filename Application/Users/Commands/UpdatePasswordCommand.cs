using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using MediatR;

namespace Application.Account.Commands;

public class UpdatePasswordCommand : IRequest<DataResponse<bool>>
{
    public UpdatePasswordRequest Request { get; }
    public UpdatePasswordCommand(UpdatePasswordRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<UpdatePasswordCommand, DataResponse<bool>>
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _identityService = identityService;
            _currentUserService = currentUserService;
        }

        public async Task<DataResponse<bool>> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var currentUsername = _currentUserService.UserName;
            if (string.IsNullOrEmpty(currentUsername) || currentUsername != request.Request.Username) {
                return DataResponse<bool>.Error("Bạn không có quyền thực hiện hành động này!");
            }
            var result = await _identityService.UpdatePasswordAsync(currentUsername, request.Request.Password, request.Request.NewPassword);
            return result;
        }

    }
}