using System.Text.Json;
using SimpleQuizApp.Models;

namespace SimpleQuizApp.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var _context = services.GetService<AppDbContext>()!;
                var env = services.GetRequiredService<IWebHostEnvironment>();

                _context.Database.EnsureCreated();

                if (!_context.Questions.Any())
                {
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

                            List<QuizQuestions> quiz_questions = new List<QuizQuestions>();
                            Random rand = new Random();
                            int n = questions.Count();
                            for (int i = 0; i < quizzes.Count; i++)
                            {
                                _context.RandomizeAnswers(questions, rand);

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

                            _context.Questions.AddRange(questions);
                            _context.Quizzes.AddRange(quizzes);
                            _context.QuizQuestions.AddRange(quiz_questions);

                            _context.SaveChanges();
                        }
                    }
                }
            }
        }


    }
}
