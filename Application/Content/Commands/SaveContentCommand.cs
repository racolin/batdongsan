using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Application.Common.Requests;
using Application.Common.Responses;
using Domain.Entities;
using Domain.Constants;

namespace Application.Images.Commands;
public class SaveContentCommand : IRequest<DataResponse<int>>
{
    public SaveContentRequest Request { get; }

    public SaveContentCommand(SaveContentRequest request)
    {
        Request = request;
    }

    public class Handler : IRequestHandler<SaveContentCommand, DataResponse<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DataResponse<int>> Handle(SaveContentCommand request, CancellationToken cancellationToken)
        {
            var newStatus = request.Request.Status;
            var oldStatus = request.Request.Status;
            ContentEntity? content;
            if (request.Request.Id == null)
            {

                content = _mapper.Map<ContentEntity>(request.Request);
                _context.Contents.Add(content);
            }
            else
            {
                content = await _context.Contents
                    .Include(x => x.ProjectSlider)
                    .FirstOrDefaultAsync(x => x.Id == request.Request.Id, cancellationToken);
                if (content == null)
                {
                    return DataResponse<int>.Error("Không thể cập nhật nội dung!");
                }
                oldStatus = content.Status;

                content.NewsMarketSection = request.Request.NewsMarketSection;
                content.NewsProjectSection = request.Request.NewsProjectSection;
                content.Status = request.Request.Status;
                content.HomeImageId = request.Request.HomeImageId;
                content.BgHomeImageId = request.Request.BgHomeImageId;
                content.NewsImageId = request.Request.NewsImageId;
                content.ContactImageId = request.Request.ContactImageId;

                if (request.Request.IsUpdateProjectSlider)
                {
                    foreach (var item in content.ProjectSlider) 
                    {
                        var i = request.Request.ProjectSlider.FirstOrDefault(x => x.Id == item.Id);
                        if (i == null)
                        {
                            _context.SliderImages.Remove(item); 
                        } else
                        {
                            item.Order = i.Order;
                            item.Link =i.Link;
                            item.ImageId = i.ImageId;
                        }
                    }

                    var items = _mapper.Map<List<SliderImageEntity>>(request.Request.ProjectSlider.Where(x => x.Id == null)).ToList();
                    content.ProjectSlider = content.ProjectSlider.Concat(items).ToList();
                }

                if (request.Request.IsUpdateIntroduceSection)
                {
                    content.IntroduceSection = request.Request.IntroduceSection;
                }

            }

            var isActive =  request.Request.Status == StatusConstant.Active;
            if (oldStatus != newStatus)
            {
                if (oldStatus == StatusConstant.Active)
                {
                    content.Status = StatusConstant.Active;
                }
                
                if (newStatus == StatusConstant.Active)
                {
                    var items = await _context.Contents.Where(x => x.Status == StatusConstant.Active).ToListAsync();
                    foreach (var item in items) { 
                        item.Status = StatusConstant.Draft;
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            if (content == null)
            {
                return DataResponse<int>.Error("Không lưu được nội dung trang web này!");
            }

            return new DataResponse<int> { Data = content!.Id };
        }
    }
}