using SimpleQuizApp.Models;

namespace SimpleQuizApp.Services
{
    /// <summary>
    /// The <see cref="MemoryStore"/> class serves as an in-memory data store 
    /// for quizzes, questions, answers, and their relationships.
    /// </summary>
    public class MemoryStore
    {
        /// <summary>
        /// A collection of all available quizzes (<see cref="Quiz"/>)
        /// stored in memory
        /// </summary>
        public required List<Quiz> Quizzes { get; set; } = [];
        /// <summary>
        /// A collection of all questions (<see cref="Question"/>) 
        /// used in the quizzes stored in memory
        /// </summary>
        public required List<Question> Questions { get; set; } = [];
        /// <summary>
        /// A collection of all possible answers (<see cref="Answer"/>)
        /// associated with the questions stored in memory
        /// </summary>
        public required List<Answer> Answers { get; set; } = [];
        /// <summary>
        /// A mapping between quizzes and their corresponding questions,
        /// represented by <see cref="QuizQuestions"/> objects 
        /// </summary>
        public required List<QuizQuestions> QuizQuestions { get; set; } = [];

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
        /// <param name="listForImport">List of quizzes to be imported from JSON or another source.</param>
        public void ImportQuizzes(List<ImportQuizModel> listForImport)
        {
            List<Quiz> listQuizzes = new List<Quiz>();
            List<Question> listQuestions = new List<Question>();
            List<QuizQuestions> listQuizQuestions = new List<QuizQuestions>();

            foreach (var quiz in listForImport)
            {
                Quiz newQuiz = new Quiz() { Id = Guid.NewGuid(), Name = quiz.name };

                List<Question> newQuestions = GetImportQuestions(quiz.questions);
                foreach (Question newQuestion in newQuestions)
                {
                    listQuestions.Add(newQuestion);
                    listQuizQuestions.Add(new QuizQuestions() { Id = Guid.NewGuid(), QuizId = newQuiz.Id, QuestionId = newQuestion.Id });
                }
                listQuizzes.Add(newQuiz);
            }
            foreach (var quiz in listQuizzes)
                Quizzes.Add(quiz);

            foreach (var question in listQuestions)
            {
                Questions.Add(question);
                foreach (var answer in question.Answers)
                    Answers.Add(answer);
            }

            foreach (var qq in listQuizQuestions)
                QuizQuestions.Add(qq);

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
