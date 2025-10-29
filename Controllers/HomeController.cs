using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SimpleQuizApp.Models;
using SimpleQuizApp.Models.ViewModels;
using SimpleQuizApp.Services;

namespace SimpleQuizApp.Controllers
{
    /// <summary>
    /// Main controller responsible for handling the home page,
    /// choosing a quiz to solve, quiz import functionality,
    /// and sample JSON export
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MemoryStore _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">Logger used for diagnostic and error logging</param>
        /// <param name="context">
        /// The <see cref="MemoryStore"/> instance providing access to in-memory 
        /// quiz, question, and answer data</param>
        public HomeController(ILogger<HomeController> logger, MemoryStore context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Displays the home page with the list of available quizzes.
        /// </summary>
        /// <returns>
        /// A view displaying all quizzes available in the database.
        /// </returns>
        [HttpGet]
        public IActionResult Index()
        {
            List<QuizViewModelItem> quizzes = GetQuizzes();
            return View(new SelectQuizViewModel { Quizzes = quizzes });
        }

        /// <summary>
        /// Handles the upload of a JSON file containing quizzes
        /// Validates and imports the quizzes into the database
        /// </summary>
        /// <param name="file">Uploaded JSON file containing quiz definitions</param>
        /// <returns>
        /// Redirects back to the Index page with a success or error message
        /// </returns>
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

        /// <summary>
        /// Generates and returns a downloadable sample JSON file
        /// that users can use as a template for creating new quizzes.
        /// </summary>
        /// <returns>
        /// A JSON file (<c>application/json</c>) named <c>sampleQuiz.json</c>.
        /// </returns>
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

        /// <summary>
        /// Retrieves all quizzes from the database and maps them
        /// into a view model suitable for displaying in the view.
        /// </summary>
        /// <returns>
        /// A list of <see cref="QuizViewModelItem"/> representing available quizzes.
        /// </returns>
        private List<QuizViewModelItem> GetQuizzes()
        {
            List<QuizViewModelItem> quizzes = _context.Quizzes
                                    .Select(q => new QuizViewModelItem()
                                    {
                                        Id = q.Id,
                                        Selected = false,
                                        QuizName = q.Name
                                    }).ToList();
            return quizzes;
        }

    }
}
