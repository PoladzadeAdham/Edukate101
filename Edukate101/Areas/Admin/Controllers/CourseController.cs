using Edukate101.Context;
using Edukate101.Helpers;
using Edukate101.Models;
using Edukate101.ViewModel.CourseViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Edukate101.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly string _folderPath;

        public CourseController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _folderPath = Path.Combine(_environment.WebRootPath, "img");
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                                                .Select(x => new CourseGetVM
                                                {
                                                    Id = x.Id,
                                                    ImagePath = x.ImagePath,
                                                    Title = x.Title,
                                                    Rating = x.Rating,
                                                })
                                                .ToListAsync();

            return View(courses);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (!vm.Image.CheckSize(2))
            {
                ModelState.AddModelError("", "Seklin olcusu 2 mbdan cox olmamalidir.");
                return View(vm);
            }

            if (!vm.Image.CheckType())
            {
                ModelState.AddModelError("", "Seklin olcusu 2 mbdan cox olmamalidir.");
                return View(vm);
            }

            string uniqueFileName = await vm.Image.SaveFileasync(_folderPath);

            Course course = new()
            {
                ImagePath = uniqueFileName,
                Title = vm.Title,
                Rating = vm.Rating,
            };

            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return NotFound();

            _context.Remove(course);
            await _context.SaveChangesAsync();

            string path = Path.Combine(_folderPath, course.ImagePath);

            ExtensionMethod.DeleteFile(path);

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Update(int id)
        {
            var course = await _context.Courses
                                                .Select(x => new CourseUpdateVM
                                                {
                                                    Id = x.Id,
                                                    Title = x.Title,
                                                    Rating = x.Rating,
                                                })
                                                .FirstOrDefaultAsync(x => x.Id == id);

            return View(course);

        }

        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (!vm.Image?.CheckSize(2) ?? false)
            {
                ModelState.AddModelError("", "Seklin olcusu 2 mbdan cox olmamalidir.");
                return View(vm);
            }

            if (!vm.Image?.CheckType() ?? false)
            {
                ModelState.AddModelError("", "Seklin olcusu 2 mbdan cox olmamalidir.");
                return View(vm);
            }

            var existingCourse = await _context.Courses.FindAsync(vm.Id);

            if (existingCourse == null)
                return NotFound();

            if (vm.Image is { })
            {
                string path = Path.Combine(_folderPath, existingCourse.ImagePath);

                ExtensionMethod.DeleteFile(path);

                string uniqueFileName = await vm.Image.SaveFileasync(_folderPath);

                existingCourse.ImagePath = uniqueFileName;

            }

            existingCourse.Title = vm.Title;
            existingCourse.Rating = vm.Rating;

            _context.Update(existingCourse);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }


    }
}
