//using AutoMapper;
//using Application.Common.Models;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using Application.Common.Interfaces;
//using Application.News.Commands.SaveNews;
//using Application.Common.Requests;

//namespace Application.News.Commands;
//public class SaveNewsCommand : IRequest<DataResponse<SliderEntity>>
//{
//    public SaveNewsRequest Request { get; }

//    public SaveNewsCommand(SaveNewsRequest request)
//    {
//        Request = request;
//    }

//    public class Handler : IRequestHandler<SaveNewsCommand, DataResponse<SliderEntity>>
//    {
//        private readonly IApplicationDbContext _context;
//        private readonly IFileService _fileService;
//        private readonly IMapper _mapper;

//        public Handler(IApplicationDbContext context, IFileService fileService, IMapper mapper)
//        {
//            _context = context;
//            _fileService = fileService;
//            _mapper = mapper;
//        }

//        public async Task<DataResponse<SliderEntity>> Handle(SaveNewsCommand request, CancellationToken cancellationToken)
//        {
//            var news = await _context.News.FirstOrDefaultAsync(x => x.Id == request.Request.Id, cancellationToken);
//            if (news == null)
//            {
//                news = _mapper.Map<Domain.Entities.NewsEntity>(request.Request);
//                _context.News.Add(news);
//            }
//            else
//            {
//                // TODO
//                //_mapper.Map(request.Request, classPackage);

//                news.Description = request.Request.Description;
//                news.ContentHTML = request.Request.ContentHTML;
//                news.Name = request.Request.Name;
//                news.Status = request.Request.Status;
//            }

//            var files = request.Request.Files;
//            if (files.Any())
//            {
//                var fileName = await _fileService.SaveFile(files[0], news.NewUD, "New");
//                news.ImageURL = fileName;
//            }
//            await _context.SaveChangesAsync(cancellationToken);

//            return new DataResponse<SliderEntity> { Data = news };
//        }
//    }
//}