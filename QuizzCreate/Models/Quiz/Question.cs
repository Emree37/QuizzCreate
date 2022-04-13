using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizzCreate.Models.Quiz
{
    public class Question
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int QuestionNo { get; set; } 
        public string QuestionContent { get; set; } 
        public OptionCharacter CorrectAnswer { get; set; } 

        public Guid QuizId { get; set; }
        [ForeignKey(nameof(QuizId))]
        public virtual Quiz Quiz { get; set; }

        public virtual List<Option> Options { get; set; }
    }
}
