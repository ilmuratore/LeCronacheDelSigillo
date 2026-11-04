using System.Collections.Generic;

// Definizioni statiche lette dai JSON. Nessuno stato di partita qui.
class DefinizioneNemico
{
    public string Id = "", Nome = "", DropOggettoId = "";
    public bool Boss;
    public int HP, MP, Attacco, Difesa, Armatura, Magia, Agilita;
    public int Monete, XP, ProbabilitaDrop;
}
class DefinizioneMissione
{
    public string Id = "", Titolo = "", Descrizione = "", NemicoId = "";
    public string PrerequisitoId = "", NPCId = "", Categoria = "Secondaria";
    public bool Finale;
    public int Quantita = 1, PremioMonete, PremioXP;
}
class DefinizioneNPC
{
    public string Id = "", Nome = "";
}
class DefinizioneDialogo
{
    public string NPCId = "", Stato = "", Testo = "";
}
class DefinizioneOggetto
{
    public string Id = "", Nome = "";
    public TipoOggetto Tipo;
    public int Potenza, BonusAttacco, BonusArmatura, BonusMagia, Prezzo;
}
class DefinizioneNegozio
{
    public string Id = "", Nome = "";
    public List<string> Oggetti = new List<string>();
}
class DefinizioneAbilita
{
    public string Id = "", Classe = "", Nome = "";
    public int CostoMana, LivelloRichiesto, BonusDanno;
    public bool Magica;
    public TipoEffetto Effetto;
    public int DurataEffetto, ProbabilitaEffetto;
}

class DefinizioneOggettoIniziale
{
    public string OggettoId = "";
    public int Quantita;
}
