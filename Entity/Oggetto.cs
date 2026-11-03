using System;

class Oggetto
{
    private string nome;
    private TipoOggetto tipo;
    private int potenza;

    public Oggetto(string nome, TipoOggetto tipo, int potenza)
    {
        this.nome = nome;
        this.tipo = tipo;
        this.potenza = potenza;
    }

    public void Mostra(int numero)
    {
        string descrizione = "";
        switch (tipo)
        {
            case TipoOggetto.PozioneVita:
                descrizione = $"+ {potenza} HP";
                break;
            case TipoOggetto.PozioneMana:
                descrizione = $"+ {potenza} MP";
                break;
            case TipoOggetto.BombaIncendiaria:
                descrizione = $"{potenza} danni diretti + bruciatura";
                break;
            case TipoOggetto.ElisirForza:
                descrizione = $"+ {potenza} a 2 attacchi fisici";
                break;
        }
        Console.WriteLine($"{numero}. {nome} ({descrizione})");
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
        }
        return false;
    }
}