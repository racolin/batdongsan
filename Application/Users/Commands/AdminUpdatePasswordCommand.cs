using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using MediatR;

namespace Application.Account.Commands;

public class AdminUpdatePasswordCommand : IRequest<DataResponse<bool>>
{
    public AdminUpdatePasswordRequest Request { get; }
    public AdminUpdatePasswordCommand(AdminUpdatePasswordRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<AdminUpdatePasswordCommand, DataResponse<bool>>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<DataResponse<bool>> Handle(AdminUpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.AdminUpdatePasswordAsync(request.Request.Id, request.Request.Password);
            return result;
        }

    }
}