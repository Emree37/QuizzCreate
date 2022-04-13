using QuizzCreate.Models.Quiz;
using System.Collections.Generic;

namespace QuizzCreate.ViewModels.QuizViewModels
{
    public class QuestionViewModel
    {
        public string QuestionContent { get; set; } 
        public OptionCharacter CorrectAnswer { get; set; } 
        public virtual List<OptionViewModel> Options { get; set; }
    }
}
