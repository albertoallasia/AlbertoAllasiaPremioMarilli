using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestioneTerreniAgricoli.Data;
using GestioneTerreniAgricoli.Models;
using Microsoft.AspNetCore.Authorization;

namespace GestioneTerreniAgricoli.Controllers
{
    [Authorize]
    public class SpesaController : Controller
    {
        private readonly GestioneTerreniAgricoliContext _context;

        public SpesaController(GestioneTerreniAgricoliContext context)
        {
            _context = context;
        }

        // GET: Spesa
        public async Task<IActionResult> Index(string searchString, string searchAttribute, decimal? minImporto, decimal? maxImporto)
        {
            if (_context.Spesa == null)
            {
                return Problem("Entity set 'Spesa' is null.");
            }

            var spese = _context.Spesa.Include(s => s.Lavoro).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                if (!String.IsNullOrEmpty(searchAttribute))
                {
                    if (searchAttribute == "Descrizione")
                    {
                        spese = spese.Where(s => s.Descrizione.Contains(searchString));
                    }
                    else if (searchAttribute == "Lavoro")
                    {
                        spese = spese.Where(s => s.Lavoro.Descrizione.Contains(searchString));
                    }
                    
                }
            }
            else
            {
                
                if (minImporto != null && maxImporto != null)
                {
                    spese = spese.Where(s => s.Importo >= minImporto && s.Importo <= maxImporto);
                }
            }

            return View(await spese.ToListAsync());
        }

        // GET: Spesa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Spesa == null)
            {
                return NotFound();
            }

            var spesa = await _context.Spesa
                .Include(s => s.Lavoro)
                .FirstOrDefaultAsync(m => m.IdSpesa == id);
            if (spesa == null)
            {
                return NotFound();
            }

            return View(spesa);
        }

        // GET: Spesa/Create
        public IActionResult Create()
        {
            ViewData["LavoroId"] = new SelectList(_context.Lavoro, "IdLavoro", "Descrizione");
            return View();
        }

        // POST: Spesa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSpesa,Descrizione,Importo,DataAquisto,LavoroId")] Spesa spesa)
        {
            ModelState.Remove("Lavoro");
            if (ModelState.IsValid)
            {
                _context.Add(spesa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LavoroId"] = new SelectList(_context.Lavoro, "IdLavoro", "IdLavoro", spesa.LavoroId);
            return View(spesa);
        }

        // GET: Spesa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Spesa == null)
            {
                return NotFound();
            }

            var spesa = await _context.Spesa.FindAsync(id);
            if (spesa == null)
            {
                return NotFound();
            }
            ViewData["LavoroId"] = new SelectList(_context.Lavoro, "IdLavoro", "Descrizione", spesa.LavoroId);
            return View(spesa);
        }

        // POST: Spesa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSpesa,Descrizione,Importo,DataAquisto,LavoroId")] Spesa spesa)
        {
            if (id != spesa.IdSpesa)
            {
                return NotFound();
            }
            ModelState.Remove("Lavoro");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spesa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpesaExists(spesa.IdSpesa))
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
            ViewData["LavoroId"] = new SelectList(_context.Lavoro, "IdLavoro", "IdLavoro", spesa.LavoroId);
            return View(spesa);
        }

        // GET: Spesa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Spesa == null)
            {
                return NotFound();
            }

            var spesa = await _context.Spesa
                .Include(s => s.Lavoro)
                .FirstOrDefaultAsync(m => m.IdSpesa == id);
            if (spesa == null)
            {
                return NotFound();
            }

            return View(spesa);
        }

        // POST: Spesa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Spesa == null)
            {
                return Problem("Entity set 'GestioneTerreniAgricoliContext.Spesa'  is null.");
            }
            var spesa = await _context.Spesa.FindAsync(id);
            if (spesa != null)
            {
                _context.Spesa.Remove(spesa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpesaExists(int id)
        {
            return (_context.Spesa?.Any(e => e.IdSpesa == id)).GetValueOrDefault();
        }
    }
}
