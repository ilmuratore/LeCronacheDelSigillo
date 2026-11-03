using System;

class Combattimento
{
    private Giocatore giocatore;
    private Nemico nemico;

    private Random casuale;

    public Combattimento(Giocatore giocatore, Nemico nemico)
    {
        this.giocatore = giocatore;
        this.nemico = nemico;
        casuale = new Random();
    }

    public bool Inizia()
    {
        nemico.Presentati();

        while (giocatore.EInVita() && nemico.EInVita())
        {
            Console.WriteLine("\n === TURNO DEL GIOCATORE === ");
            giocatore.MostraStato();
            nemico.MostraStato();

            Console.WriteLine("\n1. Attacca");
            Console.WriteLine("2. Difenditi");
            Console.WriteLine("Scelta: ");

            switch (Console.ReadLine())
            {
                case "1":
                    giocatore.Attacca(nemico, casuale);
                    break;
                case "2":
                    giocatore.Difenditi();
                    break;
                default:
                    Console.WriteLine("Azione non valida.");
                    continue;
            }

            if (nemico.EInVita())
            {
                Console.WriteLine("\n === TURNO DEL NEMICO ===");
                nemico.EseguiTurno(giocatore, casuale);
            }
        }

        if (giocatore.EInVita())
        {
            Console.WriteLine("\n Il giocatore ha vinto il combattimento !");
            return true;
        }
        Console.WriteLine("\n Giocatore sconfitto");
        return false;
    }
}