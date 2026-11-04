using System;
using System.IO;
using System.Text.Json;

class Program
{
    static int Main(string[] args)
    {
        if (args.Length > 0) return ContentRunner.Esegui(args[0]);
        try
        {
            Catalogo catalogo = Catalogo.Carica();
            new Gioco(catalogo).Avvia();
            return 0;
        }
        catch (Exception ex) when (ex is IOException || ex is JsonException ||
                                   ex is InvalidDataException || ex is UnauthorizedAccessException)
        {
            Console.Error.WriteLine("Impossibile avviare il gioco: " + ex.Message);
            return 2;
        }
    }
}
