using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;
using Application.Common.Requests;
using Application.Common.Responses.Admin;

namespace Application.Users.Commands;

public class LoginCommand : IRequest<DataResponse<LoginResponse>>
{
    public LoginRequest Request { get; }

    public LoginCommand(LoginRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<LoginCommand, DataResponse<LoginResponse>>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<DataResponse<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.LoginAsync(request.Request.Username, request.Request.Password);

            return result;
        }
    }
}