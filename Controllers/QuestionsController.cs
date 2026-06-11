using Lab5.Dtos;
using Lab5.Models;
using Lab5.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Lab5.Controllers;

[ApiController]
[Route("quizzes/{quizId:guid}/questions")]
public class QuestionsController : ControllerBase
{
    private readonly IQuizRepository     _quizRepo;
    private readonly IQuestionRepository _questionRepo;

    public QuestionsController(
        IQuizRepository     quizRepo,
        IQuestionRepository questionRepo)
    {
        _quizRepo     = quizRepo;
        _questionRepo = questionRepo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Question>>> List(Guid quizId)
    {
        if (await _quizRepo.FindAsync(quizId) is null) return NotFound();
        return Ok(await _questionRepo.AllForQuizAsync(quizId));
    }

    [HttpGet("{id:guid}", Name = nameof(GetQuestionById))]
    public async Task<ActionResult<Question>> GetQuestionById(Guid quizId, Guid id)
    {
        if (await _quizRepo.FindAsync(quizId) is null) return NotFound();

        var question = await _questionRepo.FindAsync(id);
        if (question is null || question.QuizId != quizId) return NotFound();

        return Ok(question);
    }

    [HttpPost]
    public async Task<ActionResult<Question>> Create(Guid quizId, CreateQuestionDto dto)
    {
        if (await _quizRepo.FindAsync(quizId) is null) return NotFound();

        Question question = dto.Type switch
        {
            QuestionType.MultipleChoice => new MultipleChoiceQuestion
            {
                QuizId = quizId,
                Text = dto.Text,
                OrderIndex = dto.OrderIndex,
                Options = dto.Options ?? new(),
                CorrectOptionIndex = dto.CorrectMultipleChoiceIndex!.Value
            },
            QuestionType.TrueFalse => new TrueFalseQuestion
            {
                QuizId = quizId,
                Text = dto.Text,
                OrderIndex = dto.OrderIndex,
                CorrectAnswer = dto.CorrectTrueFalseAnswer!.Value
            },
            QuestionType.FreeText => new FreeTextQuestion
            {
                QuizId = quizId,
                Text = dto.Text,
                OrderIndex = dto.OrderIndex
            },
            _ => throw new ArgumentOutOfRangeException()
        };

        await _questionRepo.AddAsync(question);

        return CreatedAtAction(
            nameof(GetQuestionById),
            new { quizId, id = question.Id },
            question);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Replace(Guid quizId, Guid id, UpdateQuestionDto dto)
    {
        if (await _quizRepo.FindAsync(quizId) is null) return NotFound();

        var existing = await _questionRepo.FindAsync(id);
        if (existing is null || existing.QuizId != quizId) return NotFound();

        Question updatedQuestion = dto.Type switch
        {
            QuestionType.MultipleChoice => new MultipleChoiceQuestion
            {
                Text = dto.Text,
                OrderIndex = dto.OrderIndex,
                Options = dto.Options ?? new(),
                CorrectOptionIndex = dto.CorrectMultipleChoiceIndex ?? 0 
            },
            QuestionType.TrueFalse => new TrueFalseQuestion
            {
                Text = dto.Text,
                OrderIndex = dto.OrderIndex,
                CorrectAnswer = dto.CorrectTrueFalseAnswer ?? false 
            },
            QuestionType.FreeText => new FreeTextQuestion
            {
                Text = dto.Text,
                OrderIndex = dto.OrderIndex,
            },
            _ => throw new ArgumentOutOfRangeException()
        };

        await _questionRepo.UpdateAsync(id, updatedQuestion);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid quizId, Guid id)
    {
        if (await _quizRepo.FindAsync(quizId) is null) return NotFound();

        var question = await _questionRepo.FindAsync(id);
        if (question is null || question.QuizId != quizId) return NotFound();

        await _questionRepo.RemoveAsync(id);
        return NoContent();
    }
}
