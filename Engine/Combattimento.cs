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
            bool giocatoreStordito = giocatore.IniziaTurno();
            if (!giocatore.EInVita())
            {
                break;
            }

            Console.WriteLine("\n === TURNO DEL GIOCATORE === ");
            giocatore.MostraStato();
            nemico.MostraStato();

            if (!giocatoreStordito)
            {
                bool azioneCompletata = false;
                while (!azioneCompletata)
                {
                    Console.WriteLine("\n1. Attacca");
                    Console.WriteLine("2. Difenditi");
                    Console.WriteLine("3. Abilitá speciale");
                    Console.WriteLine("4. Inventario");
                    Console.WriteLine("Scelta: ");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            giocatore.Attacca(nemico, casuale);
                            azioneCompletata = true;
                            break;
                        case "2":
                            giocatore.Difenditi();
                            azioneCompletata = true;
                            break;
                        case "3":
                            azioneCompletata = ScegliAbilita();
                            break;
                        case "4":
                            azioneCompletata = ScegliOggetto();
                            break;
                        default:
                            Console.WriteLine("Azione non valida.");
                            continue;
                    }
                }
            }

            bool nemicoStordito = nemico.IniziaTurno();
            if (!nemico.EInVita())
            {
                break;
            }
            if (!nemicoStordito)
            {
                Console.WriteLine("\n === TURNO DEL NEMICO ===");
                nemico.EseguiTurno(giocatore, casuale);
            }
        }

        bool vittoria = giocatore.EInVita();
        giocatore.TerminaCombattimento();
        nemico.TerminaCombattimento();
        return vittoria;
    }


    private bool ScegliAbilita()
    {
        giocatore.MostraAbilita();
        Console.WriteLine("0. Torna alle azioni");
        Console.WriteLine("Scelta abilita: ");

        switch(Console.ReadLine())
        {
            case "1":
                return giocatore.UsaAbilita(1, nemico, casuale);
            case "2":
                return giocatore.UsaAbilita(2, nemico, casuale);
            case "0":
                Console.WriteLine("Torni indietro al menu delle azioni.");
                return false;
            default:
                Console.WriteLine($"Scelta non valida.");
                return false;

        }
    }

    private bool ScegliOggetto()
    {
        giocatore.MostraInventario();
        Console.WriteLine("0. Torna alle azioni");
        Console.WriteLine("Scelta oggetto: ");

        string? risposta = Console.ReadLine();
        if(!int.TryParse(risposta, out int numero))
        {
            Console.WriteLine("Devi inserire un numero.");
            return false;
        }
        if( numero == 0)
        {
            Console.WriteLine("Torni al menu precedente.");
            return false;
        }
        return giocatore.UsaOggetto(numero, nemico);
    }
}