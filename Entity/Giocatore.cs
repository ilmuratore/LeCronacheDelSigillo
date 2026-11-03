using System;

class Giocatore : Personaggio
{
    private TipoClasse tipoClasse;
    private int puntiDisponibili;
    private int livello;
    private int monete;
    private Oggetto? armaEquipaggiata;
    private Oggetto? armaturaEquipaggiata;
    private Oggetto? accessorioEquipaggiato;
    private List<Abilita> abilita;
    private List<Oggetto> inventario;

    public Giocatore(string nome, TipoClasse tipoClasse) 
        : base(nome, 80,30,10,5,2,5,5)
    {
        this.tipoClasse = tipoClasse;
        puntiDisponibili = 5;
        livello = 1;
        monete = 50;
        abilita = new List<Abilita>();
        inventario = new List<Oggetto>();
        
        armaEquipaggiata = null;
        armaturaEquipaggiata = null;
        accessorioEquipaggiato = null;

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
    public void MostraEquipaggiamento()
    {
        Console.WriteLine("\n === EQUIPAGGIAMENTO ===");
        Console.Write("Arma: ");
        if (armaEquipaggiata == null)
        {
            Console.WriteLine("nessuna");
        }
        else
        {
            armaEquipaggiata.MostraEquipaggiato();
        }
        Console.Write("Armatura: ");
        if (armaturaEquipaggiata == null)
        {
            Console.WriteLine("nessuna");
        }
        else
        {
            armaturaEquipaggiata.MostraEquipaggiato();
        }
        Console.Write("Accessorio: ");
        if (accessorioEquipaggiato == null)
        {
            Console.WriteLine("nessuna");
        }
        else
        {
            accessorioEquipaggiato.MostraEquipaggiato();
        }


    }
    public void MostraMonete()
    {
        Console.WriteLine($"Monete disponibili :{monete}");
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
    public void ModificaBonusEquipaggiamento(int bonusAttacco, int bonusArmatura, int bonusMagia)
    {
        attacco += bonusAttacco;
        armatura += bonusArmatura;
        magia += bonusMagia;
    }
    public bool Equipaggia(int numero)
    {
        if (numero < 1 || numero > inventario.Count)
        {
            Console.WriteLine("Numero oggetto non valido");
            return false;
        }
        Oggetto nuovo = inventario[numero - 1];
        if (!nuovo.EEquipaggiabile())
        {
            Console.WriteLine("Questo oggetto é un consumabile, non é possibile equipaggiarlo");
            return false;
        }
        Oggetto? precedente = null;
        if (nuovo.EPerSlot(TipoOggetto.Arma))
        {
            precedente = armaEquipaggiata;
            armaEquipaggiata = nuovo;
        }
        else if (nuovo.EPerSlot(TipoOggetto.Armatura))
        {
            precedente = armaturaEquipaggiata;
            armaturaEquipaggiata = nuovo;
        }
        else if (nuovo.EPerSlot(TipoOggetto.Accessorio))
        {
            precedente = accessorioEquipaggiato;
            accessorioEquipaggiato = nuovo;
        }

        inventario.RemoveAt(numero - 1);
        if (precedente != null)
        {
            precedente.ApplicaBonus(this, false);
            inventario.Add(precedente);
        }

        nuovo.ApplicaBonus(this, true);
        Console.WriteLine("Equipaggiamento aggiornato");
        return true;
    }
    public bool AcquistaOggetto(Oggetto articolo)
    {
        int prezzo = articolo.LeggiPrezzo();
        if(prezzo <= 0)
        {
            Console.WriteLine("Articolo non acquistabile");
            return false;
        }
        if (monete < prezzo)
        {
            Console.WriteLine($"Monete insufficienti. Ti mancano {prezzo - monete} monete");
            return false;
        }
        Oggetto comprato = articolo.CreaCopia();
        inventario.Add(comprato);
        monete -= prezzo;
        Console.WriteLine($"Acquisto completato. Hai speso {prezzo}");
        MostraMonete();
        return true;
    }


}