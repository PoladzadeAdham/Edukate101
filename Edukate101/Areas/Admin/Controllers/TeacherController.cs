using Edukate101.Context;
using Edukate101.Helpers;
using Edukate101.Models;
using Edukate101.ViewModel.TeacherViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Edukate101.Areas.Admin.Controllers
{
    [Area("Admin")] 
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _folderPath;

        public TeacherController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _folderPath = Path.Combine(_webHostEnvironment.WebRootPath,"img");  
        }

        public async Task<IActionResult> Update(int id)
        {
            await SendCoursesWithViewBag();

            var teacher = await _context.Teachers
                                            .Select(x=> new TeacherUpdateVm
                                            {
                                                Id = x.Id,
                                                FullName = x.FullName,
                                                CourseId = x.CourseId,
                                            })
                                            .FirstOrDefaultAsync(x => x.Id == id);

            return View(teacher);

        }

        [HttpPost]
        public async Task<IActionResult> Update(TeacherUpdateVm vm)
        {
            await SendCoursesWithViewBag();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }


            if (!vm.Image?.CheckSize(2) ?? false)
            {
                ModelState.AddModelError("", "sekilin olcusu 2 mbdan cox olmamalidir.");
                return View(vm);
            }

            if (!vm.Image?.CheckType() ?? false)
            {
                ModelState.AddModelError("", "sekil image tipinde olmalidir.");
                return View(vm);
            }

            var existingTeacher = await _context.Teachers.FindAsync(vm.Id);

            if (existingTeacher == null)
                return NotFound();

            if(vm.Image is { })
            {
                string path = Path.Combine(_folderPath, existingTeacher.ImagePath);

                ExtensionMethod.DeleteFile(path);

                string uniqueFileName = await vm.Image.SaveFileasync(_folderPath);

                existingTeacher.ImagePath = uniqueFileName;
            }

            existingTeacher.FullName = vm.FullName;
            existingTeacher.CourseId = vm.CourseId;

            _context.Teachers.Update(existingTeacher);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");   

        }


        public async Task<IActionResult> Delete(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if(teacher == null) 
                return NotFound();

             _context.Remove(teacher);
            await _context.SaveChangesAsync();

            string path = Path.Combine(_folderPath, teacher.ImagePath); 

            ExtensionMethod.DeleteFile(path);

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Index()
        {
            var teachers = await _context.Teachers.Select(x=> new TeacherGetVM
                                            {
                                                Id = x.Id,
                                                CourseName = x.Course.Title,
                                                FullName = x.FullName,
                                                ImagePath = x.ImagePath,
                                            })
                                            .ToListAsync();

            return View(teachers);
        }

        public async Task<IActionResult> Create()
        {
            await SendCoursesWithViewBag();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeacherCreateVm vm)
        {
            await SendCoursesWithViewBag();

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (!vm.Image.CheckSize(2))
            {
                ModelState.AddModelError("", "sekilin olcusu 2 mbdan cox olmamalidir.");
                return View(vm);
            }

            if (!vm.Image.CheckType())
            {
                ModelState.AddModelError("", "sekil image tipinde olmalidir.");
                return View(vm);
            }

            string uniqueFileName = await vm.Image.SaveFileasync(_folderPath);

            Teacher teacher = new()
            {
                FullName = vm.FullName,
                CourseId = vm.CourseId,
                ImagePath = uniqueFileName
            };

            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");   


        }

        private async Task SendCoursesWithViewBag()
        {
            var courses = await _context.Courses.ToListAsync();

            ViewBag.Courses = courses;
        }
    }
}
