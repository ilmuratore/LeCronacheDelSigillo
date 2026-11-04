using System;
using System.Collections.Generic;

class Negozio
{
    private List<Oggetto> catalogo = new List<Oggetto>();
    private string nome;
    public Negozio(Catalogo contenuti, string idNegozio)
    {
        DefinizioneNegozio negozio = contenuti.TrovaNegozio(idNegozio);
        nome = negozio.Nome;
        foreach (string id in negozio.Oggetti) catalogo.Add(contenuti.CreaOggetto(id));
    }
    public void Apri(Giocatore giocatore)
    {
        while (true)
        {
            Schermata.Titolo(nome);
            for (int i = 0; i < catalogo.Count; i++) catalogo[i].MostraInNegozio(i + 1);
            Console.WriteLine("0. Indietro");
            giocatore.MostraMonete();
            Console.Write("Acquista: ");
            if (!int.TryParse(Console.ReadLine(), out int n)) return;
            if (n == 0) return;
            if (n < 1 || n > catalogo.Count) Console.WriteLine("Scelta errata.");
            else giocatore.AcquistaOggetto(catalogo[n - 1]);
            Schermata.Pausa();
        }
    }
}
