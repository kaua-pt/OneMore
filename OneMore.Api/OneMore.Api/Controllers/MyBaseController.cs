using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OneMore.Api.Controllers;
public abstract class MyControllerBase(ILogger logger, IConfiguration configuration, IMediator mediator) : ControllerBase
{
    protected readonly ILogger _logger = logger;
    protected readonly IConfiguration _configuration = configuration;
    protected readonly IMediator _mediator = mediator;
}