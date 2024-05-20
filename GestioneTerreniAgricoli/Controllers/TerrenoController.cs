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
    public class TerrenoController : Controller
    {
        private readonly GestioneTerreniAgricoliContext _context;

        public TerrenoController(GestioneTerreniAgricoliContext context)
        {
            _context = context;
        }

        // GET: Terreno
        public async Task<IActionResult> Index(string searchString, string searchAttribute, decimal? minMetratura, decimal? maxMetratura)
        {
            if (_context.Terreno == null)
            {
                return Problem("Entity set 'Terreno' is null.");
            }

            var terreni = _context.Terreno.Include(t => t.Localita).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                if (!String.IsNullOrEmpty(searchAttribute))
                {
                    if (searchAttribute == "NomeTerreno")
                    {
                        terreni = terreni.Where(t => t.NomeTerreno.Contains(searchString));
                    }
                    else if (searchAttribute == "TipologiaTerreno")
                    {
                        terreni = terreni.Where(t => t.TipologiaTerreno.Contains(searchString));
                    }
                    else if (searchAttribute == "FullAddress")
                    {
                        terreni = terreni.Where(t => (t.Localita.Address + " - Numero Appezzamento: " + t.Localita.NumeroAppezzamento.ToString()).Contains(searchString));
                    }
                }
                
            }
            else
            {
                // Filtra per metratura minima e massima se i valori sono presenti
                if (minMetratura != null && maxMetratura != null)
                {
                    terreni = terreni.Where(t => t.Metratura >= minMetratura && t.Metratura <= maxMetratura);
                }
            }

            var terreniList = await terreni.ToListAsync();
            return View(terreniList);
        }



        // GET: Terreno/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Terreno == null)
            {
                return NotFound();
            }

            var terreno = await _context.Terreno
                .Include(t => t.Localita)
                .FirstOrDefaultAsync(m => m.IdTerreno == id);
            if (terreno == null)
            {
                return NotFound();
            }

            return View(terreno);
        }

        // GET: Terreno/Create
        public IActionResult Create()
        {
            // Ottieni le località non associate a nessun terreno
            var localitaNonAssociate = _context.Localita
                .Where(localita => !_context.Terreno.Any(terreno => terreno.LocalitaId == localita.IdLocalita))
                .ToList();

            // Controlla se ci sono località non associate
            if (localitaNonAssociate.Count == 0)
            {
                // Non ci sono località non associate
                ViewData["Message"] = "Tutte le località inserite sono già assegnate a dei terreni.";
            }
            else
            {
                // Ci sono località non associate
                ViewData["LocalitaId"] = new SelectList(localitaNonAssociate, "IdLocalita", "FullAddress");
            }

            return View();
        }


        // POST: Terreno/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTerreno,NomeTerreno,TipologiaTerreno,Metratura,LocalitaId")] Terreno terreno)
        {
            ModelState.Remove("Localita");
            ModelState.Remove("Coltivazioni");
            if (ModelState.IsValid)
            {
                _context.Add(terreno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocalitaId"] = new SelectList(_context.Localita, "IdLocalita", "IdLocalita", terreno.LocalitaId);
            return View(terreno);
        }

        // GET: Terreno/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Terreno == null)
            {
                return NotFound();
            }

            var terreno = await _context.Terreno.FindAsync(id);
            if (terreno == null)
            {
                return NotFound();
            }

            // Ottieni l'Id della località attualmente associata al terreno
            var localitaCorrenteId = terreno.LocalitaId;

            // Filtra le località per includere solo quelle non associate a un terreno,
            // oppure includi la località attualmente associata al terreno che stai modificando
            var localitaLibere = await _context.Localita
                .Where(localita => localita.IdLocalita == localitaCorrenteId || !_context.Terreno.Any(terreno => terreno.LocalitaId == localita.IdLocalita))
                .ToListAsync();

            // Assegna la SelectList delle località libere e la località corrente
            ViewData["LocalitaId"] = new SelectList(localitaLibere, "IdLocalita", "FullAddress", localitaCorrenteId);

            return View(terreno);
        }


        // POST: Terreno/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTerreno,NomeTerreno,TipologiaTerreno,Metratura,LocalitaId")] Terreno terreno)
        {
            if (id != terreno.IdTerreno)
            {
                return NotFound();
            }
            ModelState.Remove("Localita");
            ModelState.Remove("Coltivazioni");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(terreno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TerrenoExists(terreno.IdTerreno))
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
            ViewData["LocalitaId"] = new SelectList(_context.Localita, "IdLocalita", "IdLocalita", terreno.LocalitaId);
            return View(terreno);
        }

        // GET: Terreno/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Terreno == null)
            {
                return NotFound();
            }

            // Ottieni il terreno con la località e le coltivazioni associate
            var terreno = await _context.Terreno
                .Include(t => t.Localita)
                .Include(t => t.Coltivazioni)  // Include le coltivazioni associate
                .FirstOrDefaultAsync(m => m.IdTerreno == id);

            if (terreno == null)
            {
                return NotFound();
            }

            // Verifica se ci sono coltivazioni associate
            if (terreno.Coltivazioni.Any())
            {
                // Mostra un messaggio di avviso nella vista se ci sono coltivazioni associate
                ViewData["AlertMessage"] = "Ci sono coltivazioni associate a questo terreno. Sei sicuro di volerlo eliminare?";
            }

            return View(terreno);
        }

        // POST: Terreno/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Ottieni il terreno da eliminare
            var terreno = await _context.Terreno
                .Include(t => t.Coltivazioni)  // Include le coltivazioni associate
                .FirstOrDefaultAsync(t => t.IdTerreno == id);

            if (terreno == null)
            {
                return NotFound();
            }

            // Rimuovi le coltivazioni associate al terreno
            foreach (var coltivazione in terreno.Coltivazioni)
            {
                _context.Coltivazione.Remove(coltivazione);
            }

            // Rimuovi il terreno
            _context.Terreno.Remove(terreno);

            // Salva i cambiamenti
            await _context.SaveChangesAsync();

            // Reindirizza alla pagina di elenco
            return RedirectToAction(nameof(Index));
        }

        private bool TerrenoExists(int id)
        {
            return (_context.Terreno?.Any(e => e.IdTerreno == id)).GetValueOrDefault();
        }
    }
}
