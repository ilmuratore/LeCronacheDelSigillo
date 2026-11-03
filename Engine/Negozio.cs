class Negozio
{
    private List<Oggetto> catalogo;

    public Negozio()
    {
        catalogo = new List<Oggetto>();
        catalogo.Add(new Oggetto("Pozione di salute", TipoOggetto.PozioneVita, 30, 15));
        catalogo.Add(new Oggetto("Pozione di mana", TipoOggetto.PozioneMana, 20, 15));
        catalogo.Add(new Oggetto("Bomba incendiaria", TipoOggetto.BombaIncendiaria, 12, 22));
        catalogo.Add(new Oggetto("Elisir della Forza", TipoOggetto.ElisirForza, 5, 25));
        catalogo.Add(new Oggetto("Spada de fero", TipoOggetto.Arma, 5,0,0, 25));
        catalogo.Add(new Oggetto("Ascia riforzata", TipoOggetto.Arma,7,0,0,40));
        catalogo.Add(new Oggetto("Corazza di cuoio", TipoOggetto.Armatura, 0,5,0, 34));
        catalogo.Add(new Oggetto("Amuleto", TipoOggetto.Accessorio, 0,0,10, 50));
    }

    private void MostraCatalogo()
    {
        Console.WriteLine("\n === NEGOZIO DI VALDORIA === ");
        for (int i = 0; i < catalogo.Count; i++)
        {
            catalogo[i].MostraInNegozio(i + 1);
        }
        Console.WriteLine("0. Torna al villaggio");
    }

    public void Apri(Giocatore giocatore)
    {
        bool nelNegozio = true;
        while (nelNegozio)
        {
            MostraCatalogo();
            giocatore.MostraMonete();
            Console.WriteLine("Oggetto da acquistare: ");

            string? risposta = Console.ReadLine();
            if (!int.TryParse(risposta, out int numero))
            {
                Console.WriteLine("Inserisci numero oggetto valido.");
                continue;
            }
            if (numero == 0)
            {
                nelNegozio = false;
            }
            else if(numero < 1 || numero > catalogo.Count)
            {
                Console.WriteLine("Articolo inesistente");
            }
            else
            {
                giocatore.AcquistaOggetto(catalogo[numero - 1]);
            }
        }
    }
}