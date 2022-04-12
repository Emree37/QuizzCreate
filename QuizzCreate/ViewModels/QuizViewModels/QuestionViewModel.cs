using QuizzCreate.Models.Quiz;
using System.Collections.Generic;

namespace QuizzCreate.ViewModels.QuizViewModels
{
    public class QuestionViewModel
    {
        public string QuestionContent { get; set; } // Soru icerigi
        public OptionCharacter CorrectAnswer { get; set; } // Dogru secenek
        public virtual List<OptionViewModel> Options { get; set; }
    }
}
