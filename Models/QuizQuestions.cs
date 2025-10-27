namespace SimpleQuizApp.Models
{
    public class QuizQuestions
    {
        public required Guid Id { get; init; }
        public required Guid QuizId { get; init; }
        public required Guid QuestionId { get; init; }

        public virtual Quiz? Quiz { get; set; }
        public virtual Question? Question { get; set; }

    }
}
