using Application.Common.Interfaces;
using Application.Common.Responses;
using Application.Common.Responses.Admin;
using MediatR;

namespace Application.Users.Queries;

public class GetProfileQuery : IRequest<DataResponse<UserResponse>>
{
    public GetProfileQuery()
    {
    }

    public class Handler : IRequestHandler<GetProfileQuery, DataResponse<UserResponse>>
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _identityService = identityService;
            _currentUserService = currentUserService;   

        }

        public async Task<DataResponse<UserResponse>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var id = _currentUserService.UserId;
            if (id == null)
            {
                return DataResponse<UserResponse>.Error("Người dùng không xác định. Hãy liên hệ admin!");
            }
            var result = await _identityService.GetProfileAsync(id.Value);
            return result;
        }
    }
}