using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;
using Application.Common.Requests;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Domain.Entities;

namespace Application.Configurations.Commands;

public class UpdateConfigurationCommand : IRequest<DataResponse<int>>
{
    public SaveConfigurationRequest Request { get; }
    public UpdateConfigurationCommand(SaveConfigurationRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<UpdateConfigurationCommand, DataResponse<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;

        public Handler(IApplicationDbContext context, IMapper mapper, IMailService mailService)
        {
            _context = context;
            _mapper = mapper;
            _mailService = mailService;
        }

        public async Task<DataResponse<int>> Handle(UpdateConfigurationCommand request, CancellationToken cancellationToken)
        {
            var id = request.Request.Id;
            if (id == null)
            {
                return DataResponse<int>.Error("Id không thể để trống!");
            }
            var configuration = await _context.Configurations.FindAsync(id);
            if (configuration == null)
            {
                return DataResponse<int>.Error("Không tìm thấy cấu hình!");
            }

            _mapper.Map(request.Request, configuration);

            await _context.SaveChangesAsync(cancellationToken);

            return DataResponse<int>.Success(id ?? 0);
        }

    }
}