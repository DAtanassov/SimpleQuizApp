namespace SimpleQuizApp.Models
{
    /// <summary>
    /// Represents the relationship between a quiz and its questions (many-to-many join entity)
    /// </summary>
    public class QuizQuestions
    {
        /// <summary>
        /// The unique identifier for this quiz-question relation
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// The identifier of the associated quiz
        /// </summary>
        public required Guid QuizId { get; init; }
        /// <summary>
        /// The identifier of the associated question
        /// </summary>
        public required Guid QuestionId { get; init; }

        /// <summary>
        /// Navigation property for the related quiz
        /// </summary>
        public virtual Quiz? Quiz { get; set; }
        /// <summary>
        /// Navigation property for the related question
        /// </summary>
        public virtual Question? Question { get; set; }

    }
}
