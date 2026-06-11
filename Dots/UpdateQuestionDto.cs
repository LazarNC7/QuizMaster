using System.ComponentModel.DataAnnotations;
using Lab5.Models;

namespace Lab5.Dtos;

public class UpdateQuestionDto : IValidatableObject
{
    [Required]
    [StringLength(1000, MinimumLength = 5)]
    public string Text { get; set; } = "";

    [Required]
    public QuestionType Type { get; set; }

    public List<string>? Options { get; set; }
    public int? CorrectMultipleChoiceIndex { get; set; }
    public bool? CorrectTrueFalseAnswer { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "OrderIndex must be ≥ 1.")]
    public int OrderIndex { get; set; } = 1;

    public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
    {
        var mirror = new CreateQuestionDto
        {
            Text                       = Text,
            Type                       = Type,
            Options                    = Options,
            CorrectMultipleChoiceIndex = CorrectMultipleChoiceIndex,
            CorrectTrueFalseAnswer     = CorrectTrueFalseAnswer,
            OrderIndex                 = OrderIndex
        };
        return mirror.Validate(ctx);
    }
}