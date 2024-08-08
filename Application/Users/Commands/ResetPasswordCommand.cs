using MediatR;
using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;

namespace Application.Users.Commands;

public class ResetPasswordCommand : IRequest<DataResponse<bool>>
{
    public ResetPasswordRequest Request { get; }
    public ResetPasswordCommand(ResetPasswordRequest request) 
    {
        Request = request;
    }

    public class Handler : IRequestHandler<ResetPasswordCommand, DataResponse<bool>>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<DataResponse<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.ResetPasswordWithTokenAsync(request.Request.Username, request.Request.Token, request.Request.Password);

            if (result == null)
            {
                return DataResponse<bool>.Error("Không thực hiện yêu cầu đặt lại mật khẩu!", []);
            }
            
            return result!;
        }
    }
}