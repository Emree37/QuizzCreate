using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizzCreate.Models.Quiz
{
    public class Option
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public OptionCharacter Character { get; set; } 
        public string OptionContent { get; set; } 

        public Guid QuestionId { get; set; }
        [ForeignKey(nameof(QuestionId))]
        public virtual Question Question { get; set; }
    }

    public enum OptionCharacter
    {
        A,
        B,
        C,
        D
    }
}
