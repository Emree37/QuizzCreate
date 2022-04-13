using System;
using System.Collections.Generic;

namespace QuizzCreate.ViewModels.QuizViewModels
{
    public class QuizViewModel
    {
        public DateTime CreatedDate { get; set; }
        public string Title { get; set; }
        public string Paragraphs { get; set; } 
        public virtual List<QuestionViewModel> Questions { get; set; } 
    }
}
