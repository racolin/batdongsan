using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using Application.Common.Responses.Admin;
using MediatR;

namespace Application.Users.Queries;

public class GetUsersQuery : IRequest<DataResponse<PagingResponse<UserResponse>>>
{
    public SearchRequest Request { get; }

    public GetUsersQuery(SearchRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<GetUsersQuery, DataResponse<PagingResponse<UserResponse>>>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService)
        {
            _identityService = identityService;

        }

        public async Task<DataResponse<PagingResponse<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _identityService.GetPagingUserModelsAsync(request.Request, cancellationToken);
            return result;
        }
    }
}