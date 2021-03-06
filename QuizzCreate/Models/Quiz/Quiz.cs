using System;
using System.Collections.Generic;

namespace QuizzCreate.Models.Quiz
{
    public class Quiz
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int QuizNo { get; set; }
        public string Title { get; set; }
        public string Paragraphs { get; set; } 
        public virtual List<Question> Questions { get; set; } 
    }
}
