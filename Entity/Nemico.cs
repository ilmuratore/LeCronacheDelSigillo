using System;

class Nemico : Personaggio
{
    public Nemico(string nome, int vitaMassima, int manaMassimo, int attacco, int difesa, 
                    int armatura, int magia, int agilita) 
        : base(nome, vitaMassima, manaMassimo, attacco, difesa, armatura, magia, agilita)
    {
        vita = vitaMassima;
        mana = manaMassimo;
    }
                                                                                                                                            

    public void Presentati()
    {
        Console.WriteLine($"\n Un nemico appare: {this.nome} !");
        Console.WriteLine("Si prepara a combattere");
    }

    public void EseguiTurno(Giocatore giocatore, Random casuale)
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