using Microsoft.AspNetCore.Mvc;
using SimpleQuizApp.Models;
using SimpleQuizApp.Models.ViewModels;
using SimpleQuizApp.Services;

namespace SimpleQuizApp.Controllers
{
    /// <summary>
    /// Controller responsible for displaying quizzes, handling user submissions,
    /// and calculating quiz results.
    /// </summary>
    public class QuizController : Controller
    {
        private readonly MemoryStore _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuizController"/> class.
        /// </summary>
        /// <param name="context">
        /// The <see cref="MemoryStore"/> instance providing access to in-memory 
        /// quiz, question, and answer data</param>
        public QuizController(MemoryStore context)//AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays the quiz page for the specified quiz ID.
        /// </summary>
        /// <param name="Id">Unique identifier of the quiz to be displayed.</param>
        /// <returns>
        /// A view containing the quiz questions and related data.
        /// </returns>
        public IActionResult Index(Guid Id)
        {
            List<QuestionViewModel> questions = GetQuestionsByQuizId(Id);
            return View(new QuizViewModel() { QuizId = Id, Questions = questions });
        }

        /// <summary>
        /// Processes the submitted quiz answers, calculates the score,
        /// and determines whether the user has passed.
        /// </summary>
        /// <param name="userAnswers">List of answer IDs selected by the user.</param>
        /// <param name="QuizId">Unique identifier of the quiz that was taken.</param>
        /// <returns>
        /// The <see cref="Results"/> view containing the user's score and performance details.
        /// </returns>
        [HttpPost]
        public IActionResult SubmitQuiz(List<Guid> userAnswers, Guid QuizId)
        {
            List<Question> questions = (from qq in _context.QuizQuestions
                                        join q in _context.Questions on qq.QuestionId equals q.Id
                                        where qq.QuizId == QuizId
                                        select q).ToList();

            var totalQuestions = questions.Count;
            var correctAnswers = questions.Where(q => userAnswers.Contains(q.CorrectAnswer)).Count();
            int score = (int)(100 * ((double)correctAnswers / (double)totalQuestions));

            ViewBag.TotalQuestions = totalQuestions;
            ViewBag.CorrectAnswers = correctAnswers;
            ViewBag.Answers = userAnswers.Count();
            ViewBag.Score = score;
            ViewBag.Graded = (score >= 75);

            return View(nameof(Results));
        }

        /// <summary>
        /// Displays the results page after a quiz submission.
        /// </summary>
        /// <returns>
        /// The results view showing the score and quiz performance.
        /// </returns>
        [HttpGet]
        public IActionResult Results()
        {
            return View();
        }

        /// <summary>
        /// Retrieves all questions and their answers for a given quiz ID.
        /// Randomizes both the order of questions and the order of answers.
        /// </summary>
        /// <param name="Id">Unique identifier of the quiz whose questions will be retrieved.</param>
        /// <returns>
        /// A list of <see cref="QuestionViewModel"/> objects representing randomized quiz questions.
        /// </returns>
        private List<QuestionViewModel> GetQuestionsByQuizId(Guid Id)
        {
            Guid[] qIDs = _context.QuizQuestions
                                        .Where(q => q.QuizId == Id)
                                        .Select(q => q.QuestionId)
                                        .ToArray();

            List<QuestionViewModel> questions = _context.Questions
                                        .Where(q => qIDs.Contains(q.Id))
                                        .Select(q => new QuestionViewModel()
                                        {
                                            Id = q.Id,
                                            QuestionText = q.QuestionText,
                                            Answers = q.Answers
                                                        .Select(a =>
                                                        new QuestionViewModelAnswer()
                                                        {
                                                            Text = a.AnswerText,
                                                            Id = a.Id,
                                                            QuestionViewModelId = q.Id,
                                                            Option = null
                                                        }).ToList()
                                        }).ToList();

            // Randomize question and answer order
            Random rand = new Random();
            _context.RandomizeAnswers(questions, rand);
            
            foreach (var question in questions)
            {
                _context.RandomizeAnswers(question.Answers, rand);
                for (var i = 0; i < question.Answers.Count; i++)
                {
                    question.Answers[i].Option = (AnswerOption)(i);
                }
            }

            return questions;
        }
    }
}
