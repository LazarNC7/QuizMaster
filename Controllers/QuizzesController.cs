using Lab5.Dtos;
using Lab5.Models;
using Lab5.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Lab5.Controllers;

[ApiController]
[Route("quizzes")]
public class QuizzesController : ControllerBase
{
    private readonly IQuizRepository _repo;

    public QuizzesController(IQuizRepository repo) => _repo = repo;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Quiz>>> List() => 
        Ok(await _repo.AllAsync());

    [HttpGet("{id:guid}", Name = nameof(GetById))]
    public async Task<ActionResult<Quiz>> GetById(Guid id)
    {
        var quiz = await _repo.FindAsync(id);
        return quiz is null ? NotFound() : Ok(quiz);
    }

    [HttpPost]
    public async Task<ActionResult<Quiz>> Create(CreateQuizDto dto)
    {
        var quiz = await _repo.AddAsync(dto.Title, dto.Description);
        return CreatedAtAction(nameof(GetById), new { id = quiz.Id }, quiz);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Replace(Guid id, UpdateQuizDto dto)
    {
        var updated = await _repo.UpdateAsync(id, dto.Title, dto.Description);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        await _repo.RemoveAsync(id) ? NoContent() : NotFound();
}
