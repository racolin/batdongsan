using Application.Common.Interfaces;
using MediatR;
using Application.Common.Responses.Client;
using Application.Common.Responses;

namespace Application.Pages.Queries;

public class GetQuestionQuery : IRequest<DataResponse<Question>>
{
    public GetQuestionQuery() {}

    public class Handler : IRequestHandler<GetQuestionQuery, DataResponse<Question>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataResponse<Question>> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
        {

            return DataResponse<Question>.Success(new Question { 
                Id = "12435552", 
                Title = "Sắp xếp các chữ sau thành tên một con vật (N/Â/T/O/C/U/R)",
                Hint = "Con vật này màu đen, có sừng.",
            });
        }
    }
}