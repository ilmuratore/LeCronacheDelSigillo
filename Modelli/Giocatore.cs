using System.IO;
using System;
using System.Collections.Generic;

class Giocatore : Personaggio
{
    private TipoClasse tipoClasse;
    private int puntiDisponibili;
    private int livello;
    private int esperienza;
    private int puntiAbilita;
    private int monete;
    private List<Abilita> abilita;
    private List<Oggetto> inventario;
    private Oggetto? armaEquipaggiata;
    private Oggetto? armaturaEquipaggiata;
    private Oggetto? accessorioEquipaggiato;

    public Giocatore(string nome, TipoClasse tipoClasse)
        : base(nome, 80, 30, 10, 5, 2, 5, 5)
    {
        this.tipoClasse = tipoClasse;
        puntiDisponibili = 5;
        livello = 1;
        esperienza = 0;
        puntiAbilita = 0;
        monete = 50; // Capitale iniziale; le vittorie ora aggiungono monete.
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

    // La soglia aumenta con il livello: 40, 80, 120 XP, ecc.
    private int EsperienzaNecessaria()
    {
        return livello * 40;
    }

    // L'XP in eccesso resta disponibile dopo ogni passaggio di livello.
    public void RiceviEsperienza(int quantita)
    {
        if (quantita <= 0)
        {
            return;
        }

        esperienza += quantita;
        Console.WriteLine("Hai ottenuto " + quantita + " XP!");

        while (esperienza >= EsperienzaNecessaria())
        {
            int soglia = EsperienzaNecessaria();
            esperienza -= soglia;
            livello++;
            puntiAbilita++;

            vitaMassima += 10;
            manaMassimo += 5;
            attacco += 2;
            difesa += 1;
            magia += 2;
            agilita += 1;

            // Solo quando si sale di livello, HP e MP tornano al massimo.
            vita = vitaMassima;
            mana = manaMassimo;

            Console.WriteLine("PASSAGGIO DI LIVELLO! Ora sei al livello " + livello + ".");
            Console.WriteLine("Bonus: +10 HP max, +5 MP max, +2 Attacco, +1 Difesa, +2 Magia, +1 Agilita.");
            Console.WriteLine("HP e MP ripristinati al massimo.");

            if (livello == 3)
            {
                Console.WriteLine("Nuova abilita sbloccata! Controlla la seconda abilita del personaggio.");
            }
        }

        Console.WriteLine("Esperienza: " + esperienza + "/" + EsperienzaNecessaria() + " XP");
    }

    // Un premio positivo aggiorna il saldo; un valore non valido viene ignorato.
    public void RiceviMonete(int quantita)
    {
        if (quantita <= 0)
        {
            return;
        }

        monete += quantita;
        Console.WriteLine("Ottieni " + quantita + " monete!");
        MostraMonete();
    }

    // Il nemico crea un nuovo oggetto: lo aggiungiamo senza modificare i consumabili esistenti.
    public void RiceviOggetto(Oggetto oggetto)
    {
        inventario.Add(oggetto);
        Console.WriteLine("Hai trovato un oggetto:");
        oggetto.Mostra(inventario.Count);
    }

    public void MostraMonete()
    {
        Console.WriteLine("Monete disponibili: " + monete);
    }

    // Acquisto: si verifica il prezzo prima di modificare inventario e saldo.
    public bool AcquistaOggetto(Oggetto articolo)
    {
        int prezzo = articolo.LeggiPrezzo();
        if (prezzo <= 0)
        {
            Console.WriteLine("Articolo non acquistabile.");
            return false;
        }
        if (monete < prezzo)
        {
            Console.WriteLine("Monete insufficienti. Ti mancano " + (prezzo - monete) + " monete.");
            return false;
        }

        // Il catalogo del negozio resta intatto, anche dopo l'acquisto.
        Oggetto comprato = articolo.CreaCopia();
        inventario.Add(comprato);
        monete -= prezzo;
        Console.WriteLine("Acquisto completato! Hai speso " + prezzo + " monete.");
        MostraMonete();
        return true;
    }

    public int LeggiLivello() { return livello; }
    public int LeggiPuntiAbilita() { return puntiAbilita; }

    public bool PotenziaAbilita(int numero)
    {
        if (puntiAbilita <= 0 || numero < 1 || numero > abilita.Count)
            return false;
        if (!abilita[numero - 1].Potenzia(livello)) return false;
        puntiAbilita--;
        return true;
    }

    // Le abilita del catalogo sostituiscono quelle dimostrative nel costruttore.
    public void CaricaInventarioIniziale(Catalogo catalogo)
    {
        inventario.Clear();
        foreach (DefinizioneOggettoIniziale definizione in catalogo.InventarioIniziale)
            for (int i = 0; i < definizione.Quantita; i++)
                inventario.Add(catalogo.CreaOggetto(definizione.OggettoId));
    }

    public void CaricaAbilita(List<DefinizioneAbilita> definizioni)
    {
        abilita = new List<Abilita>();
        foreach (DefinizioneAbilita d in definizioni)
        {
            if (d.Classe != tipoClasse.ToString()) continue;
            abilita.Add(new Abilita(d.Id, d.Nome, d.CostoMana, d.LivelloRichiesto,
                d.BonusDanno, d.Magica, d.Effetto, d.DurataEffetto, d.ProbabilitaEffetto));
        }
    }

    public void MostraAbilita()
    {
        Console.WriteLine("\n--- ABILITA DEL " + tipoClasse.ToString().ToUpper() + " ---");
        Console.WriteLine("Punti abilita disponibili: " + puntiAbilita);
        for (int i = 0; i < abilita.Count; i++)
        {
            abilita[i].Mostra(i + 1, livello);
        }
    }

    public bool UsaAbilita(int numero, Personaggio bersaglio, Random casuale)
    {
        if (numero < 1 || numero > abilita.Count)
        {
            Console.WriteLine("Abilita non valida.");
            return false;
        }

        return abilita[numero - 1].Esegui(this, bersaglio, casuale, livello);
    }

    public void MostraInventario()
    {
        Console.WriteLine("\n--- INVENTARIO ---");
        if (inventario.Count == 0)
        {
            Console.WriteLine("Inventario vuoto.");
            return;
        }

        for (int i = 0; i < inventario.Count; i++)
        {
            inventario[i].Mostra(i + 1);
        }
    }

    public void MostraEquipaggiamento()
    {
        Console.WriteLine("\n--- EQUIPAGGIAMENTO ---");
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
            Console.WriteLine("nessuno");
        }
        else
        {
            accessorioEquipaggiato.MostraEquipaggiato();
        }
    }

    // Un solo oggetto per slot. La sostituzione restituisce il vecchio all'inventario.
    public bool Equipaggia(int numero)
    {
        if (numero < 1 || numero > inventario.Count)
        {
            Console.WriteLine("Numero oggetto non valido.");
            return false;
        }

        Oggetto nuovo = inventario[numero - 1];
        if (!nuovo.EEquipaggiabile())
        {
            Console.WriteLine("Questo oggetto e un consumabile e non si puo equipaggiare.");
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
        Console.WriteLine("Equipaggiamento aggiornato.");
        return true;
    }

    // Metodo chiamato da Oggetto per aggiungere o togliere i bonus dello slot.
    public void ModificaBonusEquipaggiamento(int bonusAttacco, int bonusArmatura, int bonusMagia)
    {
        attacco += bonusAttacco;
        armatura += bonusArmatura;
        magia += bonusMagia;
    }

    // Un oggetto viene rimosso soltanto quando l'uso e riuscito.
    public bool UsaOggetto(int numero, Personaggio bersaglio)
    {
        if (numero < 1 || numero > inventario.Count)
        {
            Console.WriteLine("Numero oggetto non valido.");
            return false;
        }

        Oggetto oggetto = inventario[numero - 1];
        if (!oggetto.Usa(this, bersaglio))
        {
            return false;
        }

        inventario.RemoveAt(numero - 1);
        return true;
    }

    public bool HaPuntiDaDistribuire()
    {
        return puntiDisponibili > 0;
    }

    public bool AssegnaPunto(int scelta)
    {
        if (puntiDisponibili <= 0)
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

    // STEP 23: fotografia completa del personaggio prima di un combattimento.
    // Ogni oggetto e copiato: modificare l'inventario originale non tocca il checkpoint.
    public Giocatore CreaCopiaCompleta()
    {
        Giocatore copia = new Giocatore(nome, tipoClasse);
        copia.vita = vita;
        copia.vitaMassima = vitaMassima;
        copia.mana = mana;
        copia.manaMassimo = manaMassimo;
        copia.attacco = attacco;
        copia.difesa = difesa;
        copia.armatura = armatura;
        copia.magia = magia;
        copia.agilita = agilita;
        copia.puntiDisponibili = puntiDisponibili;
        copia.livello = livello;
        copia.esperienza = esperienza;
        copia.puntiAbilita = puntiAbilita;
        copia.monete = monete;

        // Le abilita non cambiano stato durante il gioco: copiamo l'elenco.
        copia.abilita = new List<Abilita>();
        foreach (Abilita a in abilita) copia.abilita.Add(a.CreaCopia());
        copia.inventario = new List<Oggetto>();
        foreach (Oggetto oggetto in inventario)
        {
            copia.inventario.Add(oggetto.CreaCopia());
        }

        if (armaEquipaggiata != null)
        {
            copia.armaEquipaggiata = armaEquipaggiata.CreaCopia();
        }
        if (armaturaEquipaggiata != null)
        {
            copia.armaturaEquipaggiata = armaturaEquipaggiata.CreaCopia();
        }
        if (accessorioEquipaggiato != null)
        {
            copia.accessorioEquipaggiato = accessorioEquipaggiato.CreaCopia();
        }

        // Le statistiche gia includono i bonus equipaggiamento: non applicarli di nuovo.
        return copia;
    }

    // STEP 24: ripristino del personaggio dopo una sconfitta.
    // La fotografia non viene mai modificata: puo servire anche per il secondo tentativo.
    public void RipristinaDa(Giocatore fotografia)
    {
        nome = fotografia.nome;
        tipoClasse = fotografia.tipoClasse;
        vita = fotografia.vita;
        vitaMassima = fotografia.vitaMassima;
        mana = fotografia.mana;
        manaMassimo = fotografia.manaMassimo;
        attacco = fotografia.attacco;
        difesa = fotografia.difesa;
        armatura = fotografia.armatura;
        magia = fotografia.magia;
        agilita = fotografia.agilita;
        puntiDisponibili = fotografia.puntiDisponibili;
        livello = fotografia.livello;
        esperienza = fotografia.esperienza;
        puntiAbilita = fotografia.puntiAbilita;
        monete = fotografia.monete;
        abilita = new List<Abilita>();
        foreach (Abilita a in fotografia.abilita) abilita.Add(a.CreaCopia());

        inventario = new List<Oggetto>();
        foreach (Oggetto oggetto in fotografia.inventario)
        {
            inventario.Add(oggetto.CreaCopia());
        }

        armaEquipaggiata = null;
        armaturaEquipaggiata = null;
        accessorioEquipaggiato = null;
        if (fotografia.armaEquipaggiata != null)
        {
            armaEquipaggiata = fotografia.armaEquipaggiata.CreaCopia();
        }
        if (fotografia.armaturaEquipaggiata != null)
        {
            armaturaEquipaggiata = fotografia.armaturaEquipaggiata.CreaCopia();
        }
        if (fotografia.accessorioEquipaggiato != null)
        {
            accessorioEquipaggiato = fotografia.accessorioEquipaggiato.CreaCopia();
        }

        // Elimina eventuali effetti temporanei del tentativo fallito.
        TerminaCombattimento();
    }

    public DatiGiocatore EsportaDati()
    {
        DatiGiocatore d = new DatiGiocatore();
        d.Nome = nome; d.Classe = tipoClasse; d.Vita = vita;
        d.VitaMassima = vitaMassima; d.Mana = mana; d.ManaMassimo = manaMassimo;
        d.Attacco = attacco; d.Difesa = difesa; d.Armatura = armatura;
        d.Magia = magia; d.Agilita = agilita;
        d.PuntiIniziali = puntiDisponibili; d.PuntiAbilita = puntiAbilita;
        d.Livello = livello; d.Esperienza = esperienza; d.Monete = monete;
        foreach (Oggetto o in inventario) d.Inventario.Add(o.EsportaDati());
        if (armaEquipaggiata != null) d.Arma = armaEquipaggiata.EsportaDati();
        if (armaturaEquipaggiata != null) d.ArmaturaEqu = armaturaEquipaggiata.EsportaDati();
        if (accessorioEquipaggiato != null) d.Accessorio = accessorioEquipaggiato.EsportaDati();
        foreach (Abilita a in abilita) d.GradiAbilita.Add(a.LeggiId(), a.LeggiLivelloPotenziamento());
        return d;
    }

    public static Giocatore DaDati(DatiGiocatore d, Catalogo catalogo)
    {
        if (d == null || string.IsNullOrWhiteSpace(d.Nome) ||
            !Enum.IsDefined(typeof(TipoClasse), d.Classe) ||
            d.Livello < 1 || d.Livello > 1000 || d.Esperienza < 0 ||
            d.Esperienza >= d.Livello * 40 || d.Monete < 0 ||
            d.PuntiAbilita < 0 || d.PuntiIniziali < 0 || d.PuntiIniziali > 5 ||
            d.VitaMassima < 1 || d.ManaMassimo < 0 ||
            d.Vita < 1 || d.Vita > d.VitaMassima ||
            d.Mana < 0 || d.Mana > d.ManaMassimo ||
            d.Attacco < 0 || d.Difesa < 0 || d.Armatura < 0 ||
            d.Magia < 0 || d.Agilita < 0)
            throw new InvalidDataException("Dati personaggio non validi.");

        Giocatore g = new Giocatore(d.Nome, d.Classe);
        g.CaricaAbilita(catalogo.Abilita);
        g.vita = d.Vita; g.vitaMassima = d.VitaMassima;
        g.mana = d.Mana; g.manaMassimo = d.ManaMassimo;
        g.attacco = d.Attacco; g.difesa = d.Difesa;
        g.armatura = d.Armatura; g.magia = d.Magia; g.agilita = d.Agilita;
        g.puntiDisponibili = d.PuntiIniziali; g.puntiAbilita = d.PuntiAbilita;
        g.livello = d.Livello; g.esperienza = d.Esperienza; g.monete = d.Monete;
        g.inventario = new List<Oggetto>();
        if (d.Inventario == null || d.GradiAbilita == null ||
            d.GradiAbilita.Count != g.abilita.Count)
            throw new InvalidDataException("Inventario o abilita mancanti nel salvataggio.");
        foreach (DatiOggetto o in d.Inventario) g.inventario.Add(Oggetto.DaDati(o));
        if (d.Arma != null)
        {
            g.armaEquipaggiata = Oggetto.DaDati(d.Arma);
            if (!g.armaEquipaggiata.EPerSlot(TipoOggetto.Arma))
                throw new InvalidDataException("Slot arma errato.");
        }
        if (d.ArmaturaEqu != null)
        {
            g.armaturaEquipaggiata = Oggetto.DaDati(d.ArmaturaEqu);
            if (!g.armaturaEquipaggiata.EPerSlot(TipoOggetto.Armatura))
                throw new InvalidDataException("Slot armatura errato.");
        }
        if (d.Accessorio != null)
        {
            g.accessorioEquipaggiato = Oggetto.DaDati(d.Accessorio);
            if (!g.accessorioEquipaggiato.EPerSlot(TipoOggetto.Accessorio))
                throw new InvalidDataException("Slot accessorio errato.");
        }
        foreach (Abilita a in g.abilita)
        {
            if (!d.GradiAbilita.TryGetValue(a.LeggiId(), out int grado) ||
                !a.ImpostaPotenziamento(grado))
                throw new InvalidDataException("Grado abilita non valido: " + a.LeggiId());
        }

        // Le statistiche contengono gia i bonus degli oggetti: non riapplicarli.
        return g;
    }

    public void MostraScheda()
    {
        Console.WriteLine("\nClasse: " + tipoClasse);
        Console.WriteLine("Livello: " + livello);
        Console.WriteLine("Esperienza: " + esperienza + "/" + EsperienzaNecessaria() + " XP");
        MostraMonete();
        MostraStato();
        Console.WriteLine("Punti da distribuire: " + puntiDisponibili);
        Console.WriteLine("Punti abilita: " + puntiAbilita);
    }
}
