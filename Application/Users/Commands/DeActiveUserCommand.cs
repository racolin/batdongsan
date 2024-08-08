//using Application.Common.Interfaces;
//using Application.Common.Models;
//using MediatR;
//using Application.Common.Requests;

//namespace Application.Account.Commands.DeActive;

//public class DeActiveUserCommand : IRequest<DataResponse<SliderEntity>>
//{
//    public DeActiveUserRequest Request { get; }

//    public DeActiveUserCommand(DeActiveUserRequest request)
//    {
//        Request = request;
//    }

//    public class Handler : IRequestHandler<DeActiveUserCommand, DataResponse<SliderEntity>>
//    {
//        private readonly IIdentityService _identityService;

//        public Handler(IIdentityService identityService)
//        {
//            _identityService = identityService;
//        }

//        public async Task<DataResponse<SliderEntity>> Handle(DeActiveUserCommand request, CancellationToken cancellationToken)
//        {
//            var result = await _identityService.DeActive(request.Request.UserId);
//            return result;
//        }
//    }
//}