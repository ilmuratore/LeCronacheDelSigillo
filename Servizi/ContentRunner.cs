using System;
using System.IO;
using System.Text.Json;

// Runner di contenuti: legge JSON reali e verifica i collegamenti, senza generare codice.
class ContentRunner
{
    public static int Esegui(string comando)
    {
        try
        {
            Catalogo c = Catalogo.Carica();
            if (comando == "--validate")
            {
                Console.WriteLine("VALIDAZIONE OK");
                Console.WriteLine(c.Missioni.Count + " missioni; " + c.Nemici.Count +
                                  " nemici; " + c.Dialoghi.Count + " dialoghi; " +
                                  c.Oggetti.Count + " oggetti; " + c.Abilita.Count + " abilita.");
                return 0;
            }
            if (comando == "--list")
            {
                Console.WriteLine("=== MISSIONI ===");
                foreach (DefinizioneMissione m in c.Missioni)
                    Console.WriteLine(m.Id + " | " + m.Titolo + " | " + m.NemicoId);
                Console.WriteLine("=== NEMICI ===");
                foreach (DefinizioneNemico n in c.Nemici)
                    Console.WriteLine(n.Id + " | " + n.Nome + " | HP " + n.HP);
                Console.WriteLine("=== NPC ===");
                foreach (DefinizioneNPC n in c.NPC)
                    Console.WriteLine(n.Id + " | " + n.Nome);
                return 0;
            }
            if (comando == "--demo")
            {
                Console.WriteLine("Creati dinamicamente dai JSON:");
                foreach (DefinizioneMissione m in c.Missioni)
                {
                    Nemico n = c.CreaNemico(m.NemicoId);
                    Console.WriteLine(m.Titolo + " -> " + n.LeggiNome());
                    foreach (string linea in c.DialoghiNPC(m.NPCId, StatoMissione.NonIniziata))
                        Console.WriteLine(c.TrovaNPC(m.NPCId).Nome + ": " + linea);
                }
                return 0;
            }
            Console.WriteLine("Comando non riconosciuto. Usa --validate, --list o --demo.");
            return 2;
        }
        catch (Exception ex) when (ex is IOException || ex is JsonException ||
                                   ex is InvalidDataException || ex is UnauthorizedAccessException)
        {
            Console.Error.WriteLine("ERRORE CONTENUTI: " + ex.Message);
            return 2;
        }
    }
}
