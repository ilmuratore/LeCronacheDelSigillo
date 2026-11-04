using System;
class Missione
{
    private string titolo;
    private string descrizione;
    private string nomeNemicoObiettivo;
    private int quantitaRichiesta;
    private int progresso;
    private int premioMoneta;
    private int premioEsperienza;
    private StatoMissione stato;

    public Missione(string titolo, string descrizione, string nomeNemicoObiettivo,
        int quantitaRichiesta, int premioMoneta, int premioEsperienza)
    {
        this.titolo = titolo;
        this.descrizione = descrizione;
        this.nomeNemicoObiettivo = nomeNemicoObiettivo;
        this.quantitaRichiesta = quantitaRichiesta;
        this.premioMoneta = premioMoneta;
        this.premioEsperienza = premioEsperienza;
        progresso = 0;
        stato = StatoMissione.NonIniziata;

    }

    public StatoMissione LeggiStato()
    {
        return stato;
    }
    public string LeggiTitolo()
    {
        return titolo;
    }
    public string LeggiDescrizione()
    {
        return descrizione;
    }
    public string LeggiObiettivo()
    {
        return $"Sconfiggi {quantitaRichiesta} / {nomeNemicoObiettivo}";
    }
    public void Mostra()
    {
        Console.WriteLine($"\n === MISSIONE {titolo} ===");
        Console.WriteLine($"{descrizione}");
        Console.WriteLine($"Stato: {stato}");
        Console.WriteLine($"Obiettivo: sconfiggi {quantitaRichiesta} {nomeNemicoObiettivo}");
        Console.WriteLine($"Progresso: {progresso} / {quantitaRichiesta}");
        Console.WriteLine($"Premio: {premioMoneta} monete e {premioEsperienza} XP");
        if (EProntaPerConsegna())
        {
            Console.WriteLine($"Obiettivo raggiunto! Torna dal capitano Aldren per riscuotere il premio");
        }
    }
    public bool Accetta()
    {
        if(stato != StatoMissione.NonIniziata)
        {
            return false;
        }
        stato = StatoMissione.InCorso;
        Console.WriteLine($"Missione accettata : {titolo}");
        return true;
    }
    public void RegistraVittoria(string nomeNemico)
    {
        if(stato != StatoMissione.InCorso || nomeNemico != nomeNemicoObiettivo || progresso >= quantitaRichiesta)
        {
            return;
        }

        progresso++;
        Console.WriteLine($"Progresso missione: {progresso} / {quantitaRichiesta}");
        if (EProntaPerConsegna())
        {
            Console.WriteLine($"Obiettivo completato: torna dal capitano Aldren");
        }
    }
    public bool EProntaPerConsegna()
    {
        return stato == StatoMissione.InCorso && progresso >= quantitaRichiesta;
    }
    public bool Consegna(Giocatore giocatore)
    {
        if (!EProntaPerConsegna())
        {
            return false;
        }

        stato = StatoMissione.Completata;
        Console.WriteLine($"Missione completata {titolo}");
        giocatore.RiceviMonete(premioMoneta);
        giocatore.RiceviEsperienza(premioEsperienza);
        return true;
    }
}