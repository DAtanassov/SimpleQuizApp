namespace SimpleQuizApp.Models.ViewModels
{
    public class SelectQuizViewModel
    {
        public required List<QuizViewModelItem> Quizzes { get; set; } = [];
    }

    public class QuizViewModelItem
    {
        public required Guid Id { get; init; }
        public bool Selected { get; set; }
        public required string QuizName { get; init; }
    }
}
