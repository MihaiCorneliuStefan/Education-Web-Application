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
    public class QuizzesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuizzesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Quizzes
        [Authorize]
        public async Task<IActionResult> Index()
        {
              return _context.Quiz != null ? 
              View(await _context.Quiz.ToListAsync()) :
              Problem("Entity set 'ApplicationDbContext.Quiz'  is null.");
        }

        // GET: Quizzes/ShowQuizzes
        [Authorize]
        public async Task<IActionResult> ShowQuizzes()
        {
            var quizzes = await _context.Quiz.ToListAsync();
            return View(quizzes);
        }

        // POST: Quizzes/SubmitQuiz
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitQuiz(IFormCollection form)
        {
            var quizzes = await _context.Quiz.ToListAsync();
            var score = 0;

            foreach (var quiz in quizzes)
            {
                var selectedAnswer = form[quiz.QuizId.ToString()];
                if (selectedAnswer == quiz.CorrectAnswer)
                {
                    score++;
                }
            }

            return View("QuizResult",score);
        }

        [Authorize]
        // GET: Quizzes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Quiz == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quiz
            .FirstOrDefaultAsync(m => m.QuizId == id);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // GET: Quizzes/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Quizzes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuizId,Question,Option1,Option2,Option3,Option4,CorrectAnswer")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quiz);
                await _context.SaveChangesAsync();
                TempData["success"] = "Quiz created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(quiz);
        }

        // GET: Quizzes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Quiz == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quiz.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }
            return View(quiz);
        }

        // POST: Quizzes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuizId,Question,Option1,Option2,Option3,Option4,CorrectAnswer")] Quiz quiz)
        {
            if (id != quiz.QuizId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quiz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizExists(quiz.QuizId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["success"] = "Quiz updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(quiz);
        }

        // GET: Quizzes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Quiz == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quiz
                .FirstOrDefaultAsync(m => m.QuizId == id);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // POST: Quizzes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Quiz == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Quiz'  is null.");
            }
            var quiz = await _context.Quiz.FindAsync(id);
            if (quiz != null)
            {
                _context.Quiz.Remove(quiz);
            }
            
            await _context.SaveChangesAsync();
            TempData["success"] = "Quiz deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool QuizExists(int id)
        {
          return (_context.Quiz?.Any(e => e.QuizId == id)).GetValueOrDefault();
        }
    }
}
