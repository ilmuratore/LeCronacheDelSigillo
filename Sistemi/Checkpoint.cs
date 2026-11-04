using System;

// STEP 23: il checkpoint vive soltanto in memoria, non su file.
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
        Console.WriteLine("Stato del giocatore ripristinato dal checkpoint.");
    }

    public void MostraConferma()
    {
        Console.WriteLine("Checkpoint acquisito prima dello scontro (solo in memoria).");
    }
}
