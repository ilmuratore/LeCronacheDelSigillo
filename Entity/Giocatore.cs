using System;
using System.Runtime.InteropServices.Marshalling;


class Giocatore : Personaggio
{
    private TipoClasse tipoClasse;
    private int puntiDisponibili;
    private int livello;

    private List<Abilita> abilita;

    private List<Oggetto> inventario;

    public Giocatore(string nome, TipoClasse tipoClasse) 
        : base(nome, 80,30,10,5,2,5,5)
    {
        this.tipoClasse = tipoClasse;
        puntiDisponibili = 5;
        livello = 1;
        abilita = new List<Abilita>();
        inventario = new List<Oggetto>();
        inventario.Add(new Oggetto("Pozione di salute", TipoOggetto.PozioneVita, 30));
        inventario.Add(new Oggetto("Pozione di salute", TipoOggetto.PozioneVita, 30));
        inventario.Add(new Oggetto("Pozione di mana", TipoOggetto.PozioneMana, 20));
        inventario.Add(new Oggetto("Bomba incendiaria", TipoOggetto.BombaIncendiaria, 12));
        inventario.Add(new Oggetto("Elisir della Forza", TipoOggetto.ElisirForza, 5));

        switch (tipoClasse)
        {
            case TipoClasse.Guerriero:
                vitaMassima += 40;
                attacco += 5;
                difesa += 3;
                armatura += 3;
                abilita.Add(new Abilita("Colpo potente", 8, 1, 10, false, TipoEffetto.Stordimento, 2, 60));
                abilita.Add(new Abilita("Fendente potente", 20, 3, 20, false, TipoEffetto.Stordimento, 4, 50));
                break;
            case TipoClasse.Mago:
                manaMassimo += 40;
                magia += 7;
                agilita += 1;
                abilita.Add(new Abilita("Palla di fuoco", 30, 1, 15, true, TipoEffetto.Bruciatura, 2, 30));
                abilita.Add(new Abilita("Tempesta di fulmini", 30, 3, 25, true, TipoEffetto.Stordimento, 4, 40));
                break;
            case TipoClasse.Ladro:
                vitaMassima += 10;
                manaMassimo += 10;
                attacco += 3;
                difesa += 1;
                magia += 1;
                agilita += 7;
                abilita.Add(new Abilita("Affondo rapido", 20, 1, 10, false, TipoEffetto.Veleno, 2, 40));
                abilita.Add(new Abilita("Furtivo", 25, 3, 18, false, TipoEffetto.Veleno, 4, 50));
                break;
        }

        vita = vitaMassima;
        mana = manaMassimo;
    }

    public void MostraScheda()
    {
        Console.WriteLine($"\n Classe: {this.tipoClasse}");
        Console.WriteLine($"Livello : {livello}");
        MostraStato();
        Console.WriteLine($"Punti abilita da distribuire : {this.puntiDisponibili}");
    }
    
    public void MostraAbilita()
    {
        Console.WriteLine($"\n === ABILITA DEL {tipoClasse.ToString().ToUpper()} ===");
        for(int i = 0; i < abilita.Count; i++)
        {
            abilita[i].Mostra(i + 1, livello);
        }
    }

    public void MostraInventario()
    {
        Console.WriteLine("\n === INVENTARIO ===");
        if(inventario.Count == 0)
        {
            Console.WriteLine("Inventario vuoto");
            return;
        }
        for (int i = 0; i < inventario.Count; i++)
        {
            inventario[i].Mostra(i + 1);
        }
    }

    public bool UsaAbilita(int numero, Personaggio bersaglio, Random casuale)
    {
        if( numero < 1 || numero > abilita.Count)
        {
            Console.WriteLine("Abilitá non valida.");
            return false;
        }
        return abilita[numero - 1].Esegui(this, bersaglio, casuale, livello);
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

    public bool UsaOggetto(int numero, Nemico nemico)
    {
        if(numero < 1 || numero > inventario.Count)
        {
            Console.WriteLine("Numero oggetto non valido.");
            return false;
        }
        Oggetto oggetto = inventario[numero - 1];
        if (!oggetto.Usa(this, nemico))
        {
            return false;
        }
        inventario.RemoveAt(numero - 1);
        return true;
         
    }
}