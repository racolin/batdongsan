using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using Domain.Entities;

namespace Application.Images.Commands;
public class SaveContentCommand : IRequest<DataResponse<bool>>
{
    public SaveContentRequest Request { get; }

    public SaveContentCommand(SaveContentRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<SaveContentCommand, DataResponse<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DataResponse<bool>> Handle(SaveContentCommand request, CancellationToken cancellationToken)
        {
            if (request.Request.IsUpdateImagePages) 
            {
                var imagePageIds = request.Request.ImagePages.Select(x => x.Id).ToList();
                var imagePages = await _context.ImagePages
                    .Where(x => imagePageIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);
                if (imagePages != null) {
                    foreach (var imagePage in imagePages)
                    {
                        var item = request.Request.ImagePages.FirstOrDefault(x => x.Id == imagePage.Id && x.Position == imagePage.Position);
                        if (item != null)
                        {
                            imagePage.ImageId = item.ImageId;
                        }
                    }
                }
            }

            if (request.Request.IsUpdateSlider) {

                var slider = await _context.Sliders
                    .Include(x => x.Items)
                    .FirstOrDefaultAsync(x => x.Id == request.Request.Slider.Id);
                if (slider != null)
                {
                    var sliderRequestIds = request.Request.Slider.Items.Select(x => x.Id).ToList();
                    var items = slider.Items.Where(x => !sliderRequestIds.Contains(x.Id)).ToList();
                    foreach (var item in items)
                    {
                        slider.Items.Remove(item);
                    }
                    foreach (var item in request.Request.Slider.Items)
                    {
                        if (item.Id == null)
                        {
                            var s = _mapper.Map<SliderImageEntity>(item);
                            slider.Items.Add(s);
                        }
                        else
                        {
                            var s = slider.Items.FirstOrDefault(x => x.Id == item.Id);
                            if (s != null)
                            {
                                _mapper.Map(item, s);
                            }
                        }
                    }
                }
            }

            if (request.Request.IsUpdateIntroduce)
            {
                var introduce = await _context.Sections
                    .FirstOrDefaultAsync(x => x.Id == request.Request.Introduce.Id && x.Position == request.Request.Introduce.Position);
                if (introduce != null)
                {
                    introduce.Content = request.Request.Introduce.Content;
                }
            }

            var newsIds = request.Request.News.Select(x => x.Id).ToList();
            var news = await _context.Sections
                .Where(x => newsIds.Contains(x.Id))
                .ToListAsync(cancellationToken);

            if (news != null)
            {
                foreach (var newsItem in news)
                {
                    var item = request.Request.News.FirstOrDefault(x => x.Id == newsItem.Id && x.Position == newsItem.Position);
                    if (item != null)
                    {
                        newsItem.Content = item.Content;
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new DataResponse<bool> { Data = true };
        }
    }
}