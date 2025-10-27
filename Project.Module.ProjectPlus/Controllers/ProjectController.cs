using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.Module.ProjectPlus.Commands;
using Project.Module.ProjectPlus.Queries;

namespace Project.Module.ProjectPlus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command)
        {
            var response = await _mediator.Send(command);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects([FromQuery] GetProjectQuery query)
        {
            var response = await _mediator.Send(query);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var query = new GetProjectQuery { Id = id };
            var response = await _mediator.Send(query);
            return StatusCode(response.StatusCode, response);
        }
    }
} 