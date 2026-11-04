using System.Collections.Generic;

// Campi pubblici: JsonSerializerOptions.IncludeFields, senza proprieta get/set.
class DatiOggetto
{
    public string Nome = "";
    public TipoOggetto Tipo;
    public int Potenza, BonusAttacco, BonusArmatura, BonusMagia, Prezzo;
}
class DatiGiocatore
{
    public string Nome = "";
    public TipoClasse Classe;
    public int Vita, VitaMassima, Mana, ManaMassimo, Attacco, Difesa, Armatura, Magia, Agilita;
    public int PuntiIniziali, PuntiAbilita, Livello, Esperienza, Monete;
    public List<DatiOggetto> Inventario = new List<DatiOggetto>();
    public DatiOggetto? Arma, ArmaturaEqu, Accessorio;
    public Dictionary<string, int> GradiAbilita = new Dictionary<string, int>();
}
class DatiMissione
{
    public string Id = "";
    public StatoMissione Stato;
    public int Progresso;
}
class DatiPartita
{
    public int Versione = 1;
    public DatiGiocatore Giocatore = new DatiGiocatore();
    public List<DatiMissione> Missioni = new List<DatiMissione>();
}
