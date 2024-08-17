using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;
using Application.Common.Requests;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Domain.Entities;
using Domain.Constants;

namespace Application.Contacts.Commands;

public class SendContactCommand : IRequest<DataResponse<bool>>
{
    public SendContactRequest Request { get; }
    public string? IP { get; }
    public SendContactCommand(SendContactRequest request, string? iP)
    {
        Request = request;
        IP = iP;
    }

    public class Handler : IRequestHandler<SendContactCommand, DataResponse<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IRecaptchaService _recaptchaService;

        public Handler(IApplicationDbContext context, IMapper mapper, IMailService mailService, IRecaptchaService recaptchaService)
        {
            _context = context;
            _mapper = mapper;
            _mailService = mailService;
            _recaptchaService = recaptchaService;
        }

        public async Task<DataResponse<bool>> Handle(SendContactCommand request, CancellationToken cancellationToken)
        {
            var confirm = await _recaptchaService.ConfirmRecaptchaV3(request.Request.RecaptchaV3Token, request.IP);
            if (!confirm)
            {
                return DataResponse<bool>.Error("Thông tin gửi lên không được xác thực!");
            }
            ContactEntity? ct = null;
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(x => 
                    x.Email == request.Request.Email 
                    && x.State == ContactStateConstant.Sent, cancellationToken);
            if (contact != null)
            {
                _mapper.Map(request.Request, contact);
            } else
            {
                ct = _mapper.Map<ContactEntity>(request.Request);
                if (ct != null)
                {
                    await _context.Contacts.AddAsync(ct!);
                }
                else
                {
                    return DataResponse<bool>.Error("Có lỗi khi chuyển đổi dữ liệu!");
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
            if (ct != null) {
                var config = await _context.Configurations.FirstOrDefaultAsync(cancellationToken);
                if (config != null)
                {
                    await _mailService.SendAdminContact(config.ReceiveEmail, ct.Id, ct.Name, ct.Email, ct.Phone, ct.Address, ct.Content);
                }
            }
            return DataResponse<bool>.Success(true);
        }

    }
}