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
        Console.WriteLine("Premi INVIO per tornare al menu");
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
}

