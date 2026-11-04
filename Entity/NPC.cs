using System;

class NPC
{
    private string nome;
    private Missione missione;

    public NPC(string nome, Missione missione)
    {
        this.nome = nome;
        this.missione = missione;
    }
    public void Parla(Giocatore giocatore)
    {
        Parla(giocatore, missione);
    }

    public void Parla(Giocatore giocatore, Missione incarico)
    {

        Console.WriteLine($"\n === DIALOGO CON {nome} ===");
        switch (incarico.LeggiStato())
        {
            case StatoMissione.NonIniziata:
                Console.WriteLine($"{nome} : {incarico.LeggiDescrizione()}");
                Console.WriteLine($"Obiettivo: {incarico.LeggiObiettivo()}");
                Console.WriteLine("1. Accetta l'incarico");
                Console.WriteLine("0. Non adesso.");
                Console.WriteLine("Scelta: ");
                if (Console.ReadLine() == "1")
                {
                    incarico.Accetta();
                }
                else
                {
                    Console.WriteLine($"{nome} : Torna quando sarai pronto.");
                }
                break;
            case StatoMissione.InCorso:
                if (incarico.EProntaPerConsegna())
                {
                    Console.WriteLine($"{nome}: Complimenti puoi completare la missione.");
                    Console.WriteLine("1. Completa la missione.");
                    Console.WriteLine("0. Torna al villagigo.");
                    Console.WriteLine("Scelta: ");
                    if (Console.ReadLine() == "1")
                    {
                        incarico.Consegna(giocatore);
                    }
                }
                else
                {
                    Console.WriteLine($"{nome} : Non hai ancora completato la missione {incarico.LeggiTitolo()}. Ritorna quando avrai finito.");
                }
                break;
            case StatoMissione.Completata:
                Console.WriteLine($"{nome}: Grazie per il tuo aiuto.");
                break;
        }
    }
}