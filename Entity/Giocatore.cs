using System;


class Giocatore : Personaggio
{
    private TipoClasse tipoClasse;
    private int puntiDisponibili;

    public Giocatore(string nome, TipoClasse tipoClasse) 
        : base(nome, 80,30,10,5,2,5,5)
    {
        this.tipoClasse = tipoClasse;
        puntiDisponibili = 5;

        switch (tipoClasse)
        {
            case TipoClasse.Guerriero:
                vitaMassima += 40;
                attacco += 5;
                difesa += 3;
                armatura += 3;
                break;
            case TipoClasse.Mago:
                manaMassimo += 40;
                magia += 7;
                agilita += 1;
                break;
            case TipoClasse.Ladro:
                vitaMassima += 10;
                manaMassimo += 10;
                attacco += 3;
                difesa += 1;
                magia += 1;
                agilita += 7;
                break;
        }

        vita = vitaMassima;
        mana = manaMassimo;
    }

    public void MostraScheda()
    {
        Console.WriteLine($"\n Classe: {this.tipoClasse}");
        MostraStato();
        Console.WriteLine($"Punti abilita da distribuire : {this.puntiDisponibili}");
    }
    
    public int GetPuntiDisponibili()
    {
        return puntiDisponibili;
    }

    public bool HaPuntiDaDistribuire()
    {
        return puntiDisponibili > 0;
    }

    public bool AssegnaPunti(int scelta)
    {
        if (!HaPuntiDaDistribuire())
        {
            return false;
        }

        switch (scelta)
        {
            case 1:
                vitaMassima += 10;
                vita += 10;
                break;
            case 2:
                manaMassimo += 5;
                mana += 5;
                break;
            case 3:
                attacco += 1;
                break;
            case 4:
                difesa += 1;
                break;
            case 5:
                magia += 1;
                break;
            case 6:
                agilita += 1;
                break;
            default:
                return false;
        }

        puntiDisponibili--;
        return true;
    }
}