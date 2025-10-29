using Microsoft.EntityFrameworkCore;
using SimpleQuizApp.Models;

namespace SimpleQuizApp.Data
{
    /// <summary>
    /// Represents the main Entity Framework Core database context
    /// </summary>
    /// <param name="options">The options used to configure the database context.</param>
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        /// <summary>
        /// Collection of quizzes stored in the database.
        /// </summary>
        public DbSet<Quiz> Quizzes { get; set; }
        /// <summary>
        /// Collection of questions stored in the database.
        /// </summary>
        public DbSet<Question> Questions { get; set; }
        /// <summary>
        /// Collection of answers stored in the database.
        /// </summary>
        public DbSet<Answer> Answers { get; set; }
        /// <summary>
        /// Collection representing the many-to-many relationship
        /// between quizzes and questions.
        /// </summary>
        public DbSet<QuizQuestions> QuizQuestions { get; set; }

        /// <summary>
        /// Configures the model relationships and database schema when the model is created.
        /// </summary>
        /// <param name="modelBuilder">Provides a simple API for configuring EF Core models.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<QuizQuestions>()
                .HasOne(q => q.Quiz)
                .WithMany(qq => qq.QuizQuestions)
                .HasForeignKey(q => q.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuizQuestions>()
                .HasOne(q => q.Question)
                .WithMany(qq => qq.QuizQuestions)
                .HasForeignKey(q => q.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        /// <summary>
        /// Randomizes the order of elements in a given list using the Fisher–Yates shuffle algorithm.
        /// </summary>
        /// <typeparam name="T">Type of elements contained in the list.</typeparam>
        /// <param name="list">The list to be randomized.</param>
        /// <param name="rand">An instance of <see cref="Random"/> used for generating random numbers.</param>
        public void RandomizeAnswers<T>(List<T> list, Random rand)
        {
            int n = list.Count;
            while (n > 1)
            {
                int k = rand.Next(n--);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        /// <summary>
        /// Imports quizzes and related data (questions and answers) from a collection of <see cref="ImportQuizModel"/> objects.
        /// This method creates new quiz, question, and quiz-question records and saves them to the database.
        /// </summary>
        /// <param name="list">List of quizzes to be imported from JSON or another source.</param>
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

        /// <summary>
        /// Converts a list of <see cref="ImportQuestionModel"/> objects into a list of <see cref="Question"/> entities,
        /// creating new associated <see cref="Answer"/> objects and setting the correct answer reference.
        /// </summary>
        /// <param name="importQuestions">List of imported question models to be converted.</param>
        /// <returns>
        /// A list of <see cref="Question"/> entities with associated answers and correct answer references.
        /// </returns>

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
