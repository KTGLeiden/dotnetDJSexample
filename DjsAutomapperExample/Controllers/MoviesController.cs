using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data;
using AutoMapper;
using DjsAutomapperExample.Models;

namespace DjsAutomapperExample.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieContext _context;
        private readonly IMapper _mapper;

        public MoviesController(MovieContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //_context.Movies.Include(x => x.MovieActors).ThenInclude(x => x.Actor);

            var movie = await _context.Movies
                .Include(m => m.MovieActors)
                .ThenInclude(ma => ma.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            ViewData["Actors"] = new SelectList(_context.Actors.ToList().Where(a => !movie.MovieActors.Any(ma => ma.ActorId == a.Id)), "Id", "Name");
            return View(_mapper.Map<MovieDetailViewModel>(movie));
        }

        public async Task<IActionResult> RemoveActor(int movieId, int actorId)
        {
            // Get movie
            var movie = await _context.Movies
               .Include(m => m.MovieActors)
               .FirstOrDefaultAsync(m => m.Id == movieId);
            // Remove actor from movie
            movie.MovieActors.RemoveAll(ma => ma.ActorId == actorId);
            // Save changes to DB :)
            _context.Update(movie);
            _context.SaveChanges();
            // Redirect back to details
            return RedirectToAction(nameof(Details), new { id = movieId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddActor(AddActorDTO addActorDTO)
        {
            // Get movie
            var movie = await _context.Movies
               .Include(m => m.MovieActors)
               .FirstOrDefaultAsync(m => m.Id == addActorDTO.movieId);
            // Get actor
            var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Id == addActorDTO.actorId);
            // Add actor to movie
            movie.MovieActors.Add(new MovieActor
            {
                Movie = movie,
                Actor = actor
            });
            // Save changes to DB :)
            _context.Update(movie);
            _context.SaveChanges();
            // Redirect back to details
            return RedirectToAction(nameof(Details), new { id = addActorDTO.movieId });
        }

        // GET: Movies/Create
        public IActionResult CreateTest()
        {
            var actor = new Actor { dateOfBirth = DateTime.Now, Name = "HANK" };
            var movie = new Movie
            {
                Name = "Independence day",
                Year = 1992
            };
            // Add relationship
            movie.MovieActors = new List<MovieActor> {new MovieActor {
                Actor = actor,
                Movie = movie
            } };
            _context.Movies.Add(movie);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Year")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Year")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
