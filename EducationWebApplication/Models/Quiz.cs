using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationWebApplication.Models
{
    public class Quiz
    {
        [Key]
        public int QuizId { get; set; }

        [Required]
        public string Question { get; set; }

        [Required]
        [DisplayName("Option 1")]
        public string Option1 { get; set; }

        [Required]
        [DisplayName("Option 2")]
        public string Option2 { get; set; }

        [Required]
        [DisplayName("Option 3")]
        public string Option3 { get; set; }

        [Required]
        [DisplayName("Option 4")]
        public string Option4 { get; set; }

        [Required]
        [DisplayName("Correct Answer")]
        public string CorrectAnswer { get; set; }

        public Quiz()
        {

        }
    }
}
