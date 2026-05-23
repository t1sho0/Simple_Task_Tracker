using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Api.Data;
using TaskTracker.Api.Models;
using TaskTracker.Api.Dtos;

namespace TaskTracker.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TasksController(ApplicationDbContext context) => _context = context;


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var taskFromDb = await _context.Tasks.ToListAsync();
            return Ok(taskFromDb);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound($"Task with id={id}, Not Found");
            }

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            var newTask = new TaskItem 
            { 
                Title = dto.Title,
                IsDone = false
            };

            await _context.Tasks.AddAsync(newTask);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new {id = newTask.Id }, newTask);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTaskStatus(int id)
        {
            var taskNow = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (taskNow == null) 
            {
                return NotFound($"Task with id={id}, not found");
            }

            taskNow.IsDone = !taskNow.IsDone;

            await _context.SaveChangesAsync();
            return Ok("Status changed successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto dto)
        {
            var taskForUpdate = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (taskForUpdate == null)
            {
                return NotFound($"Task with id={id}, Not Found");
            }

            taskForUpdate.Title = dto.Title;
            taskForUpdate.IsDone = dto.IsDone;
            await _context.SaveChangesAsync();

            return Ok($"Task with id={id} updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound($"Task with id={id}, not found in database!");
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent(); 
        }

        [HttpDelete("permanently/{id}")]
        public async Task<IActionResult> DeletePermanently(int id)
        {
            int rowsAffected = await _context.Tasks.Where(t => t.Id == id).ExecuteDeleteAsync();

            if (rowsAffected == 0)
            {
                return NotFound($"Task with id={id} not found");
            }

            return NoContent();
        }
    }
}
