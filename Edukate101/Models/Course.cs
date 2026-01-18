using Edukate101.Models.Common;

namespace Edukate101.Models
{
    public class Course : BaseEntity
    {
        public string Title { get; set; }
        public string ImagePath { get; set; }       
        public double Rating { get; set; }
        public ICollection<Teacher> Teachers { get; set; }

    }
}
