using System;

namespace QuizzCreate.ViewModels
{
    public class QuizAnswerViewModel
    {
        public bool isCorrect { get; set; }
        public Guid QuestionId { get; set; }
        public Guid OptionId { get; set; }
    }
}
