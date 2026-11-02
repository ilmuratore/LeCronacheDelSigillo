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
        mana -= quantita;
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

    public void Attacca(Personaggio bersaglio)
    {
        if(!EInVita() || !bersaglio.EInVita())
        {
            return;
        }

        int danno = attacco - bersaglio.difesa - bersaglio.armatura;

        if(danno < 1)
        {
            danno = 1;
        }

        if (bersaglio.difesaAttiva)
        {
            danno -= 5;

            if(danno < 0)
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
}
