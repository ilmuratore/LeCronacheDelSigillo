using System;
using System.IO;
using System.Text.Json;

class ServizioSalvataggi
{
    // Lavorando dalla directory del progetto: Salvataggi/partita.json.
    public static string Percorso()
    {
        return Path.Combine(Environment.CurrentDirectory, "Salvataggi", "partita.json");
    }
    public static bool Salva(Partita partita)
    {
        string tmp = Percorso() + ".tmp";
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Percorso())!);
            string json = JsonSerializer.Serialize(partita.EsportaDati(), JsonSupport.Opzioni());
            File.WriteAllText(tmp, json);
            File.Move(tmp, Percorso(), true);
            Console.WriteLine("Partita salvata in: " + Percorso());
            return true;
        }
        catch (IOException ex) { Console.WriteLine("Impossibile salvare: " + ex.Message); }
        catch (UnauthorizedAccessException ex) { Console.WriteLine("Accesso negato: " + ex.Message); }
        return false;
    }
    public static Partita? Carica(Catalogo catalogo)
    {
        try
        {
            if (!File.Exists(Percorso()))
            {
                Console.WriteLine("Nessuna partita salvata in: " + Percorso());
                return null;
            }
            DatiPartita? dati = JsonSerializer.Deserialize<DatiPartita>(
                File.ReadAllText(Percorso()), JsonSupport.Opzioni());
            if (dati == null) throw new InvalidDataException("File JSON vuoto.");
            Partita partita = Partita.DaDati(dati, catalogo);
            Console.WriteLine("Partita caricata: " + partita.Eroe.LeggiNome());
            return partita;
        }
        catch (IOException ex) { Console.WriteLine("Lettura fallita: " + ex.Message); }
        catch (UnauthorizedAccessException ex) { Console.WriteLine("Accesso negato: " + ex.Message); }
        catch (JsonException ex) { Console.WriteLine("JSON del salvataggio non valido: " + ex.Message); }
        catch (InvalidDataException ex) { Console.WriteLine("Salvataggio non valido: " + ex.Message); }
        return null;
    }
}
