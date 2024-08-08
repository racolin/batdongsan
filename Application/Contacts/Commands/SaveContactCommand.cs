using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;
using Application.Common.Requests;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Domain.Entities;
using Domain.Constants;

namespace Application.Contacts.Commands;

public class SaveContactCommand : IRequest<DataResponse<int>>
{
    public SaveContactRequest Request { get; }
    public SaveContactCommand(SaveContactRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<SaveContactCommand, DataResponse<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DataResponse<int>> Handle(SaveContactCommand request, CancellationToken cancellationToken)
        {
            request.Request.State = request.Request.State ?? ContactStateConstant.Sent;
            var id = request.Request.Id;
            if (id != null) {
                var contact = await _context.Contacts.FindAsync(id);
                if (contact != null)
                {
                    _mapper.Map(request.Request, contact);
                } else
                {
                    return DataResponse<int>.Error("Không tìm thấy liên hệ muốn lưu!");
                }
            }
            ContactEntity? ct = null;
            if (id == null) { 
                var contact = await _context.Contacts
                    .FirstOrDefaultAsync(x => 
                        x.Email == request.Request.Email 
                        && request.Request.State == ContactStateConstant.Sent, cancellationToken);
                if (contact != null)
                {
                    id = contact.Id;
                    _mapper.Map(request.Request, contact);
                    contact.Id = id.Value;
                } else
                {
                    ct = _mapper.Map<ContactEntity>(request.Request);
                    if (ct != null)
                    {
                        await _context.Contacts.AddAsync(ct!);
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