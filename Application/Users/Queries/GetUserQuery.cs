using Application.Common.Interfaces;
using Application.Common.Responses;
using Application.Common.Responses.Admin;
using MediatR;

namespace Application.Users.Queries;

public class GetUserQuery : IRequest<DataResponse<UserResponse>>
{
    public int Id { get; }

    public GetUserQuery(int id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<GetUserQuery, DataResponse<UserResponse>>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<DataResponse<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _identityService.GetUserAsync(request.Id);
            return result;
        }
    }
}