namespace SimpleQuizApp.Models.ViewModels
{
    /// <summary>
    /// View model representing a quiz that will be displayed to the user
    /// </summary>
    public class QuizViewModel
    {
        /// <summary>
        /// The unique identifier of the quiz
        /// </summary>
        public required Guid QuizId { get; init; }
        /// <summary>
        /// The list of questions included in the quiz
        /// </summary>
        public required List<QuestionViewModel> Questions { get; set; } = [];
    }

    /// <summary>
    /// View model representing a single question within a quiz
    /// </summary>
    public class QuestionViewModel
    {
        /// <summary>
        /// The unique identifier of the question
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// The text of the question
        /// </summary>
        public required string QuestionText { get; init; }
        /// <summary>
        /// The list of possible answers for this question
        /// </summary>
        public required List<QuestionViewModelAnswer> Answers { get; set; }

    }

    /// <summary>
    /// View model represents a single answer option within a question
    /// </summary>
    public class QuestionViewModelAnswer
    {
        /// <summary>
        /// The unique identifier of the answer
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// The identifier of the parent question to which this answer belongs
        /// </summary>
        public required Guid QuestionViewModelId { get; init; }
        /// <summary>
        /// The text displayed for this answer
        /// </summary>
        public required string Text { get; init; }
        /// <summary>
        /// The answer option (A, B, C, or D).
        /// </summary>
        public AnswerOption? Option { get; set; }

    }

    /// <summary>
    /// Represents the possible labeled options for quiz answers
    /// </summary>
    public enum AnswerOption
    {
        /// <summary>Answer option A.</summary>
        A,
        /// <summary>Answer option B.</summary>
        B,
        /// <summary>Answer option C.</summary>
        C,
        /// <summary>Answer option D.</summary>
        D
    }
}

