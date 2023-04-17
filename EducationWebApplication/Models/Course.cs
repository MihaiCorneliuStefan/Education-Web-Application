using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EducationWebApplication.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Course Name")]
        public string CourseName { get; set; }
        [DisplayName("Course Materials")]
        public string CourseMaterials { get; set; }
        [DisplayName("Course Professor")]
        public string CourseProf { get; set; }


        public Course()
        {

        }
    }
}
