using Application.Common.Interfaces;
using Application.Common.Responses;
using Application.Common.Responses.Admin;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Images.Queries;

public class GetReferencesOfImageQuery : IRequest<DataResponse<List<ReferencesResponse>>>
{
    public int Id { get; }

    public GetReferencesOfImageQuery(int id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<GetReferencesOfImageQuery, DataResponse<List<ReferencesResponse>>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<List<ReferencesResponse>>> Handle(GetReferencesOfImageQuery request, CancellationToken cancellationToken)
        {
            var references = new List<ReferencesResponse>();
            //Project
            var projects = await _context.Projects.AsNoTracking()
                .Where(x => x.ImageId == request.Id)
                .Select(x => new ReferenceResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .ToListAsync(cancellationToken);
            if (projects.Any())
            {
                references.Add(new ReferencesResponse
                {
                    Ud = "project",
                    UdName = "Dự án",
                    References = projects
                });
            }
            //News
            var newsList = await _context.News.AsNoTracking()
                .Where(x => x.ImageId == request.Id)
                .Select(x => new ReferenceResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .ToListAsync(cancellationToken);
            if (newsList.Any())
            {
                references.Add(new ReferencesResponse
                {
                    Ud = "news",
                    UdName = "Tin tức",
                    References = newsList
                });
            }
            //Content
            var contents = await _context.Contents.AsNoTracking()
                .Include(x => x.ProjectSlider)
                .Where(x => 
                    x.HomeImageId == request.Id
                    || x.BgHomeImageId == request.Id
                    || x.NewsImageId == request.Id
                    || x.ContactImageId == request.Id
                    || x.ProjectSlider.Any(x => x.ImageId == request.Id)
                )
                .ToListAsync(cancellationToken);
            if (contents.Any())
            {
                var items = new List<ReferenceResponse>();
                foreach (var content in contents)
                {
                    // Home in content
                    if (content.HomeImageId == request.Id)
                    {
                        items.Add(new ReferenceResponse { 
                            Id = content.Id,
                            Name = "Ảnh trên cùng trang chủ ở \"" + content.Name + "\"",
                        });
                    }
                    // BgHome in content
                    if (content.BgHomeImageId == request.Id)
                    {
                        items.Add(new ReferenceResponse
                        {
                            Id = content.Id,
                            Name = "Ảnh nền trang chủ ở \"" + content.Name + "\"",
                        });
                    }
                    // News in content
                    if (content.NewsImageId == request.Id)
                    {
                        items.Add(new ReferenceResponse
                        {
                            Id = content.Id,
                            Name = "Ảnh trên cùng tin tức ở \"" + content.Name + "\"",
                        });
                    }
                    // Contact in content
                    if (content.ContactImageId == request.Id)
                    {
                        items.Add(new ReferenceResponse
                        {
                            Id = content.Id,
                            Name = "Ảnh trên cùng liên hệ ở \"" + content.Name + "\"",
                        });
                    }
                    //SliderImage in content
                    if (content.ProjectSlider.Any(x => x.ImageId == request.Id))
                    {
                        foreach (var slider in  content.ProjectSlider)
                        {
                            if (slider.ImageId == request.Id)
                            {
                                items.Add(new ReferenceResponse
                                {
                                    Id = content.Id,
                                    Name = $"Ảnh slider dự án (order: {slider.Order}) ở " + content.Name,
                                });
                            }
                        }
                    }
                }
                references.Add(new ReferencesResponse
                {
                    Ud = "content",
                    UdName = "Nội dung web",
                    References = items
                });
            }
            return DataResponse<List<ReferencesResponse>>.Success(references);
        }
    }
}