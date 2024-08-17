using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;
using Application.Common.Requests;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Domain.Entities;
using Domain.Constants;

namespace Application.RegisterMails.Commands;

public class SendRegisterMailCommand : IRequest<DataResponse<bool>>
{
    public SendRegisterMailRequest Request { get; }
    public string? IP { get; }
    public SendRegisterMailCommand(SendRegisterMailRequest request, string? iP)
    {
        Request = request;
        IP = iP;
    }

    public class Handler : IRequestHandler<SendRegisterMailCommand, DataResponse<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IRecaptchaService _recaptchaService;

        public Handler(IApplicationDbContext context, IMapper mapper, IRecaptchaService recaptchaService)
        {
            _context = context;
            _mapper = mapper;
            _recaptchaService = recaptchaService;
        }

        public async Task<DataResponse<bool>> Handle(SendRegisterMailCommand request, CancellationToken cancellationToken)
        {
            var confirm = await _recaptchaService.ConfirmRecaptchaV3(request.Request.RecaptchaV3Token, request.IP);
            if (!confirm)
            {
                return DataResponse<bool>.Error("Thông tin gửi lên không được xác thực!");
            }
            RegisterMailEntity? ct = null;
            var email = await _context.RegisterMails.FirstOrDefaultAsync(x => 
                x.Email == request.Request.Email 
                && x.State == RegisterMailStateConstant.Sent, cancellationToken);
            if (email != null)
            {
                _mapper.Map(request.Request, email);
            } else
            {
                ct = _mapper.Map<RegisterMailEntity>(request.Request);
                if (ct != null)
                {
                    await _context.RegisterMails.AddAsync(ct!);
                }
                else
                {
                    return DataResponse<bool>.Error("Có lỗi khi chuyển đổi dữ liệu!");
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
            return DataResponse<bool>.Success(true);
        }

    }
}