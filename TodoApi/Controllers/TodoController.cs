using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _service;

        public TodoController(ITodoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodo([FromBody] Todo todo)
        {
            if (todo == null) return BadRequest();

            var created = await _service.CreateTodoAsync(todo);
            return CreatedAtAction(nameof(GetTodoById), new { id = created.Id }, created);
        }

        [HttpGet]
        public async Task<ActionResult<List<Todo>>> GetAllTodos()
        {
            var todos = await _service.GetAllTodosAsync();
            return Ok(todos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Todo>> GetTodoById(int id)
        {
            var todo = await _service.GetTodoByIdAsync(id);
            if (todo == null) return NotFound();
            return Ok(todo);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Todo>> UpdateTodo(int id, [FromBody] Todo todo)
        {
            if (todo == null) return BadRequest();

            var existing = await _service.GetTodoByIdAsync(id);
            if (existing == null) return NotFound();

            var updated = await _service.UpdateTodoAsync(id, todo);
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var deleted = await _service.DeleteTodoAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}