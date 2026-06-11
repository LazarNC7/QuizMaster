using Lab5.Data;
using Lab5.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab5.Repositories;

public class EfQuizRepository : IQuizRepository
{
    private readonly QuizzesDbContext _db;

    public EfQuizRepository(QuizzesDbContext db) => _db = db;

    public async Task<IEnumerable<Quiz>> AllAsync() =>
        await _db.Quizzes
                 .Include(q => q.Questions)
                 .OrderBy(q => q.CreatedAt)
                 .ToListAsync();

    public async Task<Quiz?> FindAsync(Guid id) =>
        await _db.Quizzes
                 .Include(q => q.Questions)
                 .FirstOrDefaultAsync(q => q.Id == id);

    public async Task<Quiz> AddAsync(string title, string? description)
    {
        var quiz = new Quiz { Title = title, Description = description };
        _db.Quizzes.Add(quiz);
        await _db.SaveChangesAsync();
        return quiz;
    }

    public async Task<bool> UpdateAsync(Guid id, string title, string? description)
    {
        var existing = await _db.Quizzes.FindAsync(id);
        if (existing is null) return false;

        existing.Title = title;
        existing.Description = description;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveAsync(Guid id)
    {
        var existing = await _db.Quizzes.FindAsync(id);
        if (existing is null) return false;

        _db.Quizzes.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }
}