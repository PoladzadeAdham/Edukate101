using Edukate101.Models.Common;

namespace Edukate101.Models
{
    public class Teacher : BaseEntity
    {
        public string FullName { get; set; }
        public string ImagePath { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
