using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;
using Application.Common.Requests;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Domain.Entities;
using Domain.Constants;

namespace Application.RegisterMails.Commands;

public class SaveRegisterMailCommand : IRequest<DataResponse<int>>
{
    public SaveRegisterMailRequest Request { get; }
    public SaveRegisterMailCommand(SaveRegisterMailRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<SaveRegisterMailCommand, DataResponse<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DataResponse<int>> Handle(SaveRegisterMailCommand request, CancellationToken cancellationToken)
        {
            request.Request.State = request.Request.State ?? RegisterMailStateConstant.Sent;
            var id = request.Request.Id;
            if (id != null) {
                var contact = await _context.RegisterMails.FindAsync(id);
                if (contact != null)
                {
                    _mapper.Map(request.Request, contact);
                } else
                {
                    return DataResponse<int>.Error("Không tìm thấy liên hệ muốn lưu!");
                }
            }
            RegisterMailEntity? ct = null;
            if (id == null) { 
                var contact = await _context.RegisterMails.FirstOrDefaultAsync(x => 
                    x.Email == request.Request.Email 
                    && request.Request.State == RegisterMailStateConstant.Sent, cancellationToken);
                if (contact != null)
                {
                    id = contact.Id;
                    _mapper.Map(request.Request, contact);
                    contact.Id = id.Value;
                } else
                {
                    ct = _mapper.Map<RegisterMailEntity>(request.Request);
                    if (ct != null)
                    {
                        await _context.RegisterMails.AddAsync(ct!);
                    }
                    else
                    {
                        return DataResponse<int>.Error("Có lỗi khi chuyển đổi dữ liệu!");
                    }
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
            if (ct != null) {
                id = ct.Id;
            }
            return DataResponse<int>.Success(id ?? 0);
        }

    }
}