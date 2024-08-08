//using Application.Common.Interfaces;
//using Application.Common.Models;
//using MediatR;

//namespace Application.Account.Commands;

//public class RefreshTokenCommand : IRequest<DataResponse<SliderEntity>>
//{
//    public RefreshTokenRequest Request { get; }
//    public RefreshTokenCommand(RefreshTokenRequest request)
//    {
//        Request = request;
//    }

//    public class Handler : IRequestHandler<RefreshTokenCommand, DataResponse<SliderEntity>>
//    {
//        private readonly IIdentityService _identityService;

//        public Handler(IIdentityService identityService)
//        {
//            _identityService = identityService;
//        }

//        public async Task<DataResponse<SliderEntity>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
//        {
//            var model = await _identityService.GenerateJwtToken(request.Request.RefreshToken, request.Request.AccessToken);
//            return model;
//        }
//    }
//}