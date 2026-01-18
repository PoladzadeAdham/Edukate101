using System.ComponentModel.DataAnnotations;

namespace Edukate101.ViewModel.CourseViewModel
{
    public class CourseCreateVM
    {
        [Required, MaxLength(255)]
        public string Title { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        [Range(0, 5)]
        public double Rating { get; set; }
    }
}
