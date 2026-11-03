using System;
using System.ComponentModel;

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


    protected Personaggio(string nome, int vitaMassima,
        int manaMassimo, int attacco, int difesa, int armatura, int magia, int agilita)
    {
        this.nome = nome;
        this.vitaMassima = vitaMassima;
        this.manaMassimo = manaMassimo;
        this.attacco = attacco;
        this.difesa = difesa;
        this.armatura = armatura;
        this.magia = magia;
        this.agilita = agilita;
        effetti = new List<EffettoStato>();
    }

    public void MostraStato()
    {
        Console.WriteLine($" === {this.nome} ===");
        Console.WriteLine($"HP: {this.vita} / {this.vitaMassima}");
        Console.WriteLine($"MP: {this.mana} / {this.manaMassimo}");
        Console.WriteLine($"Attacco: {this.attacco}");
        Console.WriteLine($"Difesa: {this.difesa}");
        Console.WriteLine($"Armatura: {this.armatura}");
        Console.WriteLine($"Magia: {this.magia}");
        Console.WriteLine($"Agilitá : {this.agilita}");
        for(int i = 0; i < effetti.Count; i++)
        {
            effetti[i].Mostra();
        }
    }

    public bool EInVita()
    {
        return vita > 0;
    }

    public void SubisciDanno(int danno)
    {
        if (danno < 0)
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
        mana = mana - quantita;
        return true;
    }

    public void RecuperaMana(int quantita)
    {
        if(quantita <= 0 || !EInVita())
        {
            return;
        }

        mana += quantita;

        if(mana > manaMassimo)
        {
            mana = manaMassimo;
        }
    }

    public void Attacca(Personaggio bersaglio, Random casuale)
    {
        EseguiAttacco(bersaglio, casuale, attacco);
    }

    public bool AttaccaConAbilita(Personaggio bersaglio, Random casuale, string nomeAbilita, int costoMana, int bonusDanno, bool usaMagia)
    {
        if (!EInVita() || !bersaglio.EInVita())
        {
            return false;
        }

        if (!ConsumaMana(costoMana))
        {
            Console.WriteLine("Mana insufficiente: 'l'abilitá non viene lanciata");
            return false;
        }
        int potenza = attacco;
        if (usaMagia)
        {
            potenza = magia;
        }

        potenza += bonusDanno;
        Console.WriteLine($"{nome} usa {nomeAbilita} ! ( - {costoMana} MP )");
        EseguiAttacco(bersaglio, casuale, potenza);
        return true;
    }

    private void EseguiAttacco(Personaggio bersaglio, Random casuale, int potenza)
    {
        if (!EInVita() || !bersaglio.EInVita())
        {
            return;
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
            Console.WriteLine($"{bersaglio.nome} schiva l'attacco di {nome} !");
            return;
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
            Console.WriteLine($"Colpo CRITICO di {nome} , {danno}  danni inflitti!");
        }

        if (bersaglio.difesaAttiva)
        {
            danno -= 5;

            if (danno < 0)
            {
                danno = 0;
            }
            bersaglio.difesaAttiva = false;
        }

        bersaglio.SubisciDanno(danno);
        Console.WriteLine($"{nome} attacca {bersaglio.nome} e infligge {danno} danni");
    }

    public void Difenditi()
    {
        if (!EInVita())
        {
            return;
        }
        difesaAttiva = true;
        Console.WriteLine($"{nome} si mette in difesa: danni minori al prossimo attacco");

    }

    public void ApplicaEffetto(TipoEffetto tipo, int durata)
    {
        if (!EInVita() || tipo == TipoEffetto.Nessuno || durata <= 0)
        {
            return;
        }

        for(int i = 0; i < effetti.Count; i++)
        {
            if (effetti[i].EDiTipo(tipo))
            {
                effetti[i].Rinnova(durata);
                Console.WriteLine($"Effetto {tipo} rinnovato");
                return;
            }
        }
        effetti.Add(new EffettoStato(tipo, durata));
        Console.WriteLine($"{nome} subisce l'effetto {tipo} per {durata} turni");
    }

    public bool IniziaTurno()
    {
        bool saltaAzione = false;

        for(int i = 0; i < effetti.Count; i++)
        {
            if(EInVita() && effetti[i].ApplicaAllInizioDelTurno(this))
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
    }
}
