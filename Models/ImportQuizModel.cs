namespace SimpleQuizApp.Models
{
    public class ImportQuizModel
    {
        public required string name {  get; set; }
        public required List<ImportQuestionModel> questions { get; set; }
    }

    public class ImportQuestionModel
    {
        public required string questionText { get; set; }
        public required string correctAnswer { get; set; }
        public required ImportAnswerModel answers { get; set; }
    }

    public class ImportAnswerModel
    {
        public required string A { get; set; }
        public required string B { get; set; }
        public required string C { get; set; }
        public required string D { get; set; }
    }
}
