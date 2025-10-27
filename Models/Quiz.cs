namespace SimpleQuizApp.Models
{
    public class Quiz
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }

        public virtual List<QuizQuestions>? QuizQuestions { get; init; }

    }
}
