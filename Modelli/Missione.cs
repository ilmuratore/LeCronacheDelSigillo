using System;
using System.Collections.Generic;
using System.IO;

// Ogni istanza ha un proprio progresso: la definizione JSON rimane immutabile.
class Missione
{
    private DefinizioneMissione dati;
    private int progresso;
    private StatoMissione stato;

    public Missione(DefinizioneMissione definizione)
    {
        dati = definizione;
        progresso = 0;
        stato = StatoMissione.NonIniziata;
    }
    public string LeggiId() { return dati.Id; }
    public string LeggiTitolo() { return dati.Titolo; }
    public string LeggiNpcId() { return dati.NPCId; }
    public string LeggiNemicoId() { return dati.NemicoId; }
    public string LeggiCategoria() { return dati.Categoria; }
    public string LeggiPrerequisito() { return dati.PrerequisitoId; }
    public StatoMissione LeggiStato() { return stato; }
    public int LeggiProgresso() { return progresso; }
    public int LeggiQuantita() { return dati.Quantita; }

    public bool Sbloccata(List<Missione> missioni)
    {
        if (dati.PrerequisitoId == "") return true;
        foreach (Missione m in missioni)
            if (m.LeggiId() == dati.PrerequisitoId)
                return m.LeggiStato() == StatoMissione.Completata;
        return false;
    }
    public void Mostra()
    {
        Console.WriteLine("[" + dati.Id + "] " + dati.Titolo + " - " + stato);
        Console.WriteLine("  " + dati.Descrizione);
        Console.WriteLine("  Obiettivo: " + progresso + "/" + dati.Quantita +
                          " vittorie su " + dati.NemicoId);
        Console.WriteLine("  Premio: " + dati.PremioMonete + " monete, " + dati.PremioXP + " XP");
    }
    public bool Accetta(List<Missione> missioni)
    {
        if (stato != StatoMissione.NonIniziata || !Sbloccata(missioni)) return false;
        stato = StatoMissione.InCorso;
        return true;
    }
    public void RegistraVittoria(string nemicoId)
    {
        if (stato != StatoMissione.InCorso || dati.NemicoId != nemicoId ||
            progresso >= dati.Quantita) return;
        progresso++;
        Console.WriteLine("Missione " + dati.Titolo + ": " + progresso + "/" + dati.Quantita);
    }
    public bool EProntaPerConsegna()
    {
        return stato == StatoMissione.InCorso && progresso == dati.Quantita;
    }
    public bool Consegna(Giocatore giocatore)
    {
        if (!EProntaPerConsegna()) return false;
        stato = StatoMissione.Completata;
        giocatore.RiceviMonete(dati.PremioMonete);
        giocatore.RiceviEsperienza(dati.PremioXP);
        return true;
    }
    public DatiMissione EsportaDati()
    {
        return new DatiMissione { Id = dati.Id, Stato = stato, Progresso = progresso };
    }
    public void Ripristina(DatiMissione m)
    {
        if (m == null || m.Id != dati.Id ||
            !Enum.IsDefined(typeof(StatoMissione), m.Stato) ||
            m.Progresso < 0 || m.Progresso > dati.Quantita ||
            (m.Stato == StatoMissione.NonIniziata && m.Progresso != 0) ||
            (m.Stato == StatoMissione.Completata && m.Progresso != dati.Quantita))
            throw new InvalidDataException("Salvataggio missione non valido: " + dati.Id);
        stato = m.Stato;
        progresso = m.Progresso;
    }
}
