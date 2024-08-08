//using MediatR;

//namespace Application.Account.Commands;

//public class ChangeUserRoleCommand : IRequest<DataResponse<SliderEntity>>
//{
//    public int userId { get; }

//    public ChangeUserRoleCommand(int request)
//    {
//        userId = request;
//    }

//    public class Handler : IRequestHandler<ChangeUserRoleCommand, DataResponse<SliderEntity>>
//    {
//        private readonly IIdentityService _identityService;

//        public Handler(IIdentityService identityService)
//        {
//            _identityService = identityService;
//        }

//        public async Task<DataResponse<SliderEntity>> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
//        {
//            var result = await _identityService.ChangeRoleAsync(request.userId, RoleConstant.Trainer, RoleConstant.Member);
//            if (result)
//            {
//                return new DataResponse<SliderEntity>() { Data = result };
//            }
//            else
//            {
//                return DataResponse<SliderEntity>.Fail("Không thể chuyển đổi tài khoản này, vui lòng liên hệ quản trị");
//            }
//        }
//    }
//}