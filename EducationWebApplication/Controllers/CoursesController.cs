using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EducationWebApplication.Data;
using EducationWebApplication.Models;
using Microsoft.AspNetCore.Authorization;

namespace EducationWebApplication.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Course.ToListAsync());
        }
        // GET: Courses/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults()
        {
            return View();
        }
        // POST: Courses/ShowResults
        public async Task<IActionResult> ShowResults(String SearchPhrase)
        {
            return View("Index", await _context.Course.Where(i => i.CourseName.Contains(SearchPhrase)).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> SearchSuggestions(string searchString)
        {
            var courses = await _context.Course
                .Where(c => c.CourseName.StartsWith(searchString))
                .Select(c => new { label = c.CourseName })
                .Distinct()
                .ToListAsync();

            return Json(courses);
        }

        public async Task<IActionResult> ShowRatingResults(String SearchPhrase)
        {
            return View("GetCourseRatings", await _context.Rating.Where(i => i.CourseName.Contains(SearchPhrase)).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseRatings()
        {
            var ratings = await _context.Rating.ToListAsync();
            return View(ratings);
        }
        [Authorize]
        public IActionResult AddRating()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddRating(Rating rating)
        {
            var course = await _context.Course.FirstOrDefaultAsync(c => c.CourseName == rating.CourseName);
            if (course == null)
            {
                return View(rating);
            }

            if (ModelState.IsValid)
            {
                _context.Rating.Add(rating);
                await _context.SaveChangesAsync();
                TempData["success"] = "Rating created successfully";
                return RedirectToAction("GetCourseRatings");
            }

            return View(rating);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, Course course, IFormFile file, string youtubeLink)
        {
            var existingCourse = await _context.Course.FindAsync(id);
            if (existingCourse == null)
            {
                return NotFound();
            }

            var courseMaterials = new List<string>();

            if (!string.IsNullOrEmpty(existingCourse.CourseMaterials))
            {
                courseMaterials.AddRange(existingCourse.CourseMaterials.Split("\n\n"));
            }

            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            if (currentUser != null)
            {
                string userName = currentUser.UserName;
                var postTime = DateTime.Now;
                courseMaterials.Add($"Posted on: {postTime} by: {userName}");
            }


            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                courseMaterials.Add(fileName);
            }

            if (!string.IsNullOrEmpty(youtubeLink))
            {
                courseMaterials.Add(youtubeLink);
            }

            if (!string.IsNullOrEmpty(course.CourseMaterials))
            {
                courseMaterials.Add(course.CourseMaterials);
            }

            existingCourse.CourseMaterials = string.Join("\n\n", courseMaterials);
            await _context.SaveChangesAsync();
            TempData["success"] = "Post created successfully";
            ModelState.Clear();
            return RedirectToAction(nameof(Details), new { id = id });
        }




        // GET: Courses/Create
        [Authorize(Roles = "Administrator,Manager")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseName,CourseMaterials,CourseProf")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                TempData["success"] = "Course created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseName,CourseMaterials,CourseProf")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["success"] = "Course updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Delete
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Course'  is null.");
            }
            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                _context.Course.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            TempData["success"] = "Course deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
          return _context.Course.Any(e => e.Id == id);
        }
    }
}
