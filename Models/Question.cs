using System.ComponentModel;

namespace SimpleQuizApp.Models
{
    public class Question
    {
        public required Guid Id { get; init; }
        public required string QuestionText { get; init; }
        public required List<Answer> Answers { get; set; }
        public required Guid CorrectAnswer { get; init; }

        public virtual List<QuizQuestions>? QuizQuestions { get; init; }

    }
}
