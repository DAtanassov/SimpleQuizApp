using System.Text.Json;
using SimpleQuizApp.Models;
using SimpleQuizApp.Services;

namespace SimpleQuizApp.Data
{
    /// <summary>
    /// Provides database seeding functionality
    /// This class initializes the database with default quizzes and questions
    /// during application startup if no data currently exists.
    /// </summary>
    public class AppDbInitializer
    {
        /// <summary>
        /// Seeds the application database with initial data if it is empty.
        /// </summary>
        /// <param name="applicationBuilder">
        /// The <see cref="IApplicationBuilder"/> used to access application services
        /// </param>
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var _context = services.GetService<MemoryStore>()!;
                var env = services.GetRequiredService<IWebHostEnvironment>();

                // Only seed data if no questions exist
                if (!_context.Questions.Any())
                {
                    // Path to the initial JSON data file
                    var filePath = Path.Combine(env.ContentRootPath, "JSONfiles", "InitialQuestions.json");

                    if (File.Exists(filePath))
                    {
                        string _questions = File.ReadAllText(filePath);
                        
                        JsonSerializerOptions options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        List<Question> questions = new List<Question>();
                        List<ImportQuestionModel>? newQuestions = JsonSerializer.Deserialize<List<ImportQuestionModel>>(_questions, options);

                        if (newQuestions != null && newQuestions.Count > 0)
                        {
                            questions = _context.GetImportQuestions(newQuestions);
                        }

                        if (questions.Count > 0)
                        {
                            // Create several sample quizzes
                            List<Quiz> quizzes = new List<Quiz>();
                            quizzes.Add(new Quiz()
                            {
                                Id = Guid.NewGuid(),
                                Name = "Случайно генериран тест 1"
                            });
                            quizzes.Add(new Quiz()
                            {
                                Id = Guid.NewGuid(),
                                Name = "Случайно генериран тест 2"
                            });
                            quizzes.Add(new Quiz()
                            {
                                Id = Guid.NewGuid(),
                                Name = "Случайно генериран тест 3"
                            });

                            // Create relationships between quizzes and random questions
                            List<QuizQuestions> quiz_questions = new List<QuizQuestions>();
                            Random rand = new Random();
                            int n = questions.Count();
                            
                            for (int i = 0; i < quizzes.Count; i++)
                            {
                                _context.RandomizeAnswers(questions, rand);

                                // Select a random number of questions per quiz (between 5 and 10)
                                int r = rand.Next(Math.Min(5, n), Math.Min(10, n));
                                var quiz = quizzes[i];

                                for (int j = 0; j < r; j++)
                                {
                                    quiz_questions.Add(new QuizQuestions()
                                    {
                                        Id = Guid.NewGuid(),
                                        QuizId = quiz.Id,
                                        QuestionId = questions[j].Id
                                    });
                                }
                            }

                            // Save all seeded entities to the in-memory data store
                            foreach (var quiz in quizzes)
                                _context.Quizzes.Add(quiz);

                            foreach (var question in questions)
                            {
                                _context.Questions.Add(question);
                                foreach (var answer in question.Answers)
                                    _context.Answers.Add(answer);
                            }

                            foreach (var qq in quiz_questions)
                                _context.QuizQuestions.Add(qq);
                        }
                    }
                }
            }
        }
    }
}
