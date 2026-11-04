using System;

class Nemico : Personaggio
{
    private int moneteRicompensa;
    private int esperienzaRicompensa;
    private Oggetto? dropConfigurato;
    private int probabilitaDrop;

    public Nemico(DefinizioneNemico d, Catalogo catalogo)
        : this(d.Nome, d.HP, d.MP, d.Attacco, d.Difesa, d.Armatura,
               d.Magia, d.Agilita, d.Monete, d.XP)
    {
        if (!string.IsNullOrEmpty(d.DropOggettoId))
            dropConfigurato = catalogo.CreaOggetto(d.DropOggettoId);
        probabilitaDrop = d.ProbabilitaDrop;
    }

    public Nemico(string nome, int vitaMassima, int manaMassimo,
                  int attacco, int difesa, int armatura, int magia, int agilita)
        : base(nome, vitaMassima, manaMassimo, attacco, difesa, armatura, magia, agilita)
    {
        moneteRicompensa = 12; // Ricompensa ordinaria dei nemici comuni.
        esperienzaRicompensa = 50;
    }

    // Overload: consente a un boss di assegnare ricompense differenti.
    public Nemico(string nome, int vitaMassima, int manaMassimo,
                  int attacco, int difesa, int armatura, int magia, int agilita,
                  int moneteRicompensa, int esperienzaRicompensa)
        : base(nome, vitaMassima, manaMassimo, attacco, difesa, armatura, magia, agilita)
    {
        this.moneteRicompensa = moneteRicompensa;
        this.esperienzaRicompensa = esperienzaRicompensa;
    }

    public int LeggiMoneteRicompensa()
    {
        return moneteRicompensa;
    }

    public int LeggiEsperienzaRicompensa()
    {
        return esperienzaRicompensa;
    }

    // Un solo tiro tra 0 e 99: ogni intervallo corrisponde a un esito esclusivo.
    public Oggetto? EstraiOggetto(Random casuale)
    {
        if (dropConfigurato != null)
        {
            if (casuale.Next(0, 100) < probabilitaDrop) return dropConfigurato.CreaCopia();
            return null;
        }
        int tiro = casuale.Next(0, 100);

        if (tiro < 30)
        {
            return new Oggetto("Pozione di salute", TipoOggetto.PozioneVita, 30);
        }
        if (tiro < 45)
        {
            return new Oggetto("Pozione di mana", TipoOggetto.PozioneMana, 20);
        }
        if (tiro < 50)
        {
            return new Oggetto("Bomba incendiaria", TipoOggetto.BombaIncendiaria, 12);
        }

        return null; // Nessun oggetto trovato.
    }

    // Riporta il nemico alle condizioni di partenza senza creare nuove classi.
    public virtual void PreparaRivincita()
    {
        vita = vitaMassima;
        mana = manaMassimo;
        TerminaCombattimento();
    }

    public virtual void Presentati()
    {
        Console.WriteLine("\nUn nemico appare: " + nome + "!");
        Console.WriteLine("Si prepara a combattere.");
    }

    public virtual void EseguiTurno(Giocatore giocatore, Random casuale)
    {
        if (!EInVita() || !giocatore.EInVita())
        {
            return;
        }

        // Sotto il 30% di HP il nemico ha il 50% di probabilita di difendersi.
        int sogliaSalute = vitaMassima * 30 / 100;

        if (vita <= sogliaSalute && casuale.Next(0, 100) < 50)
        {
            Console.WriteLine(nome + " e ferito e prova a proteggersi.");
            Difenditi();
        }
        else if (casuale.Next(0, 100) < 25)
        {
            Console.WriteLine(nome + " tenta un attacco con pugnale avvelenato!");
            int danno = EseguiAttacco(giocatore, casuale, attacco);
            if (danno > 0 && giocatore.EInVita())
            {
                giocatore.ApplicaEffetto(TipoEffetto.Veleno, 2);
            }
        }
        else
        {
            Attacca(giocatore, casuale);
        }
    }
}
