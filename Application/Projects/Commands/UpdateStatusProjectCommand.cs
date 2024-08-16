using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses;
using Application.Common.Requests;
using Domain.Constants;

namespace Application.Projects.Commands;

public class UpdateStatusProjectCommand : IRequest<DataResponse<bool>>
{
    public UpdateStatusRequest Request { get; }
    public UpdateStatusProjectCommand(UpdateStatusRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<UpdateStatusProjectCommand, DataResponse<bool>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<bool>> Handle(UpdateStatusProjectCommand request, CancellationToken cancellationToken)
        {
            var id = request.Request.Id;
            var project = await _context.Projects.FindAsync(id);
            if (project == null) {
                return DataResponse<bool>.Error("Không tìm thấy dự án muốn cập nhật!");
            }

            if (!StatusConstant.GetAllProperties().Contains(request.Request.Status))
            {
                return DataResponse<bool>.Error("Tình trạng gửi lên không chính xác!");
            }

            project.Status = request.Request.Status;

            await _context.SaveChangesAsync(cancellationToken);

            return DataResponse<bool>.Success(true);
        }

    }
}