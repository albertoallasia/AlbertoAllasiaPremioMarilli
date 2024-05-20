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
    public class ColtivazioneController : Controller
    {
        private readonly GestioneTerreniAgricoliContext _context;

        public ColtivazioneController(GestioneTerreniAgricoliContext context)
        {
            _context = context;
        }

        // GET: Coltivazione
        public async Task<IActionResult> Index(string searchString, string searchAttribute, bool? searchBoolean, decimal? minQuantity, decimal? maxQuantity)
        {
            if (_context.Coltivazione == null)
            {
                return Problem("Entity set 'Coltivazione' is null.");
            }

            var coltivazioni = _context.Coltivazione.Include(c => c.Coltura).Include(c => c.Terreno).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                if (!String.IsNullOrEmpty(searchAttribute))
                {
                    if (searchAttribute == "NomeColtivazione")
                    {
                        coltivazioni = coltivazioni.Where(c => c.NomeColtivazione.Contains(searchString));
                    }
                    else if (searchAttribute == "QuantitaProdotta")
                    {
                        if (decimal.TryParse(searchString, out decimal quantitaProdotta))
                        {
                            coltivazioni = coltivazioni.Where(c => c.QuantitaProdotta == quantitaProdotta);
                        }
                    }
                    else if (searchAttribute == "ColtivazioneTerminata" && searchBoolean != null)
                    {
                        coltivazioni = coltivazioni.Where(c => c.ColtivazioneTerminata == searchBoolean);
                    }
                    else if (searchAttribute == "Coltura")
                    {
                        coltivazioni = coltivazioni.Where(c => c.Coltura.NomeColtura.Contains(searchString));
                    }
                    else if (searchAttribute == "Terreno")
                    {
                        coltivazioni = coltivazioni.Where(c => c.Terreno.NomeTerreno.Contains(searchString));
                    }
                    // Aggiungi altre clausole 'else if' per altri attributi, se necessario
                }
                
            }
            else
            {
                // Non c'è alcuna stringa di ricerca, quindi controlla se c'è un filtro su ColtivazioneTerminata
                if (searchAttribute == "ColtivazioneTerminata" && searchBoolean != null)
                {
                    coltivazioni = coltivazioni.Where(c => c.ColtivazioneTerminata == searchBoolean);
                }

                // Aggiungi la logica per filtrare la quantità prodotta tra minQuantity e maxQuantity
                if (minQuantity != null && maxQuantity != null)
                {
                    coltivazioni = coltivazioni.Where(c => c.QuantitaProdotta >= minQuantity && c.QuantitaProdotta <= maxQuantity);
                }
            }

            return View(await coltivazioni.ToListAsync());
        }

        public async Task<IActionResult> OttieniColtivazioniLavoro(int? id)
        {
            // Verifica se l'ID è nullo
            if (id == null)
            {
                return BadRequest("ID non valido.");
            }

            // Verifica se il contesto è nullo
            if (_context.TabellaColtivazioneLavoro == null)
            {
                return Problem("Entity set 'TabellaColtivazioneLavoro' è null.");
            }

            // Query per ottenere le coltivazioni associate al lavoro specificato
            var coltivazioni = await _context.TabellaColtivazioneLavoro
                .Where(t => t.LavoroId == id)
                .Include(t => t.Coltivazione)
                    .ThenInclude(c => c.Coltura) // Include la coltura riferita a coltivazione
                .Include(t => t.Coltivazione)
                    .ThenInclude(c => c.Terreno) // Include il terreno riferito a coltivazione
                .Select(t => t.Coltivazione)
                .Distinct()
                .ToListAsync();

            // Verifica se ci sono coltivazioni associate al lavoro specificato
            if (coltivazioni == null || coltivazioni.Count == 0)
            {
                return NotFound("Nessuna coltivazione trovata per il lavoro specificato.");
            }

            // Restituisci le coltivazioni alla vista
            return View(coltivazioni);
        }



        // GET: Coltivazione/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Coltivazione == null)
            {
                return NotFound();
            }

            var coltivazione = await _context.Coltivazione
                .Include(c => c.Coltura)
                .Include(c => c.Terreno)
                .FirstOrDefaultAsync(m => m.IdColtivazione == id);
            if (coltivazione == null)
            {
                return NotFound();
            }

            return View(coltivazione);
        }

        public async Task<IActionResult> Lavorazioni(int? id)
        {
            //return RedirectToAction("IndexLavorazioni", "TabellaColtivazioneLavoro", new { id = id });
            return RedirectToAction("OttieniLavoriColtivazione", "Lavoro", new { id = id });
        }

        // GET: Coltivazione/Create
        public IActionResult Create()
        {
            ViewData["ColturaId"] = new SelectList(_context.Set<Coltura>(), "IdColtura", "NomeColtura");
            ViewData["TerrenoId"] = new SelectList(_context.Set<Terreno>(), "IdTerreno", "NomeTerreno");
            return View();
        }

        // POST: Coltivazione/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdColtivazione,NomeColtivazione,QuantitaProdotta,ColtivazioneTerminata,ColturaId,TerrenoId")] Coltivazione coltivazione)
        {
            ModelState.Remove("Coltura");
            ModelState.Remove("Terreno");
            ModelState.Remove("Lavori");
            if (ModelState.IsValid)
            {
                _context.Add(coltivazione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ColturaId"] = new SelectList(_context.Set<Coltura>(), "IdColtura", "IdColtura", coltivazione.ColturaId);
            ViewData["TerrenoId"] = new SelectList(_context.Set<Terreno>(), "IdTerreno", "IdTerreno", coltivazione.TerrenoId);
            return View(coltivazione);
        }

        // GET: Coltivazione/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Coltivazione == null)
            {
                return NotFound();
            }

            var coltivazione = await _context.Coltivazione.FindAsync(id);
            if (coltivazione == null)
            {
                return NotFound();
            }
            ViewData["ColturaId"] = new SelectList(_context.Set<Coltura>(), "IdColtura", "NomeColtura", coltivazione.ColturaId);
            ViewData["TerrenoId"] = new SelectList(_context.Set<Terreno>(), "IdTerreno", "NomeTerreno", coltivazione.TerrenoId);
            return View(coltivazione);
        }

        // POST: Coltivazione/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdColtivazione,NomeColtivazione,QuantitaProdotta,ColtivazioneTerminata,ColturaId,TerrenoId")] Coltivazione coltivazione)
        {
            if (id != coltivazione.IdColtivazione)
            {
                return NotFound();
            }
            ModelState.Remove("Coltura");
            ModelState.Remove("Terreno");
            ModelState.Remove("Lavori");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coltivazione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColtivazioneExists(coltivazione.IdColtivazione))
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
            ViewData["ColturaId"] = new SelectList(_context.Set<Coltura>(), "IdColtura", "IdColtura", coltivazione.ColturaId);
            ViewData["TerrenoId"] = new SelectList(_context.Set<Terreno>(), "IdTerreno", "IdTerreno", coltivazione.TerrenoId);
            return View(coltivazione);
        }

        // GET: Coltivazione/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Coltivazione == null)
            {
                return NotFound();
            }

            // Ottieni la coltivazione con la coltura e il terreno associati
            var coltivazione = await _context.Coltivazione
                .Include(c => c.Coltura)
                .Include(c => c.Terreno)
                .FirstOrDefaultAsync(m => m.IdColtivazione == id);

            if (coltivazione == null)
            {
                return NotFound();
            }

            // Verifica se ci sono lavori associati alla coltivazione
            bool hasLavori = await _context.TabellaColtivazioneLavoro
                .AnyAsync(tcl => tcl.ColtivazioneId == id);

            if (hasLavori)
            {
                // Imposta un avviso nella vista se ci sono lavori associati
                ViewData["AlertMessage"] = "Attenzione: ci sono lavori associati a questa coltivazione. Per eliminare la coltivazione, è necessario eliminare prima i lavori associati.";
            }

            return View(coltivazione);
        }

        // POST: Coltivazione/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Coltivazione == null)
            {
                return Problem("Entity set 'GestioneTerreniAgricoliContext.Coltivazione' is null.");
            }

            // Ottieni la coltivazione da eliminare con le relazioni associate
            var coltivazione = await _context.Coltivazione
                .Include(c => c.Lavori) // Include le relazioni di lavori associati
                .FirstOrDefaultAsync(c => c.IdColtivazione == id);

            if (coltivazione == null)
            {
                return NotFound();
            }

            // Rimuovi le relazioni di lavori associati (TabellaColtivazioneLavoro)
            foreach (var lavoro in coltivazione.Lavori)
            {
                var relazioneLavoro = await _context.TabellaColtivazioneLavoro
                    .FirstOrDefaultAsync(tcl => tcl.ColtivazioneId == coltivazione.IdColtivazione && tcl.LavoroId == lavoro.LavoroId);

                if (relazioneLavoro != null)
                {
                    _context.TabellaColtivazioneLavoro.Remove(relazioneLavoro);
                }
            }

            // Rimuovi la coltivazione
            _context.Coltivazione.Remove(coltivazione);

            // Salva i cambiamenti
            await _context.SaveChangesAsync();

            // Reindirizza alla pagina di elenco
            return RedirectToAction(nameof(Index));
        }

        private bool ColtivazioneExists(int id)
        {
            return (_context.Coltivazione?.Any(e => e.IdColtivazione == id)).GetValueOrDefault();
        }
    }
}
