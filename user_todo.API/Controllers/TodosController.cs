using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using user_todo.Domain.Entities.Model;
using user_todo.Infrastructure.Repositories;
using user_todo.Infrastructure.Services;

namespace user_todo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly ITaskService _service;

        private readonly ITaskRepo _repo;
        public TodosController(ITaskService service, ITaskRepo repo)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserTodoModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.createTask(model);

            return Ok(created);
        }

        // UPDATE
        [HttpPut]
        public async Task<IActionResult> Update(UserTodoModel model)
        {
            var ok = await _service.updateTask(model);

            if (!ok)
                return NotFound();

            return NoContent();
        }

        // DELETE
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.deleteTask(id);

            if (!ok)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _repo.GetByIdAsync(id);

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null, [FromQuery] string? category = null, [FromQuery] string? priority = null)
        {
            var (items, total) = await _repo.GetAllAsync(pageNumber, pageSize, search, category, priority);

            var response = new
            {
                Items = items,
                Total = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Ok(response);
        }

        [HttpGet("simple")]
        public async Task<IActionResult> GetAllSimple()
        {
            var items = await _repo.GetAllSimpleAsync();
            return Ok(items);
        }
    }
}
