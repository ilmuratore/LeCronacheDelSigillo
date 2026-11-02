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
        Console.WriteLine("Premi INVIO per tornare al menú");
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
}

