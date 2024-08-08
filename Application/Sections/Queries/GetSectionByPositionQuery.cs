using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Domain.Entities;

namespace Application.Sections.Queries;

public class GetSectionByPositionQuery : IRequest<SectionEntity?>
{
    public int Position { get; }

    public GetSectionByPositionQuery(int position)
    {
        Position = position;
    }

    public class Handler : IRequestHandler<GetSectionByPositionQuery, SectionEntity?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SectionEntity?> Handle(GetSectionByPositionQuery request, CancellationToken cancellationToken)
        {
            var Section = await _context.Sections.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Position == request.Position, cancellationToken);

            return Section;
        }
    }
}