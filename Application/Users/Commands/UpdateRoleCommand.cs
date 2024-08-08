using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using MediatR;

namespace Application.Account.Commands;

public class UpdateRoleCommand : IRequest<DataResponse<bool>>
{
    public UpdateRoleRequest Request { get; }
    public bool IsAdd { get; }
    public UpdateRoleCommand(UpdateRoleRequest request, bool isAdd)
    {
        Request = request;
        IsAdd = isAdd;
    }

    public class Handler : IRequestHandler<UpdateRoleCommand, DataResponse<bool>>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<DataResponse<bool>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            DataResponse<bool> result;
            if (request.IsAdd) 
            {
                result = await _identityService.AddRoleAsync(request.Request.UserId, request.Request.Role);
            } else 
            {
                result = await _identityService.RemoveRoleAsync(request.Request.UserId, request.Request.Role);
            }
            return result;
        }

    }
}