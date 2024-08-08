﻿//using MediatR;

//namespace Application.Account.Commands;

//public class ChangePasswordCommand : IRequest<DataResponse<SliderEntity>>
//{
//    public ChangePasswordRequest Request { get; }
//    public bool IsChangeUserPassword { get; }
//    public ChangePasswordCommand(ChangePasswordRequest request, bool isChangeUserPassword = false)
//    {
//        Request = request;
//        IsChangeUserPassword = isChangeUserPassword;
//    }

//    public class Handler : IRequestHandler<ChangePasswordCommand, DataResponse<SliderEntity>>
//    {
//        private readonly IIdentityService _identityService;
//        private readonly ICurrentUserService _currentUser;

//        public Handler(IIdentityService identityService, ICurrentUserService currentUser)
//        {
//            _identityService = identityService;
//            _currentUser = currentUser;
//        }

//        public async Task<DataResponse<SliderEntity>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
//        {
//            if (!_currentUser.UserId.HasValue)
//                return DataResponse<SliderEntity>.Fail("");

//            // Member change profile password
//            if (!request.IsChangeUserPassword)
//            {
//                request.Request.UserId = _currentUser.UserId.Value;
//                var isValid = await VerifyPassword(request.Request.NewPassword, _currentUser.UserId.Value, request.Request.UserId);
//                if (!isValid)
//                    return DataResponse<SliderEntity>.Fail("Mật khẩu phải ít nhất 6 ký tự - Mật khẩu không đúng định dạng");
//            }

//            var result = await _identityService.ChangeUserPassword(request.Request);
//            return result;
//        }
//    }
//}