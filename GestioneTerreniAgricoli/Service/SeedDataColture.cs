using GestioneTerreniAgricoli.Data;
using GestioneTerreniAgricoli.Models;
using Microsoft.EntityFrameworkCore;

namespace GestioneTerreniAgricoli.Service
{
    public static class SeedDataColture
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new GestioneTerreniAgricoliContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<GestioneTerreniAgricoliContext>>()))
            {
                // Verifica se esistono già colture nel database
                if (context.Coltura.Any())
                {
                    return;   // Il database è già stato inizializzato
                }

                // Aggiunta di alcune colture al database
                context.Coltura.AddRange(
                    new Coltura
                    {
                        NomeColtura = "DKC3434",
                        Descrizione = "Ibrido precocissimo a duplice attitudine, che combina un’ottima produzione di amido per un trinciato precoce da biogas o alimentazione zootecnica"
                    },
                    new Coltura
                    {
                        NomeColtura = "DKC4231",
                        Descrizione = "Ibrido rustico e adattabile, caratterizzato da una buona stabilità produttiva in tutti i contesti anche più sfidanti"
                    },
                    new Coltura
                    {
                        NomeColtura = "DKC5432",
                        Descrizione = "Ibrido duplice attitudine caratterizzato da una granella sana e colorata, che lo rende ideale per la filiera alimentare sia di primo che secondo raccolto"
                    },
                    new Coltura
                    {
                        NomeColtura = "DKC6131",
                        Descrizione = "Ibrido altamente performante in tutti gli areali maidicoli, si caratterizza per l’elevata sanità della granella destinata al consumo alimentare"
                    },
                    new Coltura
                    {
                        NomeColtura = "SY Bambus",
                        Descrizione = "PRODUZIONI da record, massima STABILITÀ e QUALITÀ al top:SY Bambus è l’ibrido che fa per te."
                    },
                    new Coltura
                    {
                        NomeColtura = "SY Fontero",
                        Descrizione = "Eccezionale vigore per ELEVATE RESE E ALTA QUALITÀ negli ambienti fertili"
                    },
                    new Coltura
                    {
                        NomeColtura = "",
                        Descrizione = ""
                    },
                    new Coltura
                    {
                        NomeColtura = "",
                        Descrizione = ""
                    }
                );

                context.SaveChanges();
            }
        }
    }
}