using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestioneTerreniAgricoli.Data;
using GestioneTerreniAgricoli.Models;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace GestioneTerreniAgricoli
{
    [Authorize]
    public class LavoratoreController : Controller
    {
        private readonly GestioneTerreniAgricoliContext _context;

        public LavoratoreController(GestioneTerreniAgricoliContext context)
        {
            _context = context;
        }

        // GET: Lavoratore
        public async Task<IActionResult> Index(string searchString, string searchAttribute/*, DateTime? searchDate, DateTime? searchDateRangeStart, DateTime? searchDateRangeEnd*/)
        {
            if (_context.Lavoratore == null)
            {
                return Problem("Entity set 'Lavoratore' is null.");
            }

            var lavoratori = _context.Lavoratore.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                if (!String.IsNullOrEmpty(searchAttribute))
                {
                    switch (searchAttribute)
                    {
                        case "CodiceFiscale":
                            lavoratori = lavoratori.Where(s => s.CodiceFiscale.Contains(searchString));
                            break;
                        case "Ruolo":
                            lavoratori = lavoratori.Where(s => s.Ruolo.Contains(searchString));
                            break;
                        case "Nome":
                            lavoratori = lavoratori.Where(s => s.Nome.Contains(searchString));
                            break;
                        case "Cognome":
                            lavoratori = lavoratori.Where(s => s.Cognome.Contains(searchString));
                            break;
                        //case "DataNascita":
                        //    if (searchDate != null)
                        //    {
                                
                        //    }
                        //    break;
                        //case "RangeDateNascita":
                        //    if (searchDateRangeStart != null && searchDateRangeEnd != null)
                        //    {
                                
                        //    }
                        //    break;
                        default:
                            
                            break;
                    }
                }
                
            }

            return View(await lavoratori.ToListAsync());
        }

        public async Task<IActionResult> OttieniLavoratoriLavoro(int? id)
        {
            // Verifica se l'ID è nullo
            if (id == null)
            {
                return BadRequest("ID non valido.");
            }

            // Verifica se il contesto è nullo
            if (_context.TabellaLavoroLavoratore == null)
            {
                return Problem("Entity set 'TabellaLavoroLavoratore' è null.");
            }

            // Query per ottenere i lavoratori associati al lavoro specificato
            var lavoratori = await _context.TabellaLavoroLavoratore
                .Where(t => t.LavoroId == id)
                .Include(t => t.Lavoratore)
                .Select(t => t.Lavoratore)
                .Distinct()
                .ToListAsync();

            // Verifica se ci sono lavoratori associati al lavoro specificato
            if (lavoratori == null || lavoratori.Count == 0)
            {
                return NotFound("Nessun lavoratore trovato per il lavoro specificato.");
            }

            // Restituisci i lavoratori alla vista
            return View(lavoratori);
        }



        // GET: Lavoratore/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lavoratore == null)
            {
                return NotFound();
            }

            var lavoratore = await _context.Lavoratore
                .FirstOrDefaultAsync(m => m.IdLavoratore == id);
            if (lavoratore == null)
            {
                return NotFound();
            }

            return View(lavoratore);
        }

        // GET: Lavoratore/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lavoratore/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLavoratore,CodiceFiscale,Ruolo,Nome,Cognome,DataNascita")] Lavoratore lavoratore)
        {
            ModelState.Remove("Lavori");
            if (ModelState.IsValid)
            {
                _context.Add(lavoratore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lavoratore);
        }

        // GET: Lavoratore/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Lavoratore == null)
            {
                return NotFound();
            }

            var lavoratore = await _context.Lavoratore.FindAsync(id);
            if (lavoratore == null)
            {
                return NotFound();
            }
            return View(lavoratore);
        }

        // POST: Lavoratore/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdLavoratore,CodiceFiscale,Ruolo,Nome,Cognome,DataNascita")] Lavoratore lavoratore)
        {
            if (id != lavoratore.IdLavoratore)
            {
                return NotFound();
            }
            ModelState.Remove("Lavori");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lavoratore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LavoratoreExists(lavoratore.IdLavoratore))
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
            return View(lavoratore);
        }

        // GET: Lavoratore/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lavoratore == null)
            {
                return NotFound();
            }

            // Ottieni il lavoratore con le relazioni associate
            var lavoratore = await _context.Lavoratore
                .Include(l => l.Lavori) // Include lavori associati (TabellaLavoroLavoratore)
                .FirstOrDefaultAsync(m => m.IdLavoratore == id);

            if (lavoratore == null)
            {
                return NotFound();
            }

            // Verifica se ci sono relazioni con lavori e imposta un avviso
            if (lavoratore.Lavori.Any())
            {
                ViewData["AlertMessage"] = "Attenzione: ci sono lavori associati a questo lavoratore.";
            }

            return View(lavoratore);
        }

        // POST: Lavoratore/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lavoratore == null)
            {
                return Problem("Entity set 'GestioneTerreniAgricoliContext.Lavoratore' is null.");
            }

            // Ottieni il lavoratore con le relazioni associate
            var lavoratore = await _context.Lavoratore
                .Include(l => l.Lavori) // Include lavori associati (TabellaLavoroLavoratore)
                .FirstOrDefaultAsync(m => m.IdLavoratore == id);

            if (lavoratore == null)
            {
                return NotFound();
            }

            // Rimuovi le relazioni con lavori associati (TabellaLavoroLavoratore)
            foreach (var lavoro in lavoratore.Lavori)
            {
                _context.TabellaLavoroLavoratore.Remove(lavoro);
            }

            // Rimuovi il lavoratore
            _context.Lavoratore.Remove(lavoratore);

            // Salva i cambiamenti
            await _context.SaveChangesAsync();

            // Reindirizza alla pagina di elenco
            return RedirectToAction(nameof(Index));
        }

        private bool LavoratoreExists(int id)
        {
          return (_context.Lavoratore?.Any(e => e.IdLavoratore == id)).GetValueOrDefault();
        }
    }
}
