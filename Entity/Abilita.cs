using System;

class Abilita
{
    private string nome;
    private int costoMana;
    private int livelloRichiesto;
    private int bonusDanno;
    private bool magica;

    public Abilita(string nome, int costoMana, int livelloRichiesto, int bonusDanno, bool magica)
    {
        this.nome = nome;
        this.costoMana = costoMana;
        this.livelloRichiesto = livelloRichiesto;
        this.bonusDanno = bonusDanno;
        this.magica = magica;

    }

    public void Mostra(int numero, int livelloGiocatore)
    {
        string tipo = "fisica";
        if (magica)
        {
            tipo = "magica";
        }

        Console.WriteLine($"{numero} {nome} | {costoMana} MP | bonus {bonusDanno} ({tipo})");
        
        if(livelloGiocatore < livelloRichiesto)
        {
            Console.WriteLine($" | BLOCCATI: livello:  {livelloRichiesto}");
        }

        Console.WriteLine();
    }

    public bool Esegui(Giocatore giocatore, Personaggio bersaglio, Random casuale, int livelloGiocatore)
    {
        if (livelloGiocatore < livelloRichiesto)
        {
            Console.WriteLine($"Abilitá bloccata: livello minimo : {livelloRichiesto}");
            return false;
        }
        return giocatore.AttaccaConAbilita(bersaglio, casuale, nome, costoMana, bonusDanno, magica);

    }
}