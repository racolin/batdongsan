//using MediatR;
//using Application.Common.Interfaces;
//using Domain.Constants;
//using Application.Common.Responses;
//using Application.Common.Requests;
//using Application.Common.Models;

//namespace Application.Users.Queries
//{
//    public class GetUserQuery : IRequest<DataResponse<UserModel>>
//    {
//        public readonly int Id;

//        public GetUserQuery(int id)
//        {
//            Id = id;
//        }

//        public class Handler : IRequestHandler<GetUserQuery, DataResponse<SliderEntity>>
//        {
//            private readonly IApplicationDbContext _context;
//            private readonly IIdentityService _identityService;

//            public Handler(IApplicationDbContext context, IIdentityService identityService)
//            {
//                _identityService = identityService;
//                _context = context;
//            }

//            public async Task<DataResponse<SliderEntity>> Handle(GetUserQuery request, CancellationToken cancellationToken)
//            {
//                List<UserProfileModel> result = new List<UserProfileModel>();
//                SearchRequest param = new SearchRequest();

//                int totalRows = _identityService.TotalUserActive(RoleConstant.Member).Result;
//                if (!string.IsNullOrEmpty(request.Json))
//                    param = JsonConvert.DeserializeObject<SearchRequest>(request.Json.ToString());

//                if (param.Length > 0)
//                    result = _identityService.GetUserProfiles(RoleConstant.Member, CancellationToken.None, param.Start, param.Length, param.Search.Value).Result;
//                else
//                    result = _identityService.GetUserProfiles(RoleConstant.Member, CancellationToken.None).Result;

//                return new DataResponse<SliderEntity>()
//                {
//                    Data = result,
//                    TotalRows = result.Count,
//                    RecordsTotal = totalRows,
//                    RecordsFiltered = totalRows
//                };
//            }
//        }
//    }
//}