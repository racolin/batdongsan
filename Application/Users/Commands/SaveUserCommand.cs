using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using MediatR;

namespace Application.Users.Commands;

public class SaveUserCommand : IRequest<DataResponse<int>>
{
    public SaveUserRequest Request { get; }

    public SaveUserCommand(SaveUserRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<SaveUserCommand, DataResponse<int>>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<DataResponse<int>> Handle(SaveUserCommand request, CancellationToken cancellationToken)
        {
            DateTime? dob = null;
            if (request.Request.DateOfBirth != null)
            {
                try
                {
                    dob = DateTime.ParseExact(request.Request.DateOfBirth, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {

                }
            };

            var result = await _identityService.SaveUserAsync(
                request.Request.Id,
                request.Request.Username,
                request.Request.Email,
                request.Request.Name,
                request.Request.Phone,
                request.Request.Gender,
                dob
            );

            return result;
        }
    }
}