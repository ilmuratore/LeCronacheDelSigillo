using System;

class Checkpoint
{
    private Giocatore fotografia;

    public Checkpoint(Giocatore giocatore)
    {
        fotografia = giocatore.CreaCopiaCompleta();
    }
    public void Ripristina(Giocatore giocatore)
    {
        giocatore.RipristinaDa(fotografia);
        Console.WriteLine("Checkpoint ripristinato");
    }

    public void MostraConferma()
    {
        Console.WriteLine("Checkpoint acquisito");
    }

}