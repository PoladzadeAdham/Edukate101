using System.Diagnostics;
using System.Threading.Tasks;
using Edukate101.Context;
using Edukate101.ViewModel.TeacherViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Edukate101.Controllers
{
    [Authorize(Roles ="Member,Admin")]
    public class HomeController(AppDbContext _context) : Controller
    {
       
        public async Task<IActionResult> Index()
        {
            var teachers = await _context.Teachers
                                                .Select(x=> new TeacherGetVM
                                                {
                                                    Id = x.Id,
                                                    CourseName = x.Course.Title,
                                                    FullName = x.FullName,
                                                    ImagePath = x.ImagePath,
                                                })
                                                .ToListAsync();

            return View(teachers);
        }

    }
}
