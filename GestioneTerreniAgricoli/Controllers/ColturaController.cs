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
    public class ColturaController : Controller
    {
        private readonly GestioneTerreniAgricoliContext _context;

        public ColturaController(GestioneTerreniAgricoliContext context)
        {
            _context = context;
        }

        // GET: Coltura
        public async Task<IActionResult> Index(string searchString, string searchAttribute)
        {
            if (_context.Coltura == null)
            {
                return Problem("Entity set 'Coltura' is null.");
            }

            var colture = _context.Coltura.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                if (!String.IsNullOrEmpty(searchAttribute))
                {
                    switch (searchAttribute)
                    {
                        case "NomeColtura":
                            colture = colture.Where(c => c.NomeColtura.Contains(searchString));
                            break;
                        case "Descrizione":
                            colture = colture.Where(c => c.Descrizione.Contains(searchString));
                            break;
                        default:
                            
                            break;
                    }
                }
                else
                {
                    colture = colture.Where(c =>
                        c.NomeColtura.Contains(searchString) ||
                        c.Descrizione.Contains(searchString));
                }
            }

            return View(await colture.ToListAsync());
        }

        // GET: Coltura/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Coltura == null)
            {
                return NotFound();
            }

            var coltura = await _context.Coltura
                .FirstOrDefaultAsync(m => m.IdColtura == id);
            if (coltura == null)
            {
                return NotFound();
            }

            return View(coltura);
        }

        // GET: Coltura/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Coltura/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdColtura,NomeColtura,Descrizione")] Coltura coltura)
        {
            ModelState.Remove("Coltivazioni");
            if (ModelState.IsValid)
            {
                _context.Add(coltura);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coltura);
        }

        // GET: Coltura/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Coltura == null)
            {
                return NotFound();
            }

            var coltura = await _context.Coltura.FindAsync(id);
            if (coltura == null)
            {
                return NotFound();
            }
            return View(coltura);
        }

        // POST: Coltura/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdColtura,NomeColtura,Descrizione")] Coltura coltura)
        {
            if (id != coltura.IdColtura)
            {
                return NotFound();
            }
            ModelState.Remove("Coltivazioni");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coltura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColturaExists(coltura.IdColtura))
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
            return View(coltura);
        }

        // GET: Coltura/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Coltura == null)
            {
                return NotFound();
            }

            // Ottieni la coltura con le coltivazioni associate
            var coltura = await _context.Coltura
                .Include(c => c.Coltivazioni)  // Include le coltivazioni associate
                .FirstOrDefaultAsync(m => m.IdColtura == id);

            if (coltura == null)
            {
                return NotFound();
            }

            // Verifica se ci sono coltivazioni associate
            if (coltura.Coltivazioni.Any())
            {
                // Mostra un messaggio di avviso nella vista se ci sono coltivazioni associate
                ViewData["AlertMessage"] = "Ci sono coltivazioni associate a questa coltura. Sei sicuro di volerla eliminare?";
            }

            return View(coltura);
        }

        // POST: Coltura/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Coltura == null)
            {
                return Problem("Entity set 'GestioneTerreniAgricoliContext.Coltura' is null.");
            }

            // Ottieni la coltura con le coltivazioni associate
            var coltura = await _context.Coltura
                .Include(c => c.Coltivazioni)  // Include le coltivazioni associate
                .FirstOrDefaultAsync(c => c.IdColtura == id);

            if (coltura == null)
            {
                return NotFound();
            }

            // Rimuovi o dissocia le coltivazioni associate alla coltura
            foreach (var coltivazione in coltura.Coltivazioni)
            {
                _context.Coltivazione.Remove(coltivazione);
            }

            // Rimuovi la coltura
            _context.Coltura.Remove(coltura);

            // Salva i cambiamenti
            await _context.SaveChangesAsync();

            // Reindirizza alla pagina di elenco
            return RedirectToAction(nameof(Index));
        }

        private bool ColturaExists(int id)
        {
            return (_context.Coltura?.Any(e => e.IdColtura == id)).GetValueOrDefault();
        }
    }
}
