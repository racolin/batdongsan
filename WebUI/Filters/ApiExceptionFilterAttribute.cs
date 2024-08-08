using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Application.Common.Exceptions;
using Application.Common.Responses;

namespace WebUI.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        // Register known exception types and handlers.
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
                { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
            };
    }

    public override void OnException(ExceptionContext context)
    {

        HandleException(context);

        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }

        var exception = context.Exception;
        context.Result = new BadRequestObjectResult(DataResponse<string>.Error("Bạn không thể thực hiện thao tác này!", [exception.Message]));
    }

    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        //var details = new ValidationProblemDetails(context.ModelState)
        //{
        //    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        //};

        context.Result = new BadRequestObjectResult(
            DataResponse<string>.Error(
                "Đã xảy ra một hoặc nhiều lỗi xác thực!",
                ["https://tools.ietf.org/html/rfc7231#section-6.5.1"]
            )
        );

        context.ExceptionHandled = true;
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;

        //var details = new ValidationProblemDetails(exception.Errors)
        //{
        //    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        //};

        context.Result = new BadRequestObjectResult(
            DataResponse<string>.Error(
                "Đã xảy ra một hoặc nhiều lỗi xác thực!",
                ["https://tools.ietf.org/html/rfc7231#section-6.5.1"]
            )
        );

        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException)context.Exception;

        //var details = new ProblemDetails()
        //{
        //    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        //    Title = "The specified resource was not found.",
        //    Detail = exception.Message
        //};

        //context.Result = new NotFoundObjectResult(details);

        context.Result = new NotFoundObjectResult(
            DataResponse<string>.Error(
                "Không thể tìm thấy dữ liệu!",
                ["https://tools.ietf.org/html/rfc7231#section-6.5.4"]
            )
        );

        context.ExceptionHandled = true;
    }

    private void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        //var details = new ProblemDetails
        //{
        //    Status = StatusCodes.Status401Unauthorized,
        //    Title = "Unauthorized",
        //    Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
        //};

        //context.Result = new ObjectResult(details)
        //{
        //    StatusCode = StatusCodes.Status401Unauthorized
        //};

        context.Result = new UnauthorizedObjectResult(
            DataResponse<string>.Error(
                "Có lỗi liên quan đến xác thực, hãy thử lại!",
                ["https://tools.ietf.org/html/rfc7235#section-3.1"]
            )
        );

        context.ExceptionHandled = true;
    }

    private void HandleForbiddenAccessException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Forbidden",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status403Forbidden
        };

        context.Result = new ObjectResult(DataResponse<string>
            .Error(
                "Quyền truy cập bị cấm!", 
                ["https://tools.ietf.org/html/rfc7231#section-6.5.3"]
            )
        ) {
            StatusCode = StatusCodes.Status403Forbidden,
        };

        context.ExceptionHandled = true;
    }
}
