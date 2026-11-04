using System;

// STEP 26 EXTRA: un unico punto in cui gestire le schermate del terminale.
class Schermata
{
    public static void Pulisci()
    {
        // Non provare a pulire l'output quando viene reindirizzato a un file.
        if (!Console.IsOutputRedirected)
        {
            Console.Clear();
        }
    }

    public static void Titolo(string titolo)
    {
        Pulisci();
        Console.WriteLine("==================================================");
        Console.WriteLine("  " + titolo);
        Console.WriteLine("==================================================");
        Console.WriteLine();
    }

    public static void Pausa()
    {
        // I test automatizzati possono inviare input reindirizzato: niente pause extra.
        if (!Console.IsInputRedirected)
        {
            Console.WriteLine();
            Console.Write("Premi INVIO per continuare...");
            Console.ReadLine();
        }
    }
}
