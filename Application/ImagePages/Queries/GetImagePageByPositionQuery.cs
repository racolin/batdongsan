using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Domain.Entities;

namespace Application.ImagePages.Queries;

public class GetImagePageByPositionQuery : IRequest<ImagePageEntity?>
{
    public int Position { get; }

    public GetImagePageByPositionQuery(int position)
    {
        Position = position;
    }

    public class Handler : IRequestHandler<GetImagePageByPositionQuery, ImagePageEntity?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ImagePageEntity?> Handle(GetImagePageByPositionQuery request, CancellationToken cancellationToken)
        {
            var imagePage = await _context.ImagePages.AsNoTracking()
                .Include(x => x.Image)
                .FirstOrDefaultAsync(x => x.Position == request.Position, cancellationToken);

            return imagePage;
        }
    }
}