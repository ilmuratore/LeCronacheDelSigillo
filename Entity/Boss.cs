using System;
class Boss : Nemico
{
    private bool secondaFaseBoss;

    public Boss()
        : base("Custode Corrotto", 200, 100, 40, 30, 15, 30, 40)
    {
        secondaFaseBoss = false;
    }

    public override void Presentati()
    {
        Console.WriteLine("\n === BOSS FINALE: CUSTODE CORROTTO === ");
        Console.WriteLine("Il custode protegge la chiave per Aprire il Sigillo");
        Console.WriteLine("Fase 1: il Custode combatte e ti attacca con la fiamma Nera");
    }

    public bool ESecondaFase()
    {
        return vita <= vitaMassima / 2;
    }

    public override void EseguiTurno(Giocatore giocatore, Random casuale)
    {
        if(!EInVita() || !giocatore.EInVita())
        {
            return;
        }

        if (ESecondaFase())
        {
            if (!secondaFaseBoss)
            {
                secondaFaseBoss = true;
                Console.WriteLine("\n Il Custode é avvolto da Fiamme Nere");
                Console.WriteLine(" Fase 2: Il Custode si é rivestito delle Fiamme Nere, i suoi attacchi sono devastanti");
            }
            if(casuale.Next(0, 100) < 55)
            {
                Console.WriteLine("Il Custode usa Fendente Corrotto della Fiamma Nera");
                int danno = EseguiAttacco(giocatore, casuale, attacco + 20);
                if(danno > 0 && giocatore.EInVita() && casuale.Next(0 ,100) < 35)
                {
                    giocatore.ApplicaEffetto(TipoEffetto.Stordimento, 4);
                }
            }
            else
            {
                Attacca(giocatore, casuale);
            }
        }
        else
        {
            if(casuale.Next(0, 100) < 40 && ConsumaMana(10))
            {
                Console.WriteLine("Il Custode lancia Fiamma Nera! (- 10MP )");
                int danno = EseguiAttacco(giocatore, casuale, magia + 20);
                if (danno > 0 && giocatore.EInVita() && casuale.Next(0, 100) < 35)
                {
                    giocatore.ApplicaEffetto(TipoEffetto.Bruciatura, 3);
                }
            }
            else
            {
                Attacca(giocatore, casuale);
            }
        }
    }
}