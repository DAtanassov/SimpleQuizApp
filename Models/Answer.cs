using System.ComponentModel;

namespace SimpleQuizApp.Models
{
    public class Answer
    {
        public required Guid Id { get; init; }

        public required string AnswerText { get; init; }

        public required Guid QuestionId { get; init; }

    }
}