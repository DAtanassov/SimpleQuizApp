namespace SimpleQuizApp.Models
{
    /// <summary>
    /// Represents an answer entity
    /// </summary>
    public class Answer
    {
        /// <summary>
        /// The unique identifier of the answer
        /// </summary>
        public required Guid Id { get; init; }

        /// <summary>
        /// The text value of the answer
        /// </summary>
        public required string AnswerText { get; init; }

        /// <summary>
        /// The identifier of the question to which this answer belongs
        /// </summary>
        public required Guid QuestionId { get; init; }

    }
}