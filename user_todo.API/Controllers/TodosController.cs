using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using user_todo.Domain.Entities.Model;
using user_todo.Infrastructure.Services;

namespace user_todo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly ITaskService _service;

        public TodosController(ITaskService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
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
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserTodoModel model)
        {
            if (id != model.Id)
                return BadRequest("Id mismatch");

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
    }
}
