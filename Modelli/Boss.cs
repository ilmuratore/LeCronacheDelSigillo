using System;

// Il Boss e un Nemico: eredita HP, attacco, difesa, effetti e ricompense.
class Boss : Nemico
{
    private bool secondaFaseAnnunciata;

    public Boss()
        : base("Custode Corrotto", 120, 30, 14, 5, 2, 13, 6, 60, 100)
    {
        secondaFaseAnnunciata = false;
    }

    public Boss(DefinizioneNemico d, Catalogo catalogo) : base(d, catalogo)
    {
        secondaFaseAnnunciata = false;
    }

    // La rivincita deve ripartire dalla fase 1, anche dopo aver visto la fase 2.
    public override void PreparaRivincita()
    {
        base.PreparaRivincita();
        secondaFaseAnnunciata = false;
    }

    public override void Presentati()
    {
        Console.WriteLine("\n=== BOSS FINALE: CUSTODE CORROTTO ===");
        Console.WriteLine("Il Custode protegge l'ultimo frammento del Sigillo!");
        Console.WriteLine("Fase 1: il Custode combatte e lancia Fiamma Nera.");
    }

    // In fase 2 quando gli HP scendono a meta o meno del massimo.
    public bool ESecondaFase()
    {
        return vita <= vitaMassima / 2;
    }

    public override void EseguiTurno(Giocatore giocatore, Random casuale)
    {
        if (!EInVita() || !giocatore.EInVita())
        {
            return;
        }

        if (ESecondaFase())
        {
            if (!secondaFaseAnnunciata)
            {
                secondaFaseAnnunciata = true;
                Console.WriteLine("\nIL CUSTODE CAMBIA FORMA!");
                Console.WriteLine("Fase 2: i suoi colpi diventano piu violenti!");
            }

            // Seconda fase: 55% di probabilita di un attacco fisico speciale.
            if (casuale.Next(0, 100) < 55)
            {
                Console.WriteLine("Il Custode usa Fendente Corrotto!");
                int danno = EseguiAttacco(giocatore, casuale, attacco + 6);
                if (danno > 0 && giocatore.EInVita() && casuale.Next(0, 100) < 35)
                {
                    giocatore.ApplicaEffetto(TipoEffetto.Stordimento, 1);
                }
            }
            else
            {
                Attacca(giocatore, casuale);
            }
        }
        else
        {
            // Prima fase: 40% di probabilita di tentare un incantesimo.
            // Se il mana non basta, esegue un attacco normale.
            if (casuale.Next(0, 100) < 40 && ConsumaMana(6))
            {
                Console.WriteLine("Il Custode lancia Fiamma Nera! (-6 MP)");
                int danno = EseguiAttacco(giocatore, casuale, magia + 5);
                if (danno > 0 && giocatore.EInVita() && casuale.Next(0, 100) < 60)
                {
                    giocatore.ApplicaEffetto(TipoEffetto.Bruciatura, 2);
                }
            }
            else
            {
                Attacca(giocatore, casuale);
            }
        }
    }
}
