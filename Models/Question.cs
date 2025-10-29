namespace SimpleQuizApp.Models
{
    /// <summary>
    /// Represents a question entity
    /// </summary>
    public class Question
    {
        /// <summary>
        /// The unique identifier of the question
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// The text of the question that will be displayed to the user
        /// </summary>
        public required string QuestionText { get; init; }
        /// <summary>
        /// The list of possible answers for this question
        /// </summary>
        public required List<Answer> Answers { get; set; }
        /// <summary>
        /// The identifier of the correct answer from the <see cref="Answers"/> list
        /// </summary>
        public required Guid CorrectAnswer { get; init; }

        /// <summary>
        /// Navigation property representing the relationship between the question
        /// and the quizzes it is part of
        /// </summary>
        public virtual List<QuizQuestions>? QuizQuestions { get; init; }

    }
}
