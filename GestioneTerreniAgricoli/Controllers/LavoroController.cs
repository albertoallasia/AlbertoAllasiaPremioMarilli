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
    public class LavoroController : Controller
    {
        private readonly GestioneTerreniAgricoliContext _context;

        public LavoroController(GestioneTerreniAgricoliContext context)
        {
            _context = context;
        }

        // GET: Lavoro
        public async Task<IActionResult> Index(string searchString, string searchAttribute)
        {
            if (_context.Lavoro == null)
            {
                return Problem("Entity set 'Lavoro' is null.");
            }

            IQueryable<Lavoro> lavoriQuery = _context.Lavoro.AsQueryable();

            if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(searchAttribute))
            {
                if (searchAttribute == "Descrizione")
                {
                    lavoriQuery = lavoriQuery.Where(l => l.Descrizione.Contains(searchString));
                }
                // Aggiungi altre clausole 'else if' per altri attributi, se necessario
            }

            return View(await lavoriQuery.ToListAsync());
        }

        public async Task<IActionResult> OttieniLavoriColtivazione(int? id)
        {
            // Verifica se l'ID è nullo
            if (id == null)
            {
                return BadRequest("ID non valido.");
            }

            if (_context.TabellaColtivazioneLavoro == null)
            {
                return Problem("Entity set 'TabellaColtivazioneLavoro' is null.");
            }

            // Query per ottenere i lavori associati alla coltivazione specificata
            var lavori = await _context.TabellaColtivazioneLavoro
                .Where(t => t.ColtivazioneId == id)
                .Include(t => t.Lavoro)
                .Select(t => t.Lavoro)
                .Distinct()
                .ToListAsync();

            // Verifica se ci sono lavori associati alla coltivazione specificata
            if (lavori == null || lavori.Count == 0)
            {
                return NotFound("Nessun lavoro trovato per la coltivazione specificata.");
            }

            // Restituisci i lavori alla vista
            return View(lavori);
        }


        // GET: Lavoro/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lavoro == null)
            {
                return NotFound();
            }

            var lavoro = await _context.Lavoro
                .FirstOrDefaultAsync(m => m.IdLavoro == id);
            if (lavoro == null)
            {
                return NotFound();
            }

            return View(lavoro);
        }

        public async Task<IActionResult> Lavoratori(int? id)
        {
            //return RedirectToAction("Index", "TabellaLavoroLavoratore", new { id = id });
            return RedirectToAction("OttieniLavoratoriLavoro", "Lavoratore", new { id = id });
        }

        public async Task<IActionResult> Coltivazioni(int? id)
        {
            //return RedirectToAction("Index", "TabellaColtivazioneLavoro", new { id = id });
            return RedirectToAction("OttieniColtivazioniLavoro", "Coltivazione", new { id = id });
        }

        // GET: Lavoro/Create
        public IActionResult Create()
        {
            ViewBag.Lavoratori = _context.Lavoratore.ToList();
            ViewBag.Coltivazioni = _context.Coltivazione.ToList();
            
            return View();
        }

        // POST: Lavoro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLavoro,Descrizione,DataInizioLavoro,DataFineLavoro")] Lavoro lavoro, int[] SelectedLavoratori, int[] SelectedColtivazioni)
        {
            // Rimuovi validazione per le proprietà di navigazione non necessarie
            ModelState.Remove("Lavoratori");
            ModelState.Remove("Coltivazioni");
            ModelState.Remove("Spese");

            if (ModelState.IsValid)
            {
                // Aggiungi il lavoro al contesto
                _context.Lavoro.Add(lavoro);
                await _context.SaveChangesAsync();

                // Assegna lavoratori
                if (SelectedLavoratori != null)
                {
                    foreach (int lavoratoreId in SelectedLavoratori)
                    {
                        var lavoroLavoratore = new TabellaLavoroLavoratore
                        {
                            LavoroId = lavoro.IdLavoro,
                            LavoratoreId = lavoratoreId
                        };
                        _context.TabellaLavoroLavoratore.Add(lavoroLavoratore);
                    }
                }

                // Assegna coltivazioni
                if (SelectedColtivazioni != null)
                {
                    foreach (int coltivazioneId in SelectedColtivazioni)
                    {
                        var coltivazioneLavoro = new TabellaColtivazioneLavoro
                        {
                            LavoroId = lavoro.IdLavoro,
                            ColtivazioneId = coltivazioneId
                        };
                        _context.TabellaColtivazioneLavoro.Add(coltivazioneLavoro);
                    }
                }

                // Salva le modifiche nel database
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Se il model state non è valido, restituisci la view con lo stesso model
            ViewBag.Lavoratori = _context.Lavoratore.ToList();
            ViewBag.Coltivazioni = _context.Coltivazione.ToList();
            
            return View(lavoro);
        }

        // GET: Lavoro/Edit/5
        // GET: Lavoro/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Lavoro == null)
            {
                return NotFound();
            }

            var lavoro = await _context.Lavoro
                .Include(l => l.Lavoratori)
                .ThenInclude(ll => ll.Lavoratore)
                .Include(l => l.Coltivazioni)
                .ThenInclude(cl => cl.Coltivazione)
                .FirstOrDefaultAsync(m => m.IdLavoro == id);

            if (lavoro == null)
            {
                return NotFound();
            }

            // Passa le liste di lavoratori e coltivazioni alla vista
            ViewBag.Lavoratori = _context.Lavoratore.ToList();
            ViewBag.Coltivazioni = _context.Coltivazione.ToList();

            return View(lavoro);
        }

        // POST: Lavoro/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdLavoro,Descrizione,DataInizioLavoro,DataFineLavoro")] Lavoro lavoro, int[] SelectedLavoratori, int[] SelectedColtivazioni)
        {
            if (id != lavoro.IdLavoro)
            {
                return NotFound();
            }

            ModelState.Remove("Lavoratori");
            ModelState.Remove("Coltivazioni");
            ModelState.Remove("Spese");

            if (ModelState.IsValid)
            {
                try
                {
                    // Rimuovi le associazioni esistenti di lavoratori e coltivazioni
                    _context.TabellaLavoroLavoratore.RemoveRange(_context.TabellaLavoroLavoratore.Where(l => l.LavoroId == lavoro.IdLavoro));
                    _context.TabellaColtivazioneLavoro.RemoveRange(_context.TabellaColtivazioneLavoro.Where(c => c.LavoroId == lavoro.IdLavoro));

                    // Assegna nuovi lavoratori al lavoro
                    if (SelectedLavoratori != null)
                    {
                        foreach (int lavoratoreId in SelectedLavoratori)
                        {
                            var lavoroLavoratore = new TabellaLavoroLavoratore
                            {
                                LavoroId = lavoro.IdLavoro,
                                LavoratoreId = lavoratoreId
                            };
                            _context.TabellaLavoroLavoratore.Add(lavoroLavoratore);
                        }
                    }

                    // Assegna nuove coltivazioni al lavoro
                    if (SelectedColtivazioni != null)
                    {
                        foreach (int coltivazioneId in SelectedColtivazioni)
                        {
                            var coltivazioneLavoro = new TabellaColtivazioneLavoro
                            {
                                LavoroId = lavoro.IdLavoro,
                                ColtivazioneId = coltivazioneId
                            };
                            _context.TabellaColtivazioneLavoro.Add(coltivazioneLavoro);
                        }
                    }

                    // Aggiorna il lavoro
                    _context.Update(lavoro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LavoroExists(lavoro.IdLavoro))
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

            // Passa le liste di lavoratori e coltivazioni alla vista
            ViewBag.Lavoratori = _context.Lavoratore.ToList();
            ViewBag.Coltivazioni = _context.Coltivazione.ToList();

            return View(lavoro);
        }

        // GET: Lavoro/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lavoro == null)
            {
                return NotFound();
            }

            // Ottieni il lavoro con le entità associate
            var lavoro = await _context.Lavoro
                .Include(l => l.Spese)  // Include le spese associate
                .Include(l => l.Coltivazioni)  // Include le coltivazioni associate (TabellaColtivazioneLavoro)
                .Include(l => l.Lavoratori)  // Include i lavoratori associati (TabellaLavoroLavoratore)
                .FirstOrDefaultAsync(m => m.IdLavoro == id);

            if (lavoro == null)
            {
                return NotFound();
            }

            // Inizializza un elenco di messaggi di avviso
            var alertMessages = new List<string>();

            // Verifica se ci sono spese associate
            if (lavoro.Spese.Any())
            {
                alertMessages.Add("Attenzione: ci sono spese associate a questo lavoro.");
            }

            // Verifica se ci sono coltivazioni associate
            if (lavoro.Coltivazioni.Any())
            {
                alertMessages.Add("Attenzione: ci sono coltivazioni associate a questo lavoro.");
            }

            // Verifica se ci sono lavoratori associati
            if (lavoro.Lavoratori.Any())
            {
                alertMessages.Add("Attenzione: ci sono lavoratori associati a questo lavoro.");
            }

            // Imposta l'elenco di messaggi di avviso
            if (alertMessages.Any())
            {
                ViewData["AlertMessages"] = alertMessages;
            }

            return View(lavoro);
        }

        // POST: Lavoro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lavoro == null)
            {
                return Problem("Entity set 'GestioneTerreniAgricoliContext.Lavoro' is null.");
            }

            // Ottieni il lavoro con le entità associate
            var lavoro = await _context.Lavoro
                .Include(l => l.Spese)  // Include le spese associate
                .Include(l => l.Coltivazioni)  // Include le coltivazioni associate (TabellaColtivazioneLavoro)
                .Include(l => l.Lavoratori)  // Include i lavoratori associati (TabellaLavoroLavoratore)
                .FirstOrDefaultAsync(l => l.IdLavoro == id);

            if (lavoro == null)
            {
                return NotFound();
            }

            // Rimuovi le spese associate al lavoro
            foreach (var spesa in lavoro.Spese)
            {
                _context.Spesa.Remove(spesa);
            }

            // Rimuovi le relazioni di coltivazioni associate al lavoro (TabellaColtivazioneLavoro)
            foreach (var coltivazione in lavoro.Coltivazioni)
            {
                _context.TabellaColtivazioneLavoro.Remove(coltivazione);
            }

            // Rimuovi le relazioni con i lavoratori associati (TabellaLavoroLavoratore)
            foreach (var lavoratore in lavoro.Lavoratori)
            {
                _context.TabellaLavoroLavoratore.Remove(lavoratore);
            }

            // Rimuovi il lavoro
            _context.Lavoro.Remove(lavoro);

            // Salva i cambiamenti
            await _context.SaveChangesAsync();

            // Reindirizza alla pagina di elenco
            return RedirectToAction(nameof(Index));
        }

        private bool LavoroExists(int id)
        {
          return (_context.Lavoro?.Any(e => e.IdLavoro == id)).GetValueOrDefault();
        }
    }
}
