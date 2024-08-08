//using Application.Common.Interfaces;
//using Application.Common.Models;
//using Microsoft.EntityFrameworkCore;
//using MediatR;
//using Newtonsoft.Json;
//using Application.News.Model;
//using Application.Common.Responses;

//namespace Application.Branches.Queries.GetNew;

//public class GetImagePagesQuery : IRequest<DataResponse<SliderEntity>>
//{
//    public string Json { get; }

//    public GetImagePagesQuery(string json)
//    {
//        Json = json;
//    }

//    public class Handler : IRequestHandler<GetImagePagesQuery, DataResponse<SliderEntity>>
//    {
//        private readonly IApplicationDbContext _context;

//        public Handler(IApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<DataResponse<SliderEntity>> Handle(GetImagePagesQuery request, CancellationToken cancellationToken)
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