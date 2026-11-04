using System;

// EXTRA: due Giocatori umani, sulla stessa console, a turni alternati.
// Non assegna XP, monete o drop e non modifica il salvataggio del PvE.
class CombattimentoPvP
{
    private Giocatore primo;
    private Giocatore secondo;
    private Random casuale;

    public CombattimentoPvP(Giocatore primo, Giocatore secondo)
    {
        this.primo = primo;
        this.secondo = secondo;
        casuale = new Random();
    }

    public void Inizia()
    {
        bool turnoPrimo = true;
        int turno = 1;

        while (primo.EInVita() && secondo.EInVita())
        {
            Giocatore attivo = primo;
            Giocatore avversario = secondo;
            if (!turnoPrimo)
            {
                attivo = secondo;
                avversario = primo;
            }

            Schermata.Titolo("ARENA PVP - TURNO " + turno);
            Console.WriteLine("Tocca a " + attivo.LeggiNome());
            Console.WriteLine("Avversario: " + avversario.LeggiNome());
            Console.WriteLine();

            // Gli effetti vengono applicati UNA volta per turno, non a ogni input.
            bool stordito = attivo.IniziaTurno();
            if (!attivo.EInVita())
            {
                Schermata.Pausa();
                break;
            }

            attivo.MostraStatoBreve();
            avversario.MostraStatoBreve();

            if (!stordito)
            {
                bool azioneCompletata = false;
                while (!azioneCompletata)
                {
                    Console.WriteLine();
                    Console.WriteLine("1. Attacca");
                    Console.WriteLine("2. Difenditi");
                    Console.WriteLine("3. Abilita speciale");
                    Console.WriteLine("4. Usa oggetto");
                    Console.Write("Scelta di " + attivo.LeggiNome() + ": ");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            attivo.Attacca(avversario, casuale);
                            azioneCompletata = true;
                            break;
                        case "2":
                            attivo.Difenditi();
                            azioneCompletata = true;
                            break;
                        case "3":
                            azioneCompletata = ScegliAbilita(attivo, avversario);
                            break;
                        case "4":
                            azioneCompletata = ScegliOggetto(attivo, avversario);
                            break;
                        default:
                            Console.WriteLine("Scelta non valida: il turno non viene perso.");
                            break;
                    }
                }
            }

            // Lascia leggere il risultato prima di cancellare la schermata.
            Schermata.Pausa();
            turnoPrimo = !turnoPrimo;
            turno++;
        }

        Schermata.Titolo("RISULTATO DEL DUELLO");
        if (primo.EInVita())
        {
            Console.WriteLine("Vince " + primo.LeggiNome() + "!");
        }
        else if (secondo.EInVita())
        {
            Console.WriteLine("Vince " + secondo.LeggiNome() + "!");
        }
        else
        {
            Console.WriteLine("Pareggio: entrambi i giocatori sono stati sconfitti.");
        }

        // Gli effetti temporanei non restano dopo il duello.
        primo.TerminaCombattimento();
        secondo.TerminaCombattimento();
        Console.WriteLine("Nessuna ricompensa PvE e stata assegnata.");
        Schermata.Pausa();
    }

    private bool ScegliAbilita(Giocatore attivo, Giocatore avversario)
    {
        attivo.MostraAbilita();
        Console.WriteLine("0. Torna alle azioni");
        Console.Write("Numero abilita: ");
        string? risposta = Console.ReadLine();
        if (!int.TryParse(risposta, out int numero) || numero < 0 || numero > 2)
        {
            Console.WriteLine("Abilita non valida. Non perdi il turno.");
            return false;
        }
        if (numero == 0)
        {
            return false;
        }
        return attivo.UsaAbilita(numero, avversario, casuale);
    }

    private bool ScegliOggetto(Giocatore attivo, Giocatore avversario)
    {
        attivo.MostraInventario();
        Console.WriteLine("0. Torna alle azioni");
        Console.Write("Numero oggetto: ");
        string? risposta = Console.ReadLine();
        if (!int.TryParse(risposta, out int numero))
        {
            Console.WriteLine("Numero non valido. Non perdi il turno.");
            return false;
        }
        if (numero == 0)
        {
            return false;
        }
        return attivo.UsaOggetto(numero, avversario);
    }
}
