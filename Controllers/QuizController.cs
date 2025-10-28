using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleQuizApp.Data;
using SimpleQuizApp.Models;
using SimpleQuizApp.Models.ViewModels;

namespace SimpleQuizApp.Controllers
{
    public class QuizController : Controller
    {
        private readonly AppDbContext _context;

        public QuizController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(Guid Id)
        {
            List<QuestionViewModel> questions = await GetQuestionsByQuizId(Id);
            return View(new QuizViewModel() { QuizId = Id, Questions = questions });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitQuiz(List<Guid> userAnswers, Guid QuizId)
        {
            List<Question?> questions = await _context.QuizQuestions
                                            .Include(q => q.Question)
                                            .Where(q => q.QuizId == QuizId)
                                            .Select(q => q.Question)
                                            .ToListAsync();

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

        [HttpGet]
        public IActionResult Results()
        {
            return View();
        }

        private async Task<List<QuestionViewModel>> GetQuestionsByQuizId(Guid Id)
        {
            Guid[] qIDs = await _context.QuizQuestions
                                        .Where(q => q.QuizId == Id)
                                        .Select(q => q.QuestionId)
                                        .ToArrayAsync();

            List<QuestionViewModel> questions = await _context.Questions
                                        .Where(q => qIDs.Contains(q.Id))
                                        .Include(q => q.Answers)
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
                                        }).ToListAsync();

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
