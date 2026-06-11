using Lab5.Models;

namespace Lab5.Repositories;

public interface IQuestionRepository
{
    Task<IEnumerable<Question>> AllForQuizAsync(Guid quizId);
    Task<Question?> FindAsync(Guid id);
    Task<Question> AddAsync(Question question);
    Task<bool> UpdateAsync(Guid id, Question updatedQuestion);
    Task<bool> RemoveAsync(Guid id);
}