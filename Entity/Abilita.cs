using System;

class Abilita
{
    private string nome;
    private int costoMana;
    private int livelloRichiesto;
    private int bonusDanno;
    private bool magica;
    private TipoEffetto effetto;
    private int durataEffetto;
    private int probabilitaEffetto;


    public Abilita(string nome, int costoMana, int livelloRichiesto, int bonusDanno, bool magica, 
        TipoEffetto effetto, int durataEffetto, int probabilitaEffetto)
    {
        this.nome = nome;
        this.costoMana = costoMana;
        this.livelloRichiesto = livelloRichiesto;
        this.bonusDanno = bonusDanno;
        this.magica = magica;
        this.effetto = effetto;
        this.durataEffetto = durataEffetto;
        this.probabilitaEffetto = probabilitaEffetto;

    }

    public void Mostra(int numero, int livelloGiocatore)
    {
        string tipo = "fisica";
        if (magica)
        {
            tipo = "magica";
        }

        Console.WriteLine($"{numero} {nome} | {costoMana} MP | bonus {bonusDanno} ({tipo})");
        
        if(effetto != TipoEffetto.Nessuno)
        {
            Console.WriteLine($" | {probabilitaEffetto} % | {effetto} ({durataEffetto} turni)");
        }

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
        return giocatore.AttaccaConAbilita(bersaglio, casuale, nome, costoMana, bonusDanno, magica, effetto, durataEffetto, probabilitaEffetto);

    }
}