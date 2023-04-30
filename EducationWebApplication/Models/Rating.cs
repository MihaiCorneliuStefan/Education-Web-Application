using System.ComponentModel.DataAnnotations;

namespace EducationWebApplication.Models
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }
        [Required]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }
        [Required]
        [Display(Name = "Rating")]
        public int RatingValue { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string RatingDescription { get; set; }
        [Required]
        public DateTime RatingTime { get; set; }
        public Rating()
        {
            RatingTime = DateTime.Now;
        }
    }
}