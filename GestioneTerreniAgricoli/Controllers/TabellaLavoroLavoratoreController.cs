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

namespace GestioneTerreniAgricoli
{
    [Authorize]
    public class TabellaLavoroLavoratoreController : Controller
    {
        private readonly GestioneTerreniAgricoliContext _context;

        public TabellaLavoroLavoratoreController(GestioneTerreniAgricoliContext context)
        {
            _context = context;
        }

        // GET: TabellaLavoroLavoratore

        public async Task<IActionResult> Index(int? id, string searchString, string searchAttribute)
        {
            if (_context.TabellaLavoroLavoratore == null)
            {
                return Problem("Entity set 'TabellaLavoroLavoratore' is null.");
            }

            var tabellaLavoroLavoratoreQuery = _context.TabellaLavoroLavoratore
                                                      .Include(t => t.Lavoratore)
                                                      .Include(t => t.Lavoro)
                                                      .Where(t => t.LavoroId == id)
                                                      .AsQueryable();

            if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(searchAttribute))
            {
                if (searchAttribute == "Lavoratore")
                {
                    tabellaLavoroLavoratoreQuery = tabellaLavoroLavoratoreQuery.Where(t => (t.Lavoratore.Nome + " " + t.Lavoratore.Cognome).Contains(searchString));
                }
                else if (searchAttribute == "Lavoro")
                {
                    tabellaLavoroLavoratoreQuery = tabellaLavoroLavoratoreQuery.Where(t => t.Lavoro.Descrizione.Contains(searchString));
                }
                // Aggiungi altre clausole 'else if' per altri attributi, se necessario
            }

            return View(await tabellaLavoroLavoratoreQuery.ToListAsync());
        }
        

        // GET: TabellaLavoroLavoratore/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TabellaLavoroLavoratore == null)
            {
                return NotFound();
            }

            var tabellaLavoroLavoratore = await _context.TabellaLavoroLavoratore
                .Include(t => t.Lavoratore)
                .Include(t => t.Lavoro)
                .FirstOrDefaultAsync(m => m.IdTabellaLavoroLavoratore == id);
            if (tabellaLavoroLavoratore == null)
            {
                return NotFound();
            }

            return View(tabellaLavoroLavoratore);
        }

        // GET: TabellaLavoroLavoratore/Create
        public IActionResult Create()
        {
            
            // Crea un SelectList per LavoratoreId con i lavoratori disponibili
            ViewData["LavoratoreId"] = new SelectList(_context.Lavoratore, "IdLavoratore", "NomeCognome");

            // Crea un SelectList per LavoroId con i lavori disponibili
            ViewData["LavoroId"] = new SelectList(_context.Lavoro, "IdLavoro", "Descrizione");

            return View();
        }

        // POST: TabellaLavoroLavoratore/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTabellaLavoroLavoratore,LavoratoreId,LavoroId")] TabellaLavoroLavoratore tabellaLavoroLavoratore)
        {
            ModelState.Remove("Lavoratore");
            ModelState.Remove("Lavoro");
            if (ModelState.IsValid)
            {
                _context.Add(tabellaLavoroLavoratore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var coltivazioniNonTerminate = _context.Coltivazione
                .Where(c => !c.ColtivazioneTerminata)
                .ToList(); // Trasforma la query in una lista

            
            ViewData["LavoratoreId"] = new SelectList(_context.Lavoratore, "IdLavoratore", "IdLavoratore", tabellaLavoroLavoratore.LavoratoreId);
            ViewData["LavoroId"] = new SelectList(_context.Lavoro, "IdLavoro", "IdLavoro", tabellaLavoroLavoratore.LavoroId);
            return View(tabellaLavoroLavoratore);
        }

        // GET: TabellaLavoroLavoratore/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TabellaLavoroLavoratore == null)
            {
                return NotFound();
            }

            var tabellaLavoroLavoratore = await _context.TabellaLavoroLavoratore.FindAsync(id);
            if (tabellaLavoroLavoratore == null)
            {
                return NotFound();
            }
            ViewData["LavoratoreId"] = new SelectList(_context.Lavoratore, "IdLavoratore", "NomeCognome", tabellaLavoroLavoratore.LavoratoreId);
            ViewData["LavoroId"] = new SelectList(_context.Lavoro, "IdLavoro", "Descrizione", tabellaLavoroLavoratore.LavoroId);
            return View(tabellaLavoroLavoratore);
        }

        // POST: TabellaLavoroLavoratore/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTabellaLavoroLavoratore,LavoratoreId,LavoroId")] TabellaLavoroLavoratore tabellaLavoroLavoratore)
        {
            if (id != tabellaLavoroLavoratore.IdTabellaLavoroLavoratore)
            {
                return NotFound();
            }
            ModelState.Remove("Lavoratore");
            ModelState.Remove("Lavoro");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tabellaLavoroLavoratore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TabellaLavoroLavoratoreExists(tabellaLavoroLavoratore.IdTabellaLavoroLavoratore))
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
            ViewData["LavoratoreId"] = new SelectList(_context.Lavoratore, "IdLavoratore", "IdLavoratore", tabellaLavoroLavoratore.LavoratoreId);
            ViewData["LavoroId"] = new SelectList(_context.Lavoro, "IdLavoro", "IdLavoro", tabellaLavoroLavoratore.LavoroId);
            return View(tabellaLavoroLavoratore);
        }

        // GET: TabellaLavoroLavoratore/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TabellaLavoroLavoratore == null)
            {
                return NotFound();
            }

            var tabellaLavoroLavoratore = await _context.TabellaLavoroLavoratore
                .Include(t => t.Lavoratore)
                .Include(t => t.Lavoro)
                .FirstOrDefaultAsync(m => m.IdTabellaLavoroLavoratore == id);
            if (tabellaLavoroLavoratore == null)
            {
                return NotFound();
            }

            return View(tabellaLavoroLavoratore);
        }

        // POST: TabellaLavoroLavoratore/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TabellaLavoroLavoratore == null)
            {
                return Problem("Entity set 'GestioneTerreniAgricoliContext.TabellaLavoroLavoratore'  is null.");
            }
            var tabellaLavoroLavoratore = await _context.TabellaLavoroLavoratore.FindAsync(id);
            if (tabellaLavoroLavoratore != null)
            {
                _context.TabellaLavoroLavoratore.Remove(tabellaLavoroLavoratore);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TabellaLavoroLavoratoreExists(int id)
        {
          return (_context.TabellaLavoroLavoratore?.Any(e => e.IdTabellaLavoroLavoratore == id)).GetValueOrDefault();
        }
    }
}
