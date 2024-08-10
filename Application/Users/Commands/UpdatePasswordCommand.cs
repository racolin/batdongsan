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

        public Handler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<DataResponse<bool>> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.AdminUpdatePasswordAsync(request.Request.Id, request.Request.Password);
            return result;
        }

    }
}