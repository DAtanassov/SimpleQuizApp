namespace SimpleQuizApp.Models
{
    /// <summary>
    /// Represents the data structure used for importing a quiz from JSON
    /// </summary>
    public class ImportQuizModel
    {
        /// <summary>
        /// The name of the quiz
        /// </summary>
        public required string name {  get; set; }
        /// <summary>
        /// The list of questions contained in the quiz
        /// </summary>
        public required List<ImportQuestionModel> questions { get; set; }
    }

    /// <summary>
    /// Represents a question imported from a JSON file
    /// </summary>
    public class ImportQuestionModel
    {
        /// <summary>
        /// The question text.
        /// </summary>
        public required string questionText { get; set; }
        /// <summary>
        /// The correct answer key ("A", "B", "C", or "D")
        /// </summary>
        public required string correctAnswer { get; set; }
        /// <summary>
        /// The possible answers for this question
        /// </summary>
        public required ImportAnswerModel answers { get; set; }
    }

    /// <summary>
    /// Represents a collection of possible answers for an imported question
    /// </summary>
    public class ImportAnswerModel
    {
        /// <summary>
        /// The text of answer option A.
        /// </summary>
        public required string A { get; set; }
        /// <summary>
        /// The text of answer option B.
        /// </summary>
        public required string B { get; set; }
        /// <summary>
        /// The text of answer option C.
        /// </summary>
        public required string C { get; set; }
        /// <summary>
        /// The text of answer option D.
        /// </summary>
        public required string D { get; set; }
    }
}
