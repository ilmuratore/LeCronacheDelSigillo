using System;
using System.Collections.Generic;
using System.IO;

// La partita conserva lo stato runtime; il catalogo conserva i dati statici.
class Partita
{
    public Giocatore Eroe;
    public List<Missione> Missioni;
    public Catalogo Catalogo;

    public Partita(Giocatore giocatore, Catalogo catalogo)
    {
        Eroe = giocatore;
        Catalogo = catalogo;
        Missioni = catalogo.CreaMissioni();
    }
    public Missione TrovaMissione(string id)
    {
        foreach (Missione m in Missioni) if (m.LeggiId() == id) return m;
        throw new InvalidDataException("Missione non trovata in partita: " + id);
    }
    public DatiPartita EsportaDati()
    {
        DatiPartita dati = new DatiPartita();
        dati.Giocatore = Eroe.EsportaDati();
        foreach (Missione m in Missioni) dati.Missioni.Add(m.EsportaDati());
        return dati;
    }
    public static Partita DaDati(DatiPartita dati, Catalogo catalogo)
    {
        if (dati == null || dati.Versione != 1 || dati.Missioni == null)
            throw new InvalidDataException("Versione o struttura salvataggio non supportata.");
        Partita partita = new Partita(Giocatore.DaDati(dati.Giocatore, catalogo), catalogo);
        HashSet<string> visti = new HashSet<string>();
        foreach (DatiMissione stato in dati.Missioni)
        {
            if (stato == null || !visti.Add(stato.Id))
                throw new InvalidDataException("Stato missione duplicato o vuoto.");
            partita.TrovaMissione(stato.Id).Ripristina(stato);
        }
        // Le nuove missioni aggiunte dopo un salvataggio partono da NonIniziata.
        foreach (Missione missione in partita.Missioni)
        {
            if (missione.LeggiStato() != StatoMissione.NonIniziata &&
                !missione.Sbloccata(partita.Missioni))
                throw new InvalidDataException("Prerequisito incoerente: " + missione.LeggiId());
        }
        return partita;
    }
}
