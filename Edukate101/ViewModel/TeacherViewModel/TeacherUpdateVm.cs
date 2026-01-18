using System.ComponentModel.DataAnnotations;

namespace Edukate101.ViewModel.TeacherViewModel
{
    public class TeacherUpdateVm
    {
        public int Id { get; set; }
        [Required, MaxLength(255)]
        public string FullName { get; set; }
        public IFormFile? Image { get; set; }
        [Required]
        public int CourseId { get; set; }
    }
}
