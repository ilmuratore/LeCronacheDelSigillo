using System;

class Combattimento
{
    private Giocatore giocatore;
    private Nemico nemico;
    private Random casuale;
    private bool terminato;

    public Combattimento(Giocatore giocatore, Nemico nemico)
    {
        this.giocatore = giocatore;
        this.nemico = nemico;
        casuale = new Random();
        terminato = false;
    }

    public bool Inizia()
    {
        if (terminato)
        {
            Console.WriteLine("Questo combattimento e gia terminato.");
            return false;
        }

        Schermata.Titolo("INCONTRO PVE");
        nemico.Presentati();
        Schermata.Pausa();

        while (giocatore.EInVita() && nemico.EInVita())
        {
            Schermata.Titolo("COMBATTIMENTO PVE - TURNO DEL GIOCATORE");
            bool giocatoreStordito = giocatore.IniziaTurno();

            // Il veleno o la bruciatura possono causare la sconfitta prima dell'azione.
            if (!giocatore.EInVita())
            {
                break;
            }

            giocatore.MostraStatoBreve();
            nemico.MostraStatoBreve();

            if (!giocatoreStordito)
            {
                bool azioneCompletata = false;

                // Scelte non valide non ricominciano il turno e non applicano di nuovo gli effetti.
                while (!azioneCompletata)
                {
                    Console.WriteLine("\n1. Attacca");
                    Console.WriteLine("2. Difenditi");
                    Console.WriteLine("3. Abilita speciale");
                    Console.WriteLine("4. Usa oggetto");
                    Console.Write("Scelta: ");

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
                            Console.WriteLine("Azione non valida. Non perdi il turno.");
                            break;
                    }
                }
            }

            if (!nemico.EInVita())
            {
                break;
            }

            Console.WriteLine("\n--- TURNO DEL NEMICO ---");
            bool nemicoStordito = nemico.IniziaTurno();
            if (!nemico.EInVita())
            {
                break;
            }
            if (!nemicoStordito)
            {
                nemico.EseguiTurno(giocatore, casuale);
            }
            Schermata.Pausa();
        }

        bool vittoria = giocatore.EInVita();
        terminato = true;

        // Gli effetti temporanei e la difesa attiva non continuano in villaggio.
        giocatore.TerminaCombattimento();
        nemico.TerminaCombattimento();

        if (vittoria)
        {
            Console.WriteLine("\nHai vinto il combattimento!");
            AssegnaRicompense();
            return true;
        }

        Console.WriteLine("\nSei stato sconfitto.");
        return false;
    }

    // Chiamato solo dopo una vittoria e una sola volta per combattimento.
    private void AssegnaRicompense()
    {
        Console.WriteLine("\n--- RICOMPENSE ---");
        giocatore.RiceviMonete(nemico.LeggiMoneteRicompensa());
        giocatore.RiceviEsperienza(nemico.LeggiEsperienzaRicompensa());

        Oggetto? trovato = nemico.EstraiOggetto(casuale);
        if (trovato == null)
        {
            Console.WriteLine("Non hai trovato oggetti.");
        }
        else
        {
            giocatore.RiceviOggetto(trovato);
        }
    }

    private bool ScegliOggetto()
    {
        giocatore.MostraInventario();
        Console.WriteLine("0. Torna alle azioni");
        Console.Write("Numero oggetto: ");

        string? risposta = Console.ReadLine();
        if (!int.TryParse(risposta, out int numero))
        {
            Console.WriteLine("Devi inserire un numero. Non perdi il turno.");
            return false;
        }
        if (numero == 0)
        {
            Console.WriteLine("Torni al menu delle azioni.");
            return false;
        }

        return giocatore.UsaOggetto(numero, nemico);
    }

    private bool ScegliAbilita()
    {
        giocatore.MostraAbilita();
        Console.WriteLine("0. Torna alle azioni");
        Console.Write("Scelta abilita: ");

        switch (Console.ReadLine())
        {
            case "1":
                return giocatore.UsaAbilita(1, nemico, casuale);
            case "2":
                return giocatore.UsaAbilita(2, nemico, casuale);
            case "0":
                Console.WriteLine("Torni al menu delle azioni.");
                return false;
            default:
                Console.WriteLine("Scelta non valida: non perdi il turno.");
                return false;
        }
    }
}
