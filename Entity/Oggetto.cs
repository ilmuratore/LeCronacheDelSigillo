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
    public Oggetto(string nome, TipoOggetto tipo, int bonusAttacco, int bonusArmatura, int bonusMagia, int prezzo)
    {
        this.nome = nome;
        this.tipo = tipo;
        this.potenza = 0;
        this.bonusAttacco = bonusAttacco;
        this.bonusArmatura = bonusArmatura;
        this.bonusMagia = bonusMagia;
        this.prezzo = prezzo;
    }

    public void Mostra(int numero)
    {
        Console.WriteLine($"{numero}. {nome} ({Descrizione()})");
    }
    public void MostraEquipaggiato()
    {
        Console.WriteLine($"{nome} ({Descrizione()})");
    }
    private string Descrizione()
    {
        switch (tipo)
        {
            case TipoOggetto.PozioneVita:
                return $"+ {potenza} HP";
            case TipoOggetto.PozioneMana:
                return $"+ {potenza} MP";
            case TipoOggetto.BombaIncendiaria:
                return $"{potenza} danni diretti + bruciatura";
            case TipoOggetto.ElisirForza:
                return $"+ {potenza} a 2 attacchi fisici";
            case TipoOggetto.Arma:
                return $"+ {bonusAttacco} Attacco";
            case TipoOggetto.Armatura:
                return $"+ {bonusArmatura} Armatura";
            case TipoOggetto.Accessorio:
                return $"+ {bonusMagia} Magia";
            default:
                return "tipo sconosciuto";
        }
    }
    public bool Usa(Giocatore giocatore, Nemico nemico)
    {
        switch (tipo)
        {
            case TipoOggetto.PozioneVita:
                if (!giocatore.PuoCurarsi())
                {
                    Console.WriteLine($"Non puoi usare la pozione: HP al massimo");
                    return false;
                }
                giocatore.Cura(potenza);
                Console.WriteLine($"Usi {nome}. Recuperi {potenza} HP fino al massimo consetito.");
                return true;
            case TipoOggetto.PozioneMana:
                if (!giocatore.PuoRecuperareMana())
                {
                    Console.WriteLine($"Non puoi usare la pozione: Mana al massimo");
                    return false;
                }
                giocatore.RecuperaMana(potenza);
                Console.WriteLine($"Usi {nome}. Recuperi {potenza} punti Mana fino al massimo consetito.");
                return true;
            case TipoOggetto.BombaIncendiaria:
                if (!nemico.EInVita())
                {
                    return false;
                }
                nemico.SubisciDanno(potenza);
                Console.WriteLine($"Usi {nome}. Il nemico subisce {potenza} danni.");
                if (nemico.EInVita())
                {
                    nemico.ApplicaEffetto(TipoEffetto.Bruciatura, 2);
                }
                return true;
            case TipoOggetto.ElisirForza:
                if (!giocatore.AttivaBonusFisico(potenza, 2))
                {
                    Console.WriteLine($"Elisir della forza giá attivo");
                    return false;
                }
                Console.WriteLine($"Usi {nome}, per i prossimi due attacchi + {potenza} danno.");
                return true;
            case TipoOggetto.Arma:
            case TipoOggetto.Armatura:
            case TipoOggetto.Accessorio:
                Console.WriteLine("Questo oggetto si equipaggia nel villaggio. Azione non consentita");
                return false;
        }
        return false;
    }
    public bool EEquipaggiabile()
    {
        return tipo == TipoOggetto.Arma || tipo == TipoOggetto.Armatura || tipo == TipoOggetto.Accessorio;
    }
    public bool EPerSlot(TipoOggetto slot)
    {
        return EEquipaggiabile() && tipo == slot;
    }
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
        giocatore.ModificaBonusEquipaggiamento(bonusAttacco * segno, bonusArmatura * segno, bonusMagia * segno);
    }
    public int LeggiPrezzo()
    {
        return prezzo;
    }
    public void MostraInNegozio(int numero)
    {
        Console.WriteLine($"{numero}. {nome} ({Descrizione()}) - {prezzo} monete");
    }
    public Oggetto CreaCopia()
    {
        if (EEquipaggiabile())
        {
            return new Oggetto(nome, tipo, bonusAttacco, bonusArmatura, bonusMagia, prezzo);
        }
        return new Oggetto(nome, tipo, potenza, prezzo);
    }
}