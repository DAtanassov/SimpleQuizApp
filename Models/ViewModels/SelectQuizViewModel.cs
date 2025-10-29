namespace SimpleQuizApp.Models.ViewModels
{
    /// <summary>
    /// View model used to represent a list of available quizzes
    /// from which the user can select one to take
    /// </summary>
    public class SelectQuizViewModel
    {
        /// <summary>
        /// The list of available quizzes for selection
        /// </summary>
        public required List<QuizViewModelItem> Quizzes { get; set; } = [];
    }

    /// <summary>
    /// View model represents a single quiz item in the quiz selection view
    /// </summary>
    public class QuizViewModelItem
    {
        /// <summary>
        /// The unique identifier of the quiz
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// A value indicating whether this quiz is currently selected
        /// </summary>
        public bool Selected { get; set; }
        /// <summary>
        /// The display name of the quiz
        /// </summary>
        public required string QuizName { get; init; }
    }
}
