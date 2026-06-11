using System.Text.Json.Serialization;

namespace Lab5.Models;

public enum QuestionType
{
    MultipleChoice,
    TrueFalse,
    FreeText
}

public abstract class Question
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    public Guid QuizId { get; init; }

    public string Text { get; set; } = "";
    public int OrderIndex { get; set; } 

    public abstract QuestionType Type { get; }

    [JsonIgnore] 
    public Quiz? Quiz { get; set; }
}

public class MultipleChoiceQuestion : Question
{
    public override QuestionType Type => QuestionType.MultipleChoice;
    
    public List<string> Options { get; set; } = new();
    public int CorrectOptionIndex { get; set; }
}

public class TrueFalseQuestion : Question
{
    public override QuestionType Type => QuestionType.TrueFalse;
    
    public bool CorrectAnswer { get; set; }
}

public class FreeTextQuestion : Question
{
    public override QuestionType Type => QuestionType.FreeText;
}
