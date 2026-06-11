using System.ComponentModel.DataAnnotations;
using Lab5.Models;

namespace Lab5.Dtos;

public class CreateQuestionDto : IValidatableObject
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
        switch (Type)
        {
            case QuestionType.MultipleChoice:
                if (Options is null || Options.Count < 2)
                    yield return new ValidationResult(
                        "MultipleChoice questions require at least two options.",
                        [nameof(Options)]);

                if (CorrectMultipleChoiceIndex is null)
                    yield return new ValidationResult(
                        "MultipleChoice questions require a CorrectMultipleChoiceIndex.",
                        [nameof(CorrectMultipleChoiceIndex)]);
                else if (Options is not null &&
                         (CorrectMultipleChoiceIndex < 0 || CorrectMultipleChoiceIndex >= Options.Count))
                    yield return new ValidationResult(
                        $"CorrectMultipleChoiceIndex must be between 0 and {Options.Count - 1}.",
                        [nameof(CorrectMultipleChoiceIndex)]);

                if (CorrectTrueFalseAnswer is not null)
                    yield return new ValidationResult(
                        "MultipleChoice questions must not include a CorrectTrueFalseAnswer value.",
                        [nameof(CorrectTrueFalseAnswer)]);
                break;

            case QuestionType.TrueFalse:
                if (CorrectTrueFalseAnswer is null)
                    yield return new ValidationResult(
                        "TrueFalse questions require a CorrectTrueFalseAnswer (true or false).",
                        [nameof(CorrectTrueFalseAnswer)]);

                if (Options is not null && Options.Count > 0)
                    yield return new ValidationResult(
                        "TrueFalse questions must not include Options.",
                        [nameof(Options)]);

                if (CorrectMultipleChoiceIndex is not null)
                    yield return new ValidationResult(
                        "TrueFalse questions must not include a CorrectMultipleChoiceIndex.",
                        [nameof(CorrectMultipleChoiceIndex)]);
                break;

            case QuestionType.FreeText:
                if (CorrectMultipleChoiceIndex is not null || CorrectTrueFalseAnswer is not null)
                    yield return new ValidationResult(
                        "FreeText questions must not include any correct answer properties.",
                        [nameof(CorrectMultipleChoiceIndex), nameof(CorrectTrueFalseAnswer)]);

                if (Options is not null && Options.Count > 0)
                    yield return new ValidationResult(
                        "FreeText questions must not include Options.",
                        [nameof(Options)]);
                break;
        }
    }
}
