using Microsoft.EntityFrameworkCore;
using SimpleQuizApp.Models;

namespace SimpleQuizApp.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuizQuestions> QuizQuestions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<QuizQuestions>()
                .HasOne(q => q.Quiz)
                .WithMany(qq => qq.QuizQuestions)
                .HasForeignKey(q => q.QuizId);

            modelBuilder.Entity<QuizQuestions>()
                .HasOne(q => q.Question)
                .WithMany(qq => qq.QuizQuestions)
                .HasForeignKey(q => q.QuestionId);
        }

        public void RandomizeAnswers<T>(List<T> list, Random rand)
        {
            int n = list.Count;
            while (n > 1)
            {
                int k = rand.Next(n--);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public void ImportQuizzes(List<ImportQuizModel> list)
        {
            List<Quiz> quizzes = new List<Quiz>();
            List<Question> questions = new List<Question>();
            List<QuizQuestions> quizQuestions = new List<QuizQuestions>();

            foreach (var quiz in list)
            {
                Quiz newQuiz = new Quiz() { Id = Guid.NewGuid(), Name = quiz.name };

                List<Question> newQuestions = GetImportQuestions(quiz.questions);
                foreach (Question newQuestion in newQuestions)
                {
                    questions.Add(newQuestion);
                    quizQuestions.Add(new QuizQuestions() { Id = Guid.NewGuid(), QuizId = newQuiz.Id, QuestionId = newQuestion.Id });
                }
                quizzes.Add(newQuiz);
            }
            Quizzes.AddRangeAsync(quizzes);
            Questions.AddRangeAsync(questions);
            QuizQuestions.AddRangeAsync(quizQuestions);

            SaveChangesAsync();
        }

        public List<Question> GetImportQuestions(List<ImportQuestionModel> importQuestions)
        {
            List<Question> questions = new List<Question>();

            foreach (var question in importQuestions)
            {
                Guid qId = Guid.NewGuid();

                Answer answerA = new Answer() { Id = Guid.NewGuid(), QuestionId = qId, AnswerText = question.answers.A };
                Answer answerB = new Answer() { Id = Guid.NewGuid(), QuestionId = qId, AnswerText = question.answers.B };
                Answer answerC = new Answer() { Id = Guid.NewGuid(), QuestionId = qId, AnswerText = question.answers.C };
                Answer answerD = new Answer() { Id = Guid.NewGuid(), QuestionId = qId, AnswerText = question.answers.D };

                Guid CorrectAnswer = Guid.Empty;
                switch (question.correctAnswer)
                {
                    case "A":
                        CorrectAnswer = answerA.Id;
                        break;
                    case "B":
                        CorrectAnswer = answerB.Id;
                        break;
                    case "C":
                        CorrectAnswer = answerC.Id;
                        break;
                    case "D":
                        CorrectAnswer = answerD.Id;
                        break;
                }

                Question newQuestion = new Question()
                {
                    Id = Guid.NewGuid(),
                    QuestionText = question.questionText,
                    Answers = [answerA, answerB, answerC, answerD],
                    CorrectAnswer = CorrectAnswer
                };

                questions.Add(newQuestion);
            }

            return questions;
        }
    }
}
