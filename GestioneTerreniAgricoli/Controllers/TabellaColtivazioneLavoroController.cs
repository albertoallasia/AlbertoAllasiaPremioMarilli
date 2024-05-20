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
    public class TabellaColtivazioneLavoroController : Controller
    {
        private readonly GestioneTerreniAgricoliContext _context;

        public TabellaColtivazioneLavoroController(GestioneTerreniAgricoliContext context)
        {
            _context = context;
        }

        // GET: TabellaColtivazioneLavoro

        public async Task<IActionResult> IndexLavorazioni(int? id, string searchString, string searchAttribute)
        {
            if (_context.TabellaColtivazioneLavoro == null)
            {
                return Problem("Entity set 'TabellaColtivazioneLavoro' is null.");
            }

            var tabellaColtivazioneLavoroQuery = _context.TabellaColtivazioneLavoro
                                                       .Include(t => t.Coltivazione)
                                                       .Where(t => t.ColtivazioneId == id)
                                                       .Include(t => t.Lavoro)
                                                       .AsQueryable();

            if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(searchAttribute))
            {
                if (searchAttribute == "Lavoro")
                {
                    tabellaColtivazioneLavoroQuery = tabellaColtivazioneLavoroQuery.Where(t => t.Lavoro.Descrizione.Contains(searchString));
                }
                else if (searchAttribute == "Coltivazione")
                {
                    tabellaColtivazioneLavoroQuery = tabellaColtivazioneLavoroQuery.Where(t => t.Coltivazione.NomeColtivazione.Contains(searchString));
                }
                // Aggiungi altre clausole 'else if' per altri attributi, se necessario
            }

            return View(await tabellaColtivazioneLavoroQuery.ToListAsync());
        }


        public async Task<IActionResult> Index(int? id, string searchString, string searchAttribute)
        {
            if (_context.TabellaColtivazioneLavoro == null)
            {
                return Problem("Entity set 'TabellaColtivazioneLavoro' is null.");
            }

            var tabellaColtivazioneLavoroQuery = _context.TabellaColtivazioneLavoro
                                                       .Include(t => t.Coltivazione)
                                                       .Include(t => t.Lavoro)
                                                       .Where(t => t.LavoroId == id)
                                                       .AsQueryable();

            if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(searchAttribute))
            {
                if (searchAttribute == "Lavoro")
                {
                    tabellaColtivazioneLavoroQuery = tabellaColtivazioneLavoroQuery.Where(t => t.Lavoro.Descrizione.Contains(searchString));
                }
                else if (searchAttribute == "Coltivazione")
                {
                    tabellaColtivazioneLavoroQuery = tabellaColtivazioneLavoroQuery.Where(t => t.Coltivazione.NomeColtivazione.Contains(searchString));
                }
                // Aggiungi altre clausole 'else if' per altri attributi, se necessario
            }

            return View(await tabellaColtivazioneLavoroQuery.ToListAsync());
        }


        // GET: TabellaColtivazioneLavoro/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TabellaColtivazioneLavoro == null)
            {
                return NotFound();
            }

            var tabellaColtivazioneLavoro = await _context.TabellaColtivazioneLavoro
                .Include(t => t.Coltivazione)
                .Include(t => t.Lavoro)
                .FirstOrDefaultAsync(m => m.IdTabellaColtivazioneLavoro == id);
            if (tabellaColtivazioneLavoro == null)
            {
                return NotFound();
            }

            return View(tabellaColtivazioneLavoro);
        }

        // GET: TabellaColtivazioneLavoro/Create
        public IActionResult Create()
        {
            var coltivazioniNonTerminate = _context.Coltivazione.Where(c => !c.ColtivazioneTerminata).ToList();
            ViewData["ColtivazioneId"] = new SelectList(coltivazioniNonTerminate, "IdColtivazione", "NomeColtivazione");
            ViewData["LavoroId"] = new SelectList(_context.Lavoro, "IdLavoro", "Descrizione");
            return View();
        }

        // POST: TabellaColtivazioneLavoro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTabellaColtivazioneLavoro,LavoroId,ColtivazioneId")] TabellaColtivazioneLavoro tabellaColtivazioneLavoro)
        {
            ModelState.Remove("Lavoro");
            ModelState.Remove("Coltivazione");
            if (ModelState.IsValid)
            {
                _context.Add(tabellaColtivazioneLavoro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var coltivazioniNonTerminate = _context.Coltivazione.Where(c => !c.ColtivazioneTerminata).ToList();
            ViewData["ColtivazioneId"] = new SelectList(coltivazioniNonTerminate, "IdColtivazione", "IdColtivazione", tabellaColtivazioneLavoro.ColtivazioneId);
            ViewData["LavoroId"] = new SelectList(_context.Lavoro, "IdLavoro", "IdLavoro", tabellaColtivazioneLavoro.LavoroId);
            return View(tabellaColtivazioneLavoro);
        }

        // GET: TabellaColtivazioneLavoro/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TabellaColtivazioneLavoro == null)
            {
                return NotFound();
            }

            var tabellaColtivazioneLavoro = await _context.TabellaColtivazioneLavoro.FindAsync(id);
            if (tabellaColtivazioneLavoro == null)
            {
                return NotFound();
            }
            ViewData["ColtivazioneId"] = new SelectList(_context.Coltivazione, "IdColtivazione", "NomeColtivazione", tabellaColtivazioneLavoro.ColtivazioneId);
            ViewData["LavoroId"] = new SelectList(_context.Lavoro, "IdLavoro", "Descrizione", tabellaColtivazioneLavoro.LavoroId);
            return View(tabellaColtivazioneLavoro);
        }

        // POST: TabellaColtivazioneLavoro/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTabellaColtivazioneLavoro,LavoroId,ColtivazioneId")] TabellaColtivazioneLavoro tabellaColtivazioneLavoro)
        {
            if (id != tabellaColtivazioneLavoro.IdTabellaColtivazioneLavoro)
            {
                return NotFound();
            }
            ModelState.Remove("Lavoro");
            ModelState.Remove("Coltivazione");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tabellaColtivazioneLavoro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TabellaColtivazioneLavoroExists(tabellaColtivazioneLavoro.IdTabellaColtivazioneLavoro))
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
            ViewData["ColtivazioneId"] = new SelectList(_context.Coltivazione, "IdColtivazione", "IdColtivazione", tabellaColtivazioneLavoro.ColtivazioneId);
            ViewData["LavoroId"] = new SelectList(_context.Lavoro, "IdLavoro", "IdLavoro", tabellaColtivazioneLavoro.LavoroId);
            return View(tabellaColtivazioneLavoro);
        }

        // GET: TabellaColtivazioneLavoro/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TabellaColtivazioneLavoro == null)
            {
                return NotFound();
            }

            var tabellaColtivazioneLavoro = await _context.TabellaColtivazioneLavoro
                .Include(t => t.Coltivazione)
                .Include(t => t.Lavoro)
                .FirstOrDefaultAsync(m => m.IdTabellaColtivazioneLavoro == id);
            if (tabellaColtivazioneLavoro == null)
            {
                return NotFound();
            }

            return View(tabellaColtivazioneLavoro);
        }

        // POST: TabellaColtivazioneLavoro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TabellaColtivazioneLavoro == null)
            {
                return Problem("Entity set 'GestioneTerreniAgricoliContext.TabellaColtivazioneLavoro'  is null.");
            }
            var tabellaColtivazioneLavoro = await _context.TabellaColtivazioneLavoro.FindAsync(id);
            if (tabellaColtivazioneLavoro != null)
            {
                _context.TabellaColtivazioneLavoro.Remove(tabellaColtivazioneLavoro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TabellaColtivazioneLavoroExists(int id)
        {
            return (_context.TabellaColtivazioneLavoro?.Any(e => e.IdTabellaColtivazioneLavoro == id)).GetValueOrDefault();
        }
    }
}
