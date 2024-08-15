using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Filters;

namespace WebUI.Areas.Admin.Controllers.Apis;

[ApiController]
[ApiExceptionFilter]
[Route("api/admin/[controller]")]
public abstract class ApiAdminControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}