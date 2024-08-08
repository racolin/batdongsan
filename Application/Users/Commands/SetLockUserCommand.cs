using Application.Common.Interfaces;
using MediatR;
using Application.Common.Requests;
using Application.Common.Responses;

namespace Application.Users.Commands;

public class SetLockUserCommand : IRequest<DataResponse<bool>>
{
    public SetLockUserRequest Request { get; }

    public SetLockUserCommand(SetLockUserRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<SetLockUserCommand, DataResponse<bool>>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<DataResponse<bool>> Handle(SetLockUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.SetLockUser(request.Request.UserId, request.Request.IsLock);
            return result;
        }
    }
}