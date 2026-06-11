using Lab5.Data;
using Lab5.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab5.Repositories;

public class EfQuestionRepository : IQuestionRepository
{
    private readonly QuizzesDbContext _db;

    public EfQuestionRepository(QuizzesDbContext db) => _db = db;

    public async Task<IEnumerable<Question>> AllForQuizAsync(Guid quizId) =>
        await _db.Questions
                 .Where(q => q.QuizId == quizId)
                 .OrderBy(q => q.OrderIndex)
                 .ThenBy(q => q.CreatedAt)
                 .ToListAsync();

    public async Task<Question?> FindAsync(Guid id) =>
        await _db.Questions.FindAsync(id);

    public async Task<Question> AddAsync(Question question)
    {
        _db.Questions.Add(question);
        await _db.SaveChangesAsync();
        return question;
    }

    public async Task<bool> UpdateAsync(Guid id, Question updatedQuestion)
    {
        var existing = await _db.Questions.FindAsync(id);
        if (existing is null) return false;

        existing.Text = updatedQuestion.Text;
        existing.OrderIndex = updatedQuestion.OrderIndex;

        if (existing is MultipleChoiceQuestion existingMc && updatedQuestion is MultipleChoiceQuestion updatedMc)
        {
            existingMc.Options = updatedMc.Options;
            existingMc.CorrectOptionIndex = updatedMc.CorrectOptionIndex;
        }
        else if (existing is TrueFalseQuestion existingTf && updatedQuestion is TrueFalseQuestion updatedTf)
        {
            existingTf.CorrectAnswer = updatedTf.CorrectAnswer;
        }

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveAsync(Guid id)
    {
        var existing = await _db.Questions.FindAsync(id);
        if (existing is null) return false;

        _db.Questions.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }
}
