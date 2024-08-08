using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Domain.Entities;

namespace Application.Sliders.Queries;

public class GetSliderByPositionQuery : IRequest<SliderEntity?>
{
    public int Position { get; }

    public GetSliderByPositionQuery(int position)
    {
        Position = position;
    }

    public class Handler : IRequestHandler<GetSliderByPositionQuery, SliderEntity?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SliderEntity?> Handle(GetSliderByPositionQuery request, CancellationToken cancellationToken)
        {
            var slider = await _context.Sliders.AsNoTracking()
                .Include(x => x.Items).ThenInclude(x => x.Image)
                .FirstOrDefaultAsync(x => x.Position == request.Position, cancellationToken);

            return slider;
        }
    }
}