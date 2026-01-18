using Edukate101.Context;
using Edukate101.ViewModel.CourseViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Edukate101.Controllers
{
    public class CourseController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                                .Select(x=> new CourseGetVM
                                {
                                    Id = x.Id,
                                    Title = x.Title,
                                    ImagePath = x.ImagePath,
                                    Rating = x.Rating,
                                })
                                .ToListAsync();

            return View(courses);
        }
    }
}
