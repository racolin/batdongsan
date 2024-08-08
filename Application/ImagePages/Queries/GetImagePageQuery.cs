//using Application.Common.Interfaces;
//using Microsoft.EntityFrameworkCore;
//using MediatR;
//using Application.Common.Responses;

//namespace Application.ImagePages.Queries;

//public class GetImagePageQuery : IRequest<DataResponse<ImagePage>>
//{
//    public string Json { get; }

//    public GetImagePageQuery(string json)
//    {
//        Json = json;
//    }

//    public class Handler : IRequestHandler<GetImagePageQuery, DataResponse<SliderEntity>>
//    {
//        private readonly IApplicationDbContext _context;

//        public Handler(IApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<DataResponse<SliderEntity>> Handle(GetImagePageQuery request, CancellationToken cancellationToken)
//        {
//            //string json = string.Empty;
//            SearchForm param = JsonConvert.DeserializeObject<SearchForm>(request.Json.ToString());
//            int totalRows = await _context.News.CountAsync();
//            var value = _context.News
//                .AsNoTracking()
//                .Select(x => new { 
//                    x.Id,
//                    x.NewUD,
//                    x.ImageURL,
//                    x.Name,
//                    x.Description,
//                    x.Status,
//                    x.UpdatedDate,
//                    x.CreatedDate
//                })
//                .OrderByDescending(x => x.CreatedDate)
//                .Skip(param.Start).Take(param.Length)
//                .ToList();
//            return new DataResponse<SliderEntity>() { Data = value, TotalRows = value.Count, RecordsTotal = totalRows, RecordsFiltered = totalRows };

//        }
//    }
//}