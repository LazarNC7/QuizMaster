using Lab5.Models;

namespace Lab5.Repositories;

public interface IQuizRepository
{
    Task<IEnumerable<Quiz>> AllAsync();
    Task<Quiz?> FindAsync(Guid id);
    Task<Quiz> AddAsync(string title, string? description);
    Task<bool> UpdateAsync(Guid id, string title, string? description);
    Task<bool> RemoveAsync(Guid id);
}