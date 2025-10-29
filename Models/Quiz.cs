namespace SimpleQuizApp.Models
{
    /// <summary>
    /// Represents a quiz entity
    /// </summary>
    public class Quiz
    {
        /// <summary>
        /// The unique identifier of the quiz
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// The name of the quiz
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Navigation property representing the relationship between the quiz
        /// and its associated questions
        /// </summary>
        public virtual List<QuizQuestions>? QuizQuestions { get; init; }

    }
}
