using MediatR;
using Application.Common.Interfaces;
using Application.Common.Responses;

namespace Application.Users.Commands
{
    public class ForgotPasswordCommand : IRequest<DataResponse<bool>>
    {
        public string Username { get; }
        public string Browser { get; }
        public string Operation { get; }

        public ForgotPasswordCommand(string username, string operation, string browser)
        {
            Username = username;
            Browser = browser;
            Operation = operation;
        }
        public class Handler : IRequestHandler<ForgotPasswordCommand, DataResponse<bool>>
        {
            private readonly IIdentityService _identityService;

            public Handler(IIdentityService identityService)
            {
                _identityService = identityService;
            }

            public async Task<DataResponse<bool>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
            {
                var result = await _identityService.GeneratePasswordResetTokenAsync(request.Username, request.Operation, request.Browser);
                return result;
            }
        }
    }
}