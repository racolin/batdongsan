using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Common.Responses.Views;
using AutoMapper;
using Domain.Constants;

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

            var result = await _context.Contents.AsNoTracking()
                .Include(x => x.ContactImage)
                .Select(x => new { x.ContactImage, x.Status })
                .Where(x => x.Status == StatusConstant.Active)
                .FirstOrDefaultAsync(cancellationToken);
            if (result != null)
            {
                _mapper.Map(result!.ContactImage, view.TopImage);
            }

            return view;
        }
    }
}