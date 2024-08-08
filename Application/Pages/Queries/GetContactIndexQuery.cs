using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Common.Responses.Views;
using Domain.Enums;
using AutoMapper;

namespace Application.Pages.Queries;

public class GetContactIndexQuery : IRequest<ContactIndexView>
{
    public GetContactIndexQuery() {}

    public class Handler : IRequestHandler<GetContactIndexQuery, ContactIndexView>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ContactIndexView> Handle(GetContactIndexQuery request, CancellationToken cancellationToken)
        {
            var view = new ContactIndexView();

            var image = await _context.ImagePages.AsNoTracking()
                .Include(x => x.Image)
                .FirstOrDefaultAsync(x => x.Position == (int)ImagePagePositionEnum.ContactScreen, cancellationToken);
            if (image != null) {
                _mapper.Map(image!.Image, view.TopImage);
            }

            return view;
        }
    }
}