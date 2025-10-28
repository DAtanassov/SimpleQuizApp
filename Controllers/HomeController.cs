using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleQuizApp.Data;
using SimpleQuizApp.Models;
using SimpleQuizApp.Models.ViewModels;


namespace SimpleQuizApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IWebHostEnvironment webHost)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<QuizViewModelItem> quizzes = await GetQuizzes();
            return View(new SelectQuizViewModel { Quizzes = quizzes });
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            if (file == null || Path.GetExtension(file.FileName).ToLower() != ".json")
            {
                TempData["ErrorMessage"] = "Invalid file format. Please upload a JSON file.";
                return RedirectToAction(nameof(Index));
            }

            var fileName = Path.GetFileName(file.FileName);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<ImportQuizModel>? newQuizzes = new List<ImportQuizModel>();
            try
            {
                newQuizzes = await JsonSerializer.DeserializeAsync<List<ImportQuizModel>>(file.OpenReadStream(), options);
            }
            catch
            {
                TempData["ErrorMessage"] = "The structure of the json file is incorrect. Please upload a correct JSON file.";
                return RedirectToAction(nameof(Index));
            }

            if (newQuizzes != null && newQuizzes.Count > 0)
            {
                TempData["SuccessMessage"] = "File uploaded successfully!";
                _context.ImportQuizzes(newQuizzes);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult DownloadSampleJson()
        {
            List<ImportQuizModel> list = new List<ImportQuizModel>()
            { 
                new ImportQuizModel()
                {
                    name = "Sample Quiz",
                    questions = new List<ImportQuestionModel>()
                    {
                        new ImportQuestionModel()
                        {
                            questionText = "What is the capital of France?",
                            correctAnswer = "A",
                            answers = new ImportAnswerModel()
                            {
                                A = "Paris",
                                B = "London",
                                C = "Berlin",
                                D = "Moscow"
                            }
                        },
                        new ImportQuestionModel()
                        {
                            questionText = "Which planet is known as the Red Planet?",
                            correctAnswer = "B",
                            answers = new ImportAnswerModel()
                            {
                                A = "Venus",
                                B = "Mars",
                                C = "Jupiter",
                                D = "Earth"
                            }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            var bytes = Encoding.UTF8.GetBytes(json);
            var fileName = "sampleQuiz.json";

            return File(bytes, "application/json", fileName);
        }

        private async Task<List<QuizViewModelItem>> GetQuizzes()
        {
            List<QuizViewModelItem> quizzes = await _context.Quizzes
                                    .Select(q => new QuizViewModelItem()
                                    {
                                        Id = q.Id,
                                        Selected = false,
                                        QuizName = q.Name
                                    }).ToListAsync();
            return quizzes;
        }

    }
}
