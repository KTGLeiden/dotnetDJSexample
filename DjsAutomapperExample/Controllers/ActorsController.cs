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
    public class ActorsController : Controller
    {
        private readonly MovieContext _context;
        private readonly IMapper _mapper;

        public ActorsController(MovieContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Actors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Actors.ToListAsync());
        }

        // GET: Actors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .Include(a => a.MovieActors)
                .ThenInclude(ma => ma.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            ViewData["Movies"] = new SelectList(_context.Movies.ToList().Where(m => !actor.MovieActors.Any(ma => ma.MovieId == m.Id)), "Id", "Name");
            return View(_mapper.Map<ActorDetailViewModel>(actor));
        }

        public async Task<IActionResult> RemoveMovie(int actorId, int movieId)
        {
            // Get actor
            var actor = await _context.Actors
               .Include(m => m.MovieActors)
               .FirstOrDefaultAsync(m => m.Id == actorId);
            // Remove movie from actor
            actor.MovieActors.RemoveAll(ma => ma.MovieId == movieId);
            // Save changes to DB :)
            _context.Update(actor);
            _context.SaveChanges();
            // Redirect back to details
            return RedirectToAction(nameof(Details), new { id = actorId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMovie(AddActorDTO addActorDTO)
        {
            // Get actor
            var actor = await _context.Actors
               .Include(m => m.MovieActors)
               .FirstOrDefaultAsync(m => m.Id == addActorDTO.actorId);
            // Get movie
            var movie = await _context.Movies.FirstOrDefaultAsync(a => a.Id == addActorDTO.movieId);
            // Add actor to movie
            actor.MovieActors.Add(new MovieActor
            {
                Movie = movie,
                Actor = actor
            });
            // Save changes to DB :)
            _context.Update(actor);
            _context.SaveChanges();
            // Redirect back to details
            return RedirectToAction(nameof(Details), new { id = addActorDTO.actorId });
        }

        // GET: Actors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Actors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,dateOfBirth")] Actor actor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        // GET: Actors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,dateOfBirth")] Actor actor)
        {
            if (id != actor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.Id))
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
            return View(actor);
        }

        // GET: Actors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorExists(int id)
        {
            return _context.Actors.Any(e => e.Id == id);
        }
    }
}
