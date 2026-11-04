using System;

class Nemico : Personaggio
{
    private int moneteRicompensa;
    private int esperienzaRicompensa;

    public Nemico(string nome, int vitaMassima, int manaMassimo, int attacco, int difesa, 
                    int armatura, int magia, int agilita) 
        : base(nome, vitaMassima, manaMassimo, attacco, difesa, armatura, magia, agilita)
    {
        vita = vitaMassima;
        mana = manaMassimo;
        moneteRicompensa = 12;
        esperienzaRicompensa = 20;
    }

    public Nemico(string nome, int vitaMassima, int manaMassimo, int attacco, int difesa,
        int armatura, int magia, int agilita, int moneteRicompensa, int esperienzaRicompensa)
        : base (nome, vitaMassima, manaMassimo, attacco,difesa,armatura,magia, agilita)
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
    public Oggetto? EstraiOggetti(Random casuale)
    {
        int tiro = casuale.Next(0, 100);
        if (tiro < 30)
        {
            return new Oggetto("Pozione di salute", TipoOggetto.PozioneVita, 30, 15);
        }
        if (tiro < 45)
        {
            return new Oggetto("Pozione di mana", TipoOggetto.PozioneMana, 20, 15);
        }
        if (tiro < 50)
        {
            return new Oggetto("Bomba incendiaria", TipoOggetto.BombaIncendiaria, 12, 22);
        }
        return null;
    }
    public virtual void Presentati()
    {
        Console.WriteLine($"\n Un nemico appare: {this.nome} !");
        Console.WriteLine("Si prepara a combattere");
    }
    public virtual void EseguiTurno(Giocatore giocatore, Random casuale)
    {
        if(!EInVita() || !giocatore.EInVita())
        {
            return;
        }

        int sogliaSalute = vitaMassima * 30 / 100;

        if (vita <= sogliaSalute && casuale.Next(0, 100) < 50)
        {
            Console.WriteLine($"{nome} é ferito e prova a difendersi");
            Difenditi();
        }
        else if(casuale.Next(0, 100) < 25)
        {
            Console.WriteLine($"{nome} tenta un attacco con pugnale avvelenato");
            int danno = EseguiAttacco(giocatore, casuale, attacco);
            if( danno > 0 && giocatore.EInVita())
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