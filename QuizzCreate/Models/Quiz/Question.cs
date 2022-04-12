using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizzCreate.Models.Quiz
{
    public class Question
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int QuestionNo { get; set; } // Soru numarasi
        public string QuestionContent { get; set; } // Soru icerigi
        public OptionCharacter CorrectAnswer { get; set; } // Dogru secenek

        // Hangi teste bagli oldugunu ifade edyor
        public Guid QuizId { get; set; }
        [ForeignKey(nameof(QuizId))]
        public virtual Quiz Quiz { get; set; }

        // Birden fazla secenek iceriyor
        public virtual List<Option> Options { get; set; }
    }
}
