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
    public class LocalitaController : Controller
    {
        private readonly GestioneTerreniAgricoliContext _context;

        public LocalitaController(GestioneTerreniAgricoliContext context)
        {
            _context = context;
        }

        // GET: Localita
        // GET: Localita
        public async Task<IActionResult> Index(string searchString, string searchAttribute, decimal? minLatitude, decimal? maxLatitude, decimal? minLongitude, decimal? maxLongitude)
        {
            var localita = _context.Localita.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                if (!string.IsNullOrEmpty(searchAttribute))
                {
                    if (searchAttribute == "CAP")
                    {
                        localita = localita.Where(l => l.CAP.Contains(searchString));
                    }
                    else if (searchAttribute == "NomeComune")
                    {
                        localita = localita.Where(l => l.NomeComune.Contains(searchString));
                    }
                    else if (searchAttribute == "Address")
                    {
                        localita = localita.Where(l => l.Address.Contains(searchString));
                    }
                    else if (searchAttribute == "NumeroAppezzamento")
                    {
                        localita = localita.Where(l => l.NumeroAppezzamento.ToString().Contains(searchString));
                    }
                    // Aggiungi altre clausole 'else if' per altri attributi, se necessario
                }
                
            }

            // Filtra per latitudine minima se il valore è presente
            if (minLatitude != null)
            {
                localita = localita.Where(l => l.Latitude >= minLatitude);
            }

            // Filtra per latitudine massima se il valore è presente
            if (maxLatitude != null)
            {
                localita = localita.Where(l => l.Latitude <= maxLatitude);
            }

            // Filtra per longitudine minima se il valore è presente
            if (minLongitude != null)
            {
                localita = localita.Where(l => l.Longitude >= minLongitude);
            }

            // Filtra per longitudine massima se il valore è presente
            if (maxLongitude != null)
            {
                localita = localita.Where(l => l.Longitude <= maxLongitude);
            }

            // Esegui la query e restituisci i risultati alla vista
            var localitaList = await localita.ToListAsync();
            return View(localitaList);
        }

        // GET: Localita/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Localita == null)
            {
                return NotFound();
            }

            var localita = await _context.Localita
                .FirstOrDefaultAsync(m => m.IdLocalita == id);
            if (localita == null)
            {
                return NotFound();
            }

            return View(localita);
        }

        // GET: Localita/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Localita/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLocalita,CAP,NomeComune,Address,NumeroAppezzamento,Latitude,Longitude")] Localita localita)
        {
            ModelState.Remove("Terreno");
            if (ModelState.IsValid)
            {
                _context.Add(localita);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(localita);
        }

        // GET: Localita/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Localita == null)
            {
                return NotFound();
            }

            var localita = await _context.Localita.FindAsync(id);
            if (localita == null)
            {
                return NotFound();
            }
            return View(localita);
        }

        // POST: Localita/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdLocalita,CAP,NomeComune,Address,NumeroAppezzamento,Latitude,Longitude")] Localita localita)
        {
            if (id != localita.IdLocalita)
            {
                return NotFound();
            }
            ModelState.Remove("Terreno");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(localita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocalitaExists(localita.IdLocalita))
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
            return View(localita);
        }

        // GET: Localita/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Localita == null)
            {
                return NotFound();
            }

            var localita = await _context.Localita.FirstOrDefaultAsync(m => m.IdLocalita == id);
            if (localita == null)
            {
                return NotFound();
            }

            // Verifica se ci sono terreni associati alla località
            bool hasTerreni = await _context.Terreno.AnyAsync(t => t.LocalitaId == id);

            if (hasTerreni)
            {
                // Mostra un messaggio di avviso nella vista
                ViewData["AlertMessage"] = "Attenzione: ci sono terreni associati a questa località. Sei sicuro di volerla eliminare?";
            }

            return View(localita);
        }

        // POST: Localita/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Trova la località da eliminare
            var localita = await _context.Localita.FindAsync(id);

            if (localita == null)
            {
                return NotFound();
            }

            // Trova i terreni associati alla località
            var terreniAssociati = await _context.Terreno.Where(t => t.LocalitaId == id).ToListAsync();

            foreach (var terreno in terreniAssociati)
            {
                // Trova le coltivazioni associate a ogni terreno
                var coltivazioniAssociati = await _context.Coltivazione.Where(c => c.TerrenoId == terreno.IdTerreno).ToListAsync();

                // Rimuovi o dissocia le coltivazioni associate al terreno
                foreach (var coltivazione in coltivazioniAssociati)
                {
                    _context.Coltivazione.Remove(coltivazione);
                }

                // Rimuovi il terreno
                _context.Terreno.Remove(terreno);
            }

            // Elimina la località
            _context.Localita.Remove(localita);

            // Salva i cambiamenti
            await _context.SaveChangesAsync();

            // Reindirizza alla pagina di elenco
            return RedirectToAction(nameof(Index));
        }

        private bool LocalitaExists(int id)
        {
            return (_context.Localita?.Any(e => e.IdLocalita == id)).GetValueOrDefault();
        }
    }
}
