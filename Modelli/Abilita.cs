using System;

class Abilita
{
    private string id;
    private string nome;
    private int costoMana;
    private int livelloRichiesto;
    private int bonusDanno;
    private bool magica;
    private TipoEffetto effetto;
    private int durataEffetto;
    private int probabilitaEffetto;
    private int livelloPotenziamento;

    public Abilita(string id, string nome, int costoMana, int livelloRichiesto,
                   int bonusDanno, bool magica, TipoEffetto effetto,
                   int durataEffetto, int probabilitaEffetto)
    {
        this.id = id;
        this.nome = nome;
        this.costoMana = costoMana;
        this.livelloRichiesto = livelloRichiesto;
        this.bonusDanno = bonusDanno;
        this.magica = magica;
        this.effetto = effetto;
        this.durataEffetto = durataEffetto;
        this.probabilitaEffetto = probabilitaEffetto;
        livelloPotenziamento = 1;
    }

    public string LeggiId() { return id; }
    public int LeggiLivelloPotenziamento() { return livelloPotenziamento; }

    public bool ImpostaPotenziamento(int nuovoLivello)
    {
        if (nuovoLivello < 1 || nuovoLivello > 3) return false;
        livelloPotenziamento = nuovoLivello;
        return true;
    }

    public bool Potenzia(int livelloGiocatore)
    {
        if (livelloGiocatore < livelloRichiesto || livelloPotenziamento >= 3)
            return false;
        livelloPotenziamento++;
        return true;
    }

    public Abilita CreaCopia()
    {
        Abilita copia = new Abilita(id, nome, costoMana, livelloRichiesto, bonusDanno,
                                    magica, effetto, durataEffetto, probabilitaEffetto);
        copia.livelloPotenziamento = livelloPotenziamento;
        return copia;
    }

    public void Mostra(int numero, int livelloGiocatore)
    {
        string tipo = "fisica";
        if (magica)
        {
            tipo = "magica";
        }

        Console.Write(numero + ". " + nome + " [grado " + livelloPotenziamento + "/3]"
                      + " | " + costoMana + " MP"
                      + " | bonus +" + (bonusDanno + 3 * (livelloPotenziamento - 1))
                      + " (" + tipo + ")");

        if (effetto != TipoEffetto.Nessuno)
        {
            Console.Write(" | " + probabilitaEffetto + "% " + effetto +
                          " (" + durataEffetto + " turni)");
        }

        if (livelloGiocatore < livelloRichiesto)
        {
            Console.Write(" | BLOCCATA: livello " + livelloRichiesto);
        }

        Console.WriteLine();
    }

    public bool Esegui(Giocatore giocatore, Personaggio bersaglio,
                       Random casuale, int livelloGiocatore)
    {
        if (livelloGiocatore < livelloRichiesto)
        {
            Console.WriteLine("Abilita bloccata: serve il livello " + livelloRichiesto + ".");
            return false;
        }

        return giocatore.AttaccaConAbilita(bersaglio, casuale, nome,
                                          costoMana, bonusDanno + 3 * (livelloPotenziamento - 1), magica,
                                          effetto, durataEffetto, probabilitaEffetto);
    }
}
