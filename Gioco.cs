using System;
using System.Collections.Generic;
using System.Text;

class Gioco
{

    public void Avvia()
    {
        bool inEsecuzione = true;

        while (inEsecuzione)
        {
            MostraMenu();

            switch (Console.ReadLine())
            {
                case "1":
                    IniziaPartita();
                    break;
                case "2":
                    MostraIstruzioni();
                    break;
                case "0":
                    inEsecuzione = false;
                    Console.WriteLine("Chiusura del gioco.");
                    break;
                default:
                    Console.WriteLine("Scelta non valida. Riprova.\n");
                    break;
            }
        }

    }

    private void MostraMenu()
    {
        Console.WriteLine("=== LE CRONACHE DEL SIGILLO === ");
        Console.WriteLine("1. Inizia partita");
        Console.WriteLine("2. Istruzioni");
        Console.WriteLine("0. Esci");
        Console.WriteLine("Scelta: ");
    }

    private void IniziaPartita()
    {
        Console.WriteLine("\n Un antico sigillo é stato spezzato...");
        Console.WriteLine("Il viaggio di un eroe che cambierá le sorti del mondo intero ha inizio");
        Console.WriteLine("Sei pronto all'avventura ? ");
        Console.WriteLine("Prima di partire, crea il tuo eroe.");

        string nome = LeggiNome();
        TipoClasse tipoClasse = ScegliClasse();
        Giocatore giocatore = new Giocatore(nome, tipoClasse);
        DistribuisciPunti(giocatore);
        Console.WriteLine("\n === GIOCATORE CREATO ===");
        giocatore.MostraScheda();

        Console.WriteLine("\n Il viaggio nel regno inizia da un piccolo villaggio ...");

        EntraVillaggio(giocatore);

        Console.WriteLine("\n Partita terminata. Premi INVIO per tornare al menu principale");

        Console.ReadLine();
        Console.WriteLine();
      

    }

    private void MostraIstruzioni()
    {
        Console.WriteLine("\n Digita il numero dell'opzione e premi INVIO.");
        Console.WriteLine("Premi INVIO per tornare al menu.");
        Console.ReadLine();
        Console.WriteLine();
    }

    private string LeggiNome()
    {
        string nome = "";
        while (nome == "")
        {
            Console.WriteLine("Nome del personaggio: ");
            nome = "" + Console.ReadLine();
            nome = nome.Trim();

            if( nome == "")
            {
                Console.WriteLine("Il nome non puo essere vuoto.");
            }
         }

        return nome;
    }

    private TipoClasse ScegliClasse()
    {
        while (true)
        {
            Console.WriteLine(" === SCEGLI LA CLASSE GIOCATORE ===");
            Console.WriteLine("1. Guerriero");
            Console.WriteLine("2. Mago");
            Console.WriteLine("3. Ladro");
            Console.WriteLine("Scelta:  ");

            switch (Console.ReadLine())
            {
                case "1":
                    return TipoClasse.Guerriero;
                case "2":
                    return TipoClasse.Mago;
                case "3":
                    return TipoClasse.Ladro;
                default:
                    Console.WriteLine("Classe non valida. Riprova");
                    break;
            }
        }
    }

    private void DistribuisciPunti(Giocatore giocatore)
    {
        Console.WriteLine($"\n Hai {giocatore.GetPuntiDisponibili()} da distribuire. ");
        
        while(giocatore.HaPuntiDaDistribuire())
        {
            giocatore.MostraScheda();
            Console.WriteLine(" === Quale statistica vuoi aumentare ? ===");
            Console.WriteLine("1. HP massimi +10");
            Console.WriteLine("2. MP massimi +5");
            Console.WriteLine("3. Attacco +1");
            Console.WriteLine("4. Difesa +1");
            Console.WriteLine("5. Magia +1");
            Console.WriteLine("6. Agilita +1");
            Console.WriteLine("Scelta : ");

            bool assegnato = false;

            switch (Console.ReadLine())
            {
                case "1":
                    assegnato = giocatore.AssegnaPunti(1);
                    break;
                case "2":
                    assegnato = giocatore.AssegnaPunti(2);
                    break;
                case "3":
                    assegnato = giocatore.AssegnaPunti(3);
                    break;
                case "4":
                    assegnato = giocatore.AssegnaPunti(4);
                    break;
                case "5":
                    assegnato = giocatore.AssegnaPunti(5);
                    break;
                case "6":
                    assegnato = giocatore.AssegnaPunti(6);
                    break;
            }
            if (!assegnato)
            {
                Console.WriteLine("Scelta non valida. Nessun punto distribuito.");
            }
        }
    }

    private void EntraVillaggio(Giocatore giocatore)
    {
        Console.WriteLine("\n Sei arrivato in un villaggio, che si trova ai piedi di antiche rovine");
        bool nelVillaggio = true;

        while(nelVillaggio && giocatore.EInVita())
        {
            MostraMenuVillaggio();
            switch (Console.ReadLine())
            {
                case "1":
                    giocatore.MostraScheda();
                    break;
                case "2":
                    bool vittoria = AffrontaGoblin(giocatore);
                    if (!vittoria)
                    {
                        nelVillaggio = false;
                    }
                    break;
                case "5":
                    giocatore.MostraInventario();
                    break;
                case "0":
                    nelVillaggio = false;
                    Console.WriteLine("Lasci il villaggio.");
                    break;
                default:
                    Console.WriteLine("Scelta non valida");
                    break;
            }
        }
    }


    private void MostraMenuVillaggio()
    {
        Console.WriteLine("\n === VILLAGGIO DI VALDORIA === ");
        Console.WriteLine("1. Mostra la scheda Personaggio");
        Console.WriteLine("2. Affronta il Goblin (allenamento )");
        Console.WriteLine("3. Negozio");
        Console.WriteLine("4. Missioni");
        Console.WriteLine("5. Inventario");
        Console.WriteLine("0. Termina la partita");
        Console.WriteLine("Scelta: ");
    }

    private bool AffrontaGoblin(Giocatore giocatore)
    {
        Nemico goblin = new Nemico("Goblin Esploratore", 60, 10, 9, 3, 1, 1, 1);
        Combattimento combattimento = new Combattimento(giocatore, goblin);

        bool vittoria = combattimento.Inizia();

        if (vittoria)
        {
            Console.WriteLine("Hai protetto il villaggio di Valdoria");
            Console.WriteLine("I tuoi HP non vengono rigenerati");
        }
        else
        {
            Console.WriteLine("Il tuo eore é stato sconfitto. Partita terminata");
            
        }
        return vittoria;

    }
}

