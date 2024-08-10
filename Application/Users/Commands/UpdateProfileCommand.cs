using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using MediatR;

namespace Application.Users.Commands;

public class UpdateProfileCommand : IRequest<DataResponse<bool>>
{
    public SaveUserRequest Request { get; }

    public UpdateProfileCommand(SaveUserRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<UpdateProfileCommand, DataResponse<bool>>
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _identityService = identityService;
            _currentUserService = currentUserService;
        }

        public async Task<DataResponse<bool>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var id = _currentUserService.UserId;
            if (id == null)
            {
                return DataResponse<bool>.Error("Người dùng không xác định. Hãy liên hệ admin!");
            }

            DateTime? dob = null;
            if (request.Request.DateOfBirth != null)
            {
                try
                {
                    dob = DateTime.ParseExact(request.Request.DateOfBirth, "dd-MM-yyyy", null);
                }
                catch (Exception ex)
                {

                }
            };

            var result = await _identityService.UpdateProfileAsync(
                id.Value,
                request.Request.Name,
                request.Request.Gender,
                dob
            );

            return result;
        }
    }
}