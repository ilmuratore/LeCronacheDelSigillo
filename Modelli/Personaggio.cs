using System;
using System.Collections.Generic;

abstract class Personaggio
{
    protected string nome;
    protected int vita;
    protected int vitaMassima;
    protected int mana;
    protected int manaMassimo;
    protected int attacco;
    protected int difesa;
    protected int armatura;
    protected int magia;
    protected int agilita;
    private bool difesaAttiva;
    private List<EffettoStato> effetti;
    private int bonusFisicoTemporaneo;
    private int attacchiFisiciRimanenti;

    protected Personaggio(string nome, int vitaMassima, int manaMassimo,
                          int attacco, int difesa, int armatura, int magia, int agilita)
    {
        this.nome = nome;
        this.vitaMassima = vitaMassima;
        vita = vitaMassima;
        this.manaMassimo = manaMassimo;
        mana = manaMassimo;
        this.attacco = attacco;
        this.difesa = difesa;
        this.armatura = armatura;
        this.magia = magia;
        this.agilita = agilita;
        effetti = new List<EffettoStato>();
        bonusFisicoTemporaneo = 0;
        attacchiFisiciRimanenti = 0;
    }

    public string LeggiNome()
    {
        return nome;
    }

    public void MostraStato()
    {
        Console.WriteLine("=== " + nome + " ===");
        Console.WriteLine("HP: " + vita + "/" + vitaMassima);
        Console.WriteLine("MP: " + mana + "/" + manaMassimo);
        Console.WriteLine("Attacco: " + attacco);
        Console.WriteLine("Difesa: " + difesa);
        Console.WriteLine("Armatura: " + armatura);
        Console.WriteLine("Magia: " + magia);
        Console.WriteLine("Agilita: " + agilita);
        if (attacchiFisiciRimanenti > 0)
        {
            Console.WriteLine("Elisir forza: +" + bonusFisicoTemporaneo +
                              " per " + attacchiFisiciRimanenti + " attacchi fisici");
        }

        for (int i = 0; i < effetti.Count; i++)
        {
            effetti[i].Mostra();
        }
    }

    // STEP 26: riepilogo compatto, adatto alle schermate del combattimento.
    public void MostraStatoBreve()
    {
        Console.WriteLine(nome + " | HP " + vita + "/" + vitaMassima +
                          " | MP " + mana + "/" + manaMassimo);
    }

    public void Attacca(Personaggio bersaglio, Random casuale)
    {
        if (!EInVita() || !bersaglio.EInVita())
        {
            return;
        }

        int potenza = attacco + ConsumaBonusFisico();
        EseguiAttacco(bersaglio, casuale, potenza);
    }

    public bool AttaccaConAbilita(Personaggio bersaglio, Random casuale,
                                 string nomeAbilita, int costoMana,
                                 int bonusDanno, bool usaMagia,
                                 TipoEffetto tipoEffetto, int durataEffetto,
                                 int probabilitaEffetto)
    {
        if (!EInVita() || !bersaglio.EInVita())
        {
            return false;
        }

        if (!ConsumaMana(costoMana))
        {
            Console.WriteLine("Mana insufficiente: l'abilita non viene utilizzata.");
            return false;
        }

        int potenza = attacco;
        if (usaMagia)
        {
            potenza = magia;
        }

        potenza += bonusDanno;
        if (!usaMagia)
        {
            potenza += ConsumaBonusFisico();
        }
        Console.WriteLine(nome + " usa " + nomeAbilita + "! (-" + costoMana + " MP)");
        int danno = EseguiAttacco(bersaglio, casuale, potenza);

        // Un effetto si applica soltanto se l'attacco fa danno e il bersaglio sopravvive.
        if (danno > 0 && bersaglio.EInVita() && tipoEffetto != TipoEffetto.Nessuno &&
            casuale.Next(0, 100) < probabilitaEffetto)
        {
            bersaglio.ApplicaEffetto(tipoEffetto, durataEffetto);
        }

        return true;
    }

    // Restituisce il danno realmente inflitto: 0 se l'attacco non colpisce.
    protected int EseguiAttacco(Personaggio bersaglio, Random casuale, int potenza)
    {
        if (!EInVita() || !bersaglio.EInVita())
        {
            return 0;
        }

        int probabilitaSchivata = bersaglio.agilita;
        if (probabilitaSchivata < 0)
        {
            probabilitaSchivata = 0;
        }
        if (probabilitaSchivata > 30)
        {
            probabilitaSchivata = 30;
        }

        if (casuale.Next(0, 100) < probabilitaSchivata)
        {
            Console.WriteLine(bersaglio.nome + " schiva l'attacco di " + nome + "!");
            // Una schivata non consuma la difesa attiva del bersaglio.
            return 0;
        }

        int probabilitaCritico = 5 + agilita;
        if (probabilitaCritico < 0)
        {
            probabilitaCritico = 0;
        }
        if (probabilitaCritico > 35)
        {
            probabilitaCritico = 35;
        }
        bool critico = casuale.Next(0, 100) < probabilitaCritico;

        int danno = potenza - bersaglio.difesa - bersaglio.armatura;
        if (danno < 1)
        {
            danno = 1;
        }
        if (critico)
        {
            danno *= 2;
            Console.WriteLine("COLPO CRITICO di " + nome + "!");
        }

        if (bersaglio.difesaAttiva)
        {
            danno -= 4;
            if (danno < 0)
            {
                danno = 0;
            }
            bersaglio.difesaAttiva = false;
        }

        bersaglio.SubisciDanno(danno);
        Console.WriteLine(nome + " attacca " + bersaglio.nome +
                          " e infligge " + danno + " danni calcolati.");
        return danno;
    }

    public void ApplicaEffetto(TipoEffetto tipo, int durata)
    {
        if (!EInVita() || tipo == TipoEffetto.Nessuno || durata <= 0)
        {
            return;
        }

        for (int i = 0; i < effetti.Count; i++)
        {
            if (effetti[i].EDiTipo(tipo))
            {
                effetti[i].Rinnova(durata);
                Console.WriteLine("Effetto " + tipo + " rinnovato.");
                return;
            }
        }

        effetti.Add(new EffettoStato(tipo, durata));
        Console.WriteLine(nome + " subisce l'effetto " + tipo + " per " + durata + " turni!");
    }

    // Gli effetti si applicano una sola volta per turno, prima della scelta dell'azione.
    public bool IniziaTurno()
    {
        bool saltaAzione = false;

        for (int i = effetti.Count - 1; i >= 0; i--)
        {
            if (EInVita() && effetti[i].ApplicaAllInizioTurno(this))
            {
                saltaAzione = true;
            }

            if (effetti[i].EScaduto())
            {
                effetti.RemoveAt(i);
            }
        }

        return saltaAzione;
    }

    public void TerminaCombattimento()
    {
        effetti.Clear();
        difesaAttiva = false;
        bonusFisicoTemporaneo = 0;
        attacchiFisiciRimanenti = 0;
    }

    public bool PuoCurarsi()
    {
        return EInVita() && vita < vitaMassima;
    }

    public bool PuoRecuperareMana()
    {
        return EInVita() && mana < manaMassimo;
    }

    public bool AttivaBonusFisico(int bonus, int numeroAttacchi)
    {
        if (!EInVita() || bonus <= 0 || numeroAttacchi <= 0 || attacchiFisiciRimanenti > 0)
        {
            return false;
        }

        bonusFisicoTemporaneo = bonus;
        attacchiFisiciRimanenti = numeroAttacchi;
        return true;
    }

    private int ConsumaBonusFisico()
    {
        if (attacchiFisiciRimanenti <= 0)
        {
            return 0;
        }

        int bonus = bonusFisicoTemporaneo;
        attacchiFisiciRimanenti--;
        if (attacchiFisiciRimanenti == 0)
        {
            bonusFisicoTemporaneo = 0;
        }
        return bonus;
    }

    public void Difenditi()
    {
        if (!EInVita())
        {
            return;
        }

        difesaAttiva = true;
        Console.WriteLine(nome + " si mette in difesa: -4 danni dal prossimo attacco.");
    }

    public bool EInVita()
    {
        return vita > 0;
    }

    public void SubisciDanno(int danno)
    {
        if (danno <= 0)
        {
            return;
        }

        vita -= danno;
        if (vita < 0)
        {
            vita = 0;
        }
    }

    public void Cura(int quantita)
    {
        if (quantita <= 0 || !EInVita())
        {
            return;
        }

        vita += quantita;
        if (vita > vitaMassima)
        {
            vita = vitaMassima;
        }
    }

    public bool ConsumaMana(int quantita)
    {
        if (quantita <= 0 || !EInVita() || mana < quantita)
        {
            return false;
        }

        mana -= quantita;
        return true;
    }

    public void RecuperaMana(int quantita)
    {
        if (quantita <= 0 || !EInVita())
        {
            return;
        }

        mana += quantita;
        if (mana > manaMassimo)
        {
            mana = manaMassimo;
        }
    }
}
