namespace SimpleQuizApp.Models.ViewModels
{
    public class QuizViewModel
    {
        public required Guid QuizId { get; init; }
        public required List<QuestionViewModel> Questions { get; set; } = [];
    }

    public class QuestionViewModel
    {
        public required Guid Id { get; init; }
        public required string QuestionText { get; init; }
        public required List<QuestionViewModelAnswer> Answers { get; set; }

    }

    public class QuestionViewModelAnswer
    {
        public required Guid Id { get; init; }
        public required Guid QuestionViewModelId { get; init; }
        public required string Text { get; init; }
        public AnswerOption? Option { get; set; }

    }
    public enum AnswerOption
    {
        A,
        B,
        C,
        D
    }
}

