using System.IO;
using System;

class Oggetto
{
    private string nome;
    private TipoOggetto tipo;
    private int potenza;
    private int bonusAttacco;
    private int bonusArmatura;
    private int bonusMagia;
    private int prezzo;

    // Costruttore dello Step 13: mantiene invariati tutti i consumabili.
    public Oggetto(string nome, TipoOggetto tipo, int potenza)
    {
        this.nome = nome;
        this.tipo = tipo;
        this.potenza = potenza;
        bonusAttacco = 0;
        bonusArmatura = 0;
        bonusMagia = 0;
        prezzo = 0;
    }

    // Overload: gli oggetti equipaggiabili hanno bonus permanenti.
    public Oggetto(string nome, TipoOggetto tipo,
                   int bonusAttacco, int bonusArmatura, int bonusMagia)
    {
        this.nome = nome;
        this.tipo = tipo;
        potenza = 0;
        this.bonusAttacco = bonusAttacco;
        this.bonusArmatura = bonusArmatura;
        this.bonusMagia = bonusMagia;
        prezzo = 0;
    }

    // Overload per un consumabile venduto nel negozio.
    public Oggetto(string nome, TipoOggetto tipo, int potenza, int prezzo)
    {
        this.nome = nome;
        this.tipo = tipo;
        this.potenza = potenza;
        bonusAttacco = 0;
        bonusArmatura = 0;
        bonusMagia = 0;
        this.prezzo = prezzo;
    }

    // Overload per un equipaggiabile venduto nel negozio.
    public Oggetto(string nome, TipoOggetto tipo,
                   int bonusAttacco, int bonusArmatura, int bonusMagia, int prezzo)
    {
        this.nome = nome;
        this.tipo = tipo;
        potenza = 0;
        this.bonusAttacco = bonusAttacco;
        this.bonusArmatura = bonusArmatura;
        this.bonusMagia = bonusMagia;
        this.prezzo = prezzo;
    }

    public DatiOggetto EsportaDati()
    {
        return new DatiOggetto {
            Nome = nome, Tipo = tipo, Potenza = potenza, BonusAttacco = bonusAttacco,
            BonusArmatura = bonusArmatura, BonusMagia = bonusMagia, Prezzo = prezzo
        };
    }

    public static Oggetto DaDati(DatiOggetto dati)
    {
        if (dati == null || string.IsNullOrWhiteSpace(dati.Nome))
            throw new InvalidDataException("Oggetto del salvataggio non valido.");
        if (!Enum.IsDefined(typeof(TipoOggetto), dati.Tipo))
            throw new InvalidDataException("Tipo oggetto non valido.");
        if (dati.Potenza < 0 || dati.BonusAttacco < 0 || dati.BonusArmatura < 0 ||
            dati.BonusMagia < 0 || dati.Prezzo < 0)
            throw new InvalidDataException("Valore oggetto negativo.");
        if (dati.Tipo == TipoOggetto.Arma || dati.Tipo == TipoOggetto.Armatura ||
            dati.Tipo == TipoOggetto.Accessorio)
            return new Oggetto(dati.Nome, dati.Tipo, dati.BonusAttacco,
                               dati.BonusArmatura, dati.BonusMagia, dati.Prezzo);
        return new Oggetto(dati.Nome, dati.Tipo, dati.Potenza, dati.Prezzo);
    }

    public int LeggiPrezzo()
    {
        return prezzo;
    }

    public void MostraInNegozio(int numero)
    {
        Console.WriteLine(numero + ". " + nome + " (" + Descrizione() + ") - " + prezzo + " monete");
    }

    // Il catalogo e un modello: l'acquirente riceve una nuova istanza.
    public Oggetto CreaCopia()
    {
        if (EEquipaggiabile())
        {
            return new Oggetto(nome, tipo, bonusAttacco, bonusArmatura, bonusMagia, prezzo);
        }
        return new Oggetto(nome, tipo, potenza, prezzo);
    }

    private string Descrizione()
    {
        switch (tipo)
        {
            case TipoOggetto.PozioneVita:
                return "+" + potenza + " HP";
            case TipoOggetto.PozioneMana:
                return "+" + potenza + " MP";
            case TipoOggetto.BombaIncendiaria:
                return potenza + " danni diretti + bruciatura";
            case TipoOggetto.ElisirForza:
                return "+" + potenza + " a 2 attacchi fisici";
            case TipoOggetto.Arma:
                return "arma, +" + bonusAttacco + " Attacco";
            case TipoOggetto.Armatura:
                return "armatura, +" + bonusArmatura + " Armatura";
            case TipoOggetto.Accessorio:
                return "accessorio, +" + bonusMagia + " Magia";
            default:
                return "tipo sconosciuto";
        }
    }

    public void Mostra(int numero)
    {
        Console.WriteLine(numero + ". " + nome + " (" + Descrizione() + ")");
    }

    public void MostraEquipaggiato()
    {
        Console.WriteLine(nome + " (" + Descrizione() + ")");
    }

    public bool EEquipaggiabile()
    {
        return tipo == TipoOggetto.Arma || tipo == TipoOggetto.Armatura ||
               tipo == TipoOggetto.Accessorio;
    }

    public bool EPerSlot(TipoOggetto slot)
    {
        return EEquipaggiabile() && tipo == slot;
    }

    // Incapsuliamo qui l'applicazione e la rimozione dei bonus dell'oggetto.
    public void ApplicaBonus(Giocatore giocatore, bool equipaggia)
    {
        if (!EEquipaggiabile())
        {
            return;
        }

        int segno = -1;
        if (equipaggia)
        {
            segno = 1;
        }
        giocatore.ModificaBonusEquipaggiamento(bonusAttacco * segno,
                                               bonusArmatura * segno,
                                               bonusMagia * segno);
    }

    // Restituisce true soltanto se il consumabile e stato realmente utilizzato.
    public bool Usa(Giocatore giocatore, Personaggio bersaglio)
    {
        switch (tipo)
        {
            case TipoOggetto.PozioneVita:
                if (!giocatore.PuoCurarsi())
                {
                    Console.WriteLine("Non puoi usare la pozione: HP gia pieni.");
                    return false;
                }
                giocatore.Cura(potenza);
                Console.WriteLine("Usi " + nome + ". Recuperi HP fino al massimo consentito.");
                return true;

            case TipoOggetto.PozioneMana:
                if (!giocatore.PuoRecuperareMana())
                {
                    Console.WriteLine("Non puoi usare la pozione: MP gia pieni.");
                    return false;
                }
                giocatore.RecuperaMana(potenza);
                Console.WriteLine("Usi " + nome + ". Recuperi MP fino al massimo consentito.");
                return true;

            case TipoOggetto.BombaIncendiaria:
                if (!bersaglio.EInVita())
                {
                    return false;
                }
                // I danni diretti della bomba ignorano armatura, schivata e critici.
                bersaglio.SubisciDanno(potenza);
                Console.WriteLine("Lanci " + nome + " e infliggi " + potenza + " danni diretti!");
                if (bersaglio.EInVita())
                {
                    bersaglio.ApplicaEffetto(TipoEffetto.Bruciatura, 2);
                }
                return true;

            case TipoOggetto.ElisirForza:
                if (!giocatore.AttivaBonusFisico(potenza, 2))
                {
                    Console.WriteLine("Hai gia un elisir della forza attivo.");
                    return false;
                }
                Console.WriteLine("Usi " + nome + ": +" + potenza +
                                  " ai prossimi 2 attacchi fisici.");
                return true;

            case TipoOggetto.Arma:
            case TipoOggetto.Armatura:
            case TipoOggetto.Accessorio:
                Console.WriteLine("Questo oggetto si equipaggia nel villaggio, non in combattimento.");
                return false;
        }

        return false;
    }
}
